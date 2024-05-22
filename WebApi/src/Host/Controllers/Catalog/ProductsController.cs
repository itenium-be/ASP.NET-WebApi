using FSH.WebApi.Application.Catalog.Products;
using FSH.WebApi.Application.Common.Persistence;
using FSH.WebApi.Domain.Catalog;
using Microsoft.Extensions.Localization;
using System.Threading;
using Ardalis.Specification;
using FSH.WebApi.Application.Common.Exceptions;
using FSH.WebApi.Application.Common.FileStorage;
using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Common;
using FSH.WebApi.Application.Common.Exporters;

namespace FSH.WebApi.Host.Controllers.Catalog;

public class ProductsController : VersionedApiController
{
    private readonly IRepository<Product> _repository;
    private readonly IStringLocalizer _t;
    private readonly IDapperRepository _dapper;
    private readonly IFileStorageService _file;
    private readonly IExcelWriter _excelWriter;

    public ProductsController(IRepository<Product> repository, IStringLocalizer t, IDapperRepository dapper, IFileStorageService file, IExcelWriter excelWriter)
    {
        _repository = repository;
        _t = t;
        _dapper = dapper;
        _file = file;
        _excelWriter = excelWriter;
    }

    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Products)]
    [OpenApiOperation("Search products using available filters.", "")]
    public Task<PaginationResponse<ProductDto>> Search(SearchProductsRequest request, CancellationToken cancellationToken)
    {
        var spec = new ProductsBySearchRequestWithBrandsSpec(request);
        return _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken: cancellationToken);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.Products)]
    [OpenApiOperation("Get product details.", "")]
    public async Task<ProductDetailsDto> Get(Guid id, CancellationToken cancellationToken)
    {
        return await _repository.FirstOrDefaultAsync(
                   (ISpecification<Product, ProductDetailsDto>)new ProductByIdWithBrandSpec(id), cancellationToken)
               ?? throw new NotFoundException(_t["Product {0} Not Found.", id]);
    }

    [HttpGet("dapper")]
    [MustHavePermission(FSHAction.View, FSHResource.Products)]
    [OpenApiOperation("Get product details via dapper.", "")]
    public async Task<ProductDto> GetDapper(Guid id, CancellationToken cancellationToken)
    {
        var product = await _dapper.QueryFirstOrDefaultAsync<Product>(
            $"SELECT * FROM Catalog.\"Products\" WHERE \"Id\"  = '{id}' AND \"TenantId\" = '@tenant'", cancellationToken: cancellationToken);

        _ = product ?? throw new NotFoundException(_t["Product {0} Not Found.", id]);

        // Using mapster here throws a nullreference exception because of the "BrandName" property
        // in ProductDto and the product not having a Brand assigned.
        return new ProductDto
        {
            Id = product.Id,
            BrandId = product.BrandId,
            BrandName = string.Empty,
            Description = product.Description,
            ImagePath = product.ImagePath,
            Name = product.Name,
            Rate = product.Rate
        };
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Products)]
    [OpenApiOperation("Create a new product.", "")]
    public async Task<Guid> Create(CreateProductRequest request, CancellationToken cancellationToken)
    {
        string productImagePath = await _file.UploadAsync<Product>(request.Image, FileType.Image, cancellationToken);
        var product = new Product(request.Name, request.Description, request.Rate, request.BrandId, productImagePath);

        // Add Domain Events to be raised after the commit
        product.DomainEvents.Add(EntityCreatedEvent.WithEntity(product));

        await _repository.AddAsync(product, cancellationToken);
        return product.Id;
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Products)]
    [OpenApiOperation("Update a product.", "")]
    public async Task<ActionResult<Guid>> Update(UpdateProductRequest request, Guid id, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest();

        var product = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = product ?? throw new NotFoundException(_t["Product {0} Not Found.", request.Id]);

        // Remove old image if flag is set
        if (request.DeleteCurrentImage)
        {
            string? currentProductImagePath = product.ImagePath;
            if (!string.IsNullOrEmpty(currentProductImagePath))
            {
                string root = Directory.GetCurrentDirectory();
                _file.Remove(Path.Combine(root, currentProductImagePath));
            }

            product = product.ClearImagePath();
        }

        string? productImagePath = request.Image is not null
            ? await _file.UploadAsync<Product>(request.Image, FileType.Image, cancellationToken)
            : null;

        var updatedProduct = product.Update(request.Name, request.Description, request.Rate, request.BrandId, productImagePath);

        // Add Domain Events to be raised after the commit
        product.DomainEvents.Add(EntityUpdatedEvent.WithEntity(product));

        await _repository.UpdateAsync(updatedProduct, cancellationToken);

        return request.Id;
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Products)]
    [OpenApiOperation("Delete a product.", "")]
    public async Task<Guid> Delete(Guid id, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(id, cancellationToken);
        _ = product ?? throw new NotFoundException(_t["Product {0} Not Found."]);

        // Add Domain Events to be raised after the commit
        product.DomainEvents.Add(EntityDeletedEvent.WithEntity(product));

        await _repository.DeleteAsync(product, cancellationToken);
        return id;
    }

    [HttpPost("export")]
    [MustHavePermission(FSHAction.Export, FSHResource.Products)]
    [OpenApiOperation("Export a products.", "")]
    public async Task<FileResult> Export(ExportProductsRequest filter, CancellationToken cancellationToken)
    {
        var spec = new ExportProductsWithBrandsSpecification(filter);
        var list = await _repository.ListAsync(spec, cancellationToken);
        var result = _excelWriter.WriteToStream(list);
        return File(result, "application/octet-stream", "ProductExports");
    }
}
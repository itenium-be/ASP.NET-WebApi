using FSH.WebApi.Application.Catalog.Brands;
using FSH.WebApi.Application.Catalog.Products;
using FSH.WebApi.Application.Common.Exceptions;
using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Application.Common.Persistence;
using FSH.WebApi.Domain.Catalog;
using FSH.WebApi.Infrastructure.Common;
using Microsoft.Extensions.Localization;

namespace FSH.WebApi.Host.Controllers.Catalog;

public class BrandsController : VersionedApiController
{
    private readonly IRepositoryWithEvents<Brand> _repository;
    private readonly IStringLocalizer _t;
    private readonly IReadRepository<Product> _productRepo;
    private readonly IJobService _jobService;

    public BrandsController(
        IRepositoryWithEvents<Brand> repository,
        IStringLocalizer<BrandsController> localizer,
        IReadRepository<Product> productRepo,
        IJobService jobService)
    {
        _repository = repository;
        _t = localizer;
        _productRepo = productRepo;
        _jobService = jobService;
    }

    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search brands using available filters.", "")]
    public Task<PaginationResponse<BrandDto>> Search(SearchBrandsRequest request, CancellationToken cancellationToken)
    {
        var spec = new BrandsBySearchRequestSpec(request);
        return _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get brand details.", "")]
    public async Task<BrandDto> Get(Guid id, CancellationToken cancellationToken)
    {
        var result = await _repository.FirstOrDefaultAsync(new BrandByIdSpec(id), cancellationToken);
        if (result == null)
            // TODO: the po files are generated I hope? It included the namespace, so prolly won't work anymore now?
            throw new NotFoundException(_t["Brand {0} Not Found.", id]);

        return result;
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new brand.", "")]
    public async Task<Guid> Create(CreateBrandRequest request, CancellationToken cancellationToken)
    {
        var brand = new Brand(request.Name, request.Description, request.Type.ToString(), request.ActiveFrom ?? DateOnly.FromDateTime(DateTime.UtcNow));
        await _repository.AddAsync(brand, cancellationToken);
        return brand.Id;
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a brand.", "")]
    public async Task<ActionResult<Guid>> Update([FromBody] UpdateBrandRequest request, [FromQuery] Guid id, CancellationToken cancellationToken)
    {
        var brand = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (brand == null)
            throw new NotFoundException(_t["Brand {0} Not Found.", request.Id]);

        brand.Update(request.Name, request.Description);
        await _repository.UpdateAsync(brand, cancellationToken);
        return Ok(id);
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Brands)]
    [OpenApiOperation("Delete a brand.", "")]
    public async Task<Guid> Delete(Guid id, CancellationToken cancellationToken)
    {
        if (await _productRepo.AnyAsync(new ProductsByBrandSpec(id), cancellationToken))
        {
            throw new ConflictException(_t["Brand cannot be deleted as it's being used."]);
        }

        var brand = await _repository.GetByIdAsync(id, cancellationToken);
        _ = brand ?? throw new NotFoundException(_t["Brand {0} Not Found."]);
        await _repository.DeleteAsync(brand, cancellationToken);
        return id;
    }

    [HttpDelete]
    [MustHavePermission(FSHAction.Delete, FSHResource.Brands)]
    [OpenApiOperation("Delete multiple brands.", "")]
    public Task<Guid[]> DeleteMany(Guid[] ids)
    {
        // ModelBinding Exercise
        return Task.FromResult(ids);
    }

    [HttpPost("generate-random")]
    [MustHavePermission(FSHAction.Generate, FSHResource.Brands)]
    [OpenApiOperation("Generate a number of random brands.", "")]
    public Task<string> GenerateRandomAsync(int seedAmount)
    {
        string jobId = _jobService.Enqueue<IBrandGeneratorJob>(x => x.GenerateAsync(seedAmount, default));
        return Task.FromResult(jobId);
    }
}
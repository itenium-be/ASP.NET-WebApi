using Ardalis.Specification.EntityFrameworkCore;
using Bogus;
using FSH.WebApi.Application.Catalog.Brands;
using System.Reflection;
using FSH.WebApi.Domain.Catalog;
using FSH.WebApi.Infrastructure.Persistence.Context;
using FSH.WebApi.Infrastructure.Persistence.Initialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FSH.WebApi.Infrastructure.Catalog;

public class ProductSeeder : ICustomSeeder
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<ProductSeeder> _logger;

    public ProductSeeder(ILogger<ProductSeeder> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (!_db.Products.Any())
        {
            _logger.LogInformation("Started to Seed Products.");

            // Here you can use your own logic to populate the database.
            var random = new Random();
            var brands = await _db.Brands.ToListAsync();
            var productFaker = new Faker<Product>()
                .CustomInstantiator(f => new Product(f.Random.Word(), f.Lorem.Word(), f.Finance.Amount(), brands[random.Next(brands.Count)].Id, f.Image.PicsumUrl()));

            foreach (int index in Enumerable.Range(1, 100))
            {
                var product = productFaker.Generate();
                await _db.Products.AddAsync(product, cancellationToken);
            }

            await _db.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Seeded Products.");
        }
    }
}
using FSH.WebApi.Application.Common.Persistence;
using FSH.WebApi.Application.Common.Specification;
using FSH.WebApi.Application.Dashboard;
using FSH.WebApi.Application.Identity.Roles;
using FSH.WebApi.Application.Identity.Users;
using FSH.WebApi.Domain.Catalog;
using Microsoft.Extensions.Localization;

namespace FSH.WebApi.Host.Controllers.Dashboard;

public class DashboardController : VersionedApiController
{
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;
    private readonly IReadRepository<Brand> _brandRepo;
    private readonly IReadRepository<Product> _productRepo;
    private readonly IStringLocalizer _t;

    public DashboardController(IUserService userService, IRoleService roleService, IReadRepository<Brand> brandRepo, IReadRepository<Product> productRepo, IStringLocalizer<DashboardController> localizer)
    {
        _userService = userService;
        _roleService = roleService;
        _brandRepo = brandRepo;
        _productRepo = productRepo;
        _t = localizer;
    }

    [HttpGet]
    [MustHavePermission(FSHAction.View, FSHResource.Dashboard)]
    [OpenApiOperation("Get statistics for the dashboard.", "")]
    public async Task<StatsDto> GetAsync(CancellationToken cancellationToken)
    {
        var stats = new StatsDto
        {
            ProductCount = await _productRepo.CountAsync(cancellationToken),
            BrandCount = await _brandRepo.CountAsync(cancellationToken),
            UserCount = await _userService.GetCountAsync(cancellationToken),
            RoleCount = await _roleService.GetCountAsync(cancellationToken)
        };

        int selectedYear = DateTime.UtcNow.Year;
        double[] productsFigure = new double[13];
        double[] brandsFigure = new double[13];
        for (int i = 1; i <= 12; i++)
        {
            int month = i;
            var filterStartDate = new DateTime(selectedYear, month, 01).ToUniversalTime();
            var filterEndDate = new DateTime(selectedYear, month, DateTime.DaysInMonth(selectedYear, month), 23, 59, 59).ToUniversalTime(); // Monthly Based

            var brandSpec = new AuditableEntitiesByCreatedOnBetweenSpec<Brand>(filterStartDate, filterEndDate);
            var productSpec = new AuditableEntitiesByCreatedOnBetweenSpec<Product>(filterStartDate, filterEndDate);

            brandsFigure[i - 1] = await _brandRepo.CountAsync(brandSpec, cancellationToken);
            productsFigure[i - 1] = await _productRepo.CountAsync(productSpec, cancellationToken);
        }

        stats.DataEnterBarChart.Add(new ChartSeries { Name = _t["Products"], Data = productsFigure });
        stats.DataEnterBarChart.Add(new ChartSeries { Name = _t["Brands"], Data = brandsFigure });

        return stats;
    }
}
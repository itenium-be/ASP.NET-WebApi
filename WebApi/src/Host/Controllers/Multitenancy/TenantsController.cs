using FSH.WebApi.Application.Multitenancy;
using System.Threading;

namespace FSH.WebApi.Host.Controllers.Multitenancy;

public class TenantsController : VersionNeutralApiController
{
    private readonly ITenantService _tenantService;

    public TenantsController(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    [HttpGet]
    [MustHavePermission(FSHAction.View, FSHResource.Tenants)]
    [OpenApiOperation("Get a list of all tenants.", "")]
    public Task<List<TenantDto>> GetList()
    {
        return _tenantService.GetAllAsync();
    }

    [HttpGet("{id}")]
    [MustHavePermission(FSHAction.View, FSHResource.Tenants)]
    [OpenApiOperation("Get tenant details.", "")]
    public Task<TenantDto> Get(string id)
    {
        return _tenantService.GetByIdAsync(id);
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Tenants)]
    [OpenApiOperation("Create a new tenant.", "")]
    public Task<string> Create(CreateTenantRequest request, CancellationToken cancellationToken)
    {
        return _tenantService.CreateAsync(request, cancellationToken);
    }

    [HttpPost("{id}/activate")]
    [MustHavePermission(FSHAction.Update, FSHResource.Tenants)]
    [OpenApiOperation("Activate a tenant.", "")]
    [ApiConventionMethod(typeof(FSHApiConventions), nameof(FSHApiConventions.Register))]
    public Task<string> Activate(string id)
    {
        return _tenantService.ActivateAsync(id);
    }

    [HttpPost("{id}/deactivate")]
    [MustHavePermission(FSHAction.Update, FSHResource.Tenants)]
    [OpenApiOperation("Deactivate a tenant.", "")]
    [ApiConventionMethod(typeof(FSHApiConventions), nameof(FSHApiConventions.Register))]
    public Task<string> Deactivate(string id)
    {
        return _tenantService.DeactivateAsync(id);
    }

    [HttpPost("{id}/upgrade")]
    [MustHavePermission(FSHAction.UpgradeSubscription, FSHResource.Tenants)]
    [OpenApiOperation("Upgrade a tenant's subscription.", "")]
    [ApiConventionMethod(typeof(FSHApiConventions), nameof(FSHApiConventions.Register))]
    public async Task<ActionResult<string>> UpgradeSubscription(string id, UpgradeSubscriptionRequest request)
    {
        return id != request.TenantId
            ? BadRequest()
            : Ok(await _tenantService.UpdateSubscription(request.TenantId, request.ExtendedExpiryDate));
    }
}
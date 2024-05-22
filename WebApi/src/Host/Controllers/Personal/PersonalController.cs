using System.Security.Claims;
using FSH.WebApi.Application.Auditing;
using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Application.Identity.Users;
using FSH.WebApi.Application.Identity.Users.Password;
using FSH.WebApi.Infrastructure.Auditing;

namespace FSH.WebApi.Host.Controllers.Identity;

public class PersonalController : VersionNeutralApiController
{
    private readonly IUserService _userService;
    private readonly IAuditService _auditService;

    public PersonalController(IUserService userService, IAuditService auditService)
    {
        _userService = userService;
        _auditService = auditService;
    }

    [HttpGet("profile")]
    [OpenApiOperation("Get profile details of currently logged in user.", "")]
    public async Task<ActionResult<UserDetailsDto>> GetProfile(CancellationToken cancellationToken)
    {
        return User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId)
            ? Unauthorized()
            : Ok(await _userService.GetAsync(userId, cancellationToken));
    }

    [HttpPut("profile")]
    [OpenApiOperation("Update profile details of currently logged in user.", "")]
    public async Task<ActionResult> UpdateProfile(UpdateUserRequest request)
    {
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        await _userService.UpdateAsync(request, userId);
        return Ok();
    }

    [HttpPut("change-password")]
    [OpenApiOperation("Change password of currently logged in user.", "")]
    [ApiConventionMethod(typeof(FSHApiConventions), nameof(FSHApiConventions.Register))]
    public async Task<ActionResult> ChangePassword(ChangePasswordRequest model)
    {
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        await _userService.ChangePasswordAsync(model, userId);
        return Ok();
    }

    [HttpGet("permissions")]
    [OpenApiOperation("Get permissions of currently logged in user.", "")]
    public async Task<ActionResult<List<string>>> GetPermissions(CancellationToken cancellationToken)
    {
        return User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId)
            ? Unauthorized()
            : Ok(await _userService.GetPermissionsAsync(userId, cancellationToken));
    }

    [HttpGet("logs")]
    [OpenApiOperation("Get audit logs of currently logged in user.", "")]
    public Task<List<AuditDto>> GetLogs()
    {
        return _auditService.GetUserTrailsAsync();
    }
}
using FSH.WebApi.Application.Auditing;
using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Infrastructure.Persistence.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace FSH.WebApi.Infrastructure.Auditing;

public class AuditService : IAuditService
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;

    public AuditService(ApplicationDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<List<AuditDto>> GetUserTrailsAsync()
    {
        var trails = await _context.AuditTrails
            .Where(a => a.UserId == _currentUser.GetUserId())
            .OrderByDescending(a => a.DateTime)
            .Take(250)
            .ToListAsync();

        return trails.Adapt<List<AuditDto>>();
    }
}
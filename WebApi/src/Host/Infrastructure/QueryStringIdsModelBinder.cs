using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FSH.WebApi.Host.Infrastructure;

/// <summary>
/// Bind ?ids=1,2,3 to a Guid[]
/// </summary>
public class QueryStringIdsModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        return Task.CompletedTask;
    }
}

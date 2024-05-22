using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FSH.WebApi.Host.Infrastructure;

/// <summary>
/// Parse all Guid[] as QueryString?ids=1,2,3
/// </summary>
public class QueryStringIdsBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        return null;
    }
}
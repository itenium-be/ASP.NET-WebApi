BrandsController :: DeleteMany
==============================

BrandsController:

```c#
public Task<Guid[]> DeleteMany([ModelBinder(BinderType = typeof(QueryStringIdsModelBinder))] Guid[] ids)
```


```c#
/// <summary>
/// Bind ?ids=1,2,3 to a Guid[]
/// </summary>
public class QueryStringIdsModelBinder : IModelBinder
{
  public Task BindModelAsync(ModelBindingContext bindingContext)
  {
    var data = bindingContext.HttpContext.Request.Query;
    bool result = data.TryGetValue("ids", out var ids);

    if (!result)
    {
      bindingContext.Result = ModelBindingResult.Failed();
      return Task.CompletedTask;
    }

    var guids = ids.ToString().Split(',').Select(Guid.Parse).ToArray();
    bindingContext.Result = ModelBindingResult.Success(guids);

    return Task.CompletedTask;
  }
}
```


## Bind all Guid[]

Program.cs:

```c#
builder.Services.AddControllers(options =>
{
  options.ModelBinderProviders.Insert(0, new QueryStringIdsBinderProvider());
});
```

QueryStringIdsBinderProvider:

```c#
public class QueryStringIdsBinderProvider : IModelBinderProvider
{
  public IModelBinder GetBinder(ModelBinderProviderContext context)
  {
    if (context == null)
    {
      throw new ArgumentNullException(nameof(context));
    }

    if (context.Metadata.ModelType == typeof(Guid[]))
    {
      return new BinderTypeModelBinder(typeof(QueryStringIdsModelBinder));
    }

    return null;
  }
}
```

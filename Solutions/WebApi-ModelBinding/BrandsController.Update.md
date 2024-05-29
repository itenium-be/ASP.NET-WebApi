UpdateBrandRequestValidator:

```c#
public class UpdateBrandRequestValidator : CustomValidator<UpdateBrandRequest> {
    public UpdateBrandRequestValidator(IHttpContextAccessor httpContextAccessor) {
      string? queryId = httpContextAccessor.HttpContext?.GetRouteValue("id").ToString();
      Guid.TryParse(queryId, out var queryIdValue);
      RuleFor(p => p.Id)
        .Must(id => id == queryIdValue)
        .WithMessage("Id in de querystring komt niet overeen met de id in de request body.");
    }
  }


```
Register the services and validators
```c#
private static IServiceCollection AddValidations(this IServiceCollection services) {
        return services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
          .AddFluentValidationAutoValidation()
          .AddHttpContextAccessor();
      }
```
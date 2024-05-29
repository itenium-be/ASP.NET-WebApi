using FluentValidation;
using FSH.WebApi.Application.Catalog.Brands;
using FSH.WebApi.Application.Common.Validation;

namespace FSH.WebApi.Host.Infrastructure;

public class UpdateBrandRequestValidator : CustomValidator<UpdateBrandRequest> {
    public UpdateBrandRequestValidator(IHttpContextAccessor httpContextAccessor) {
    }
}
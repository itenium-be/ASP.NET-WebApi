namespace FSH.WebApi.Application.Identity.Users.Password;

public class ForgotPasswordRequest
{
    public string Email { get; set; } = default!;

    public override string ToString() => $"{Email}";
}

public class ForgotPasswordRequestValidator : CustomValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator(IStringLocalizer<ForgotPasswordRequestValidator> T) =>
        RuleFor(p => p.Email).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
                .WithMessage(T["Invalid Email Address."]);
}
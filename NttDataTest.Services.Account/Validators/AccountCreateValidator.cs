using FluentValidation;
using NttDataTest.Services.Account.Commands;

namespace NttDataTest.Services.Account.Validators
{
    public class AccountCreateValidator : AbstractValidator<AccountCreateCommand>
    {
        public AccountCreateValidator()
        {
            RuleFor(c => c.NumeroCuenta).NotEmpty()
                                        .MinimumLength(6)
                                        .MaximumLength(15);

            RuleFor(c => c.TipoCuenta).IsInEnum();

            RuleFor(c => c.SaldoInicial).Custom((x, context) =>
                                        {
                                            if (x.HasValue)
                                                if (x < 0)
                                                    context.AddFailure($"La propiedad {context.PropertyName} no puede ser un valor negativo");
                                        });

            RuleFor(c => c.Estado).NotNull();

            RuleFor(c => c.ClienteGuid).NotEmpty();
        }
    }
}

using FluentValidation;
using NttDataTest.Services.Account.Commands;

namespace NttDataTest.Services.Account.Validators
{
    public class AccountUpdateValidator : AbstractValidator<AccountUpdateCommand>
    {
        public AccountUpdateValidator()
        {
            RuleFor(c => c.CuentaGuid).NotEmpty();

            RuleFor(c => c.TipoCuenta).IsInEnum();

            RuleFor(c => c.Estado).NotNull();
        }
    }
}

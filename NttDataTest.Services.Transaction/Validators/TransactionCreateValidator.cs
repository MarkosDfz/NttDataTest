using FluentValidation;
using NttDataTest.Services.Transaction.Commands;

namespace NttDataTest.Services.Transaction.Validators
{
    public class TransactionCreateValidator : AbstractValidator<TransactionCreateCommand>
    {
        public TransactionCreateValidator()
        {
            RuleFor(t => t.CuentaGuid).NotEmpty();

            RuleFor(t => t.TipoMovimiento).IsInEnum();

            RuleFor(t => t.ValorMovimiento).Custom((x, context) =>
                                           {
                                                   if (x == 0)
                                                       context.AddFailure($"La propiedad {context.PropertyName} debe ser diferente de 0");
                                           });
        }
    }
}

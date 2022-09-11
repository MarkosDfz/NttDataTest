using FluentValidation;
using NttDataTest.Services.Client.Commands;

namespace NttDataTest.Services.Client.Validators
{
    public class ClientUpdateValidator : AbstractValidator<ClientUpdateCommand>
    {
        public ClientUpdateValidator()
        {
            RuleFor(c => c.Nombre).NotEmpty()
                                  .MinimumLength(6)
                                  .MaximumLength(50);

            RuleFor(c => c.Genero).IsInEnum();

            RuleFor(c => c.Edad).NotEmpty();

            RuleFor(c => c.Identificacion).NotEmpty()
                                          .MinimumLength(10)
                                          .MaximumLength(15);

            RuleFor(c => c.Direccion).NotEmpty()
                                     .MinimumLength(4)
                                     .MaximumLength(250);

            RuleFor(c => c.Telefono).NotEmpty()
                                          .MinimumLength(10)
                                          .MaximumLength(15);

            RuleFor(c => c.Estado).NotNull();
        }
    }
}

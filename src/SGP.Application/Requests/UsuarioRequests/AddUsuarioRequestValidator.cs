using FluentValidation;
using SGP.Shared.Extensions;

namespace SGP.Application.Requests.UsuarioRequests
{
    public class AddUsuarioRequestValidator : AbstractValidator<AddUsuarioRequest>
    {
        public AddUsuarioRequestValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(30);

            RuleFor(x => x.Email)
                .NotEmpty()
                .IsValidEmailAddress()
                .MaximumLength(100);

            RuleFor(x => x.Senha)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(16);
        }
    }
}

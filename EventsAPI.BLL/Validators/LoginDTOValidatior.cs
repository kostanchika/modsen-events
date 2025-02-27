using EventsAPI.BLL.DTO;
using FluentValidation;

namespace EventsAPI.BLL.Validators
{
    public class LoginDTOValidatior : AbstractValidator<LoginDTO>
    {
        public LoginDTOValidatior()
        {
            RuleFor(x => x.Login)
                .MinimumLength(3)
                .WithMessage("Длина логина должна составлять от 3 до 20 символов")
                .MaximumLength(20)
                .WithMessage("Длина логина должна составлять от 3 до 20 символов");

            RuleFor(x => x.Password)
                .MinimumLength(8)
                .WithMessage("Длина пароля должна составлять от 8 до 20 символов")
                .MaximumLength(20)
                .WithMessage("Длина пароля должна составлять от 8 до 20 символов")
                .Matches(@"^[A-Za-z\d]*$")
                .WithMessage("Пароль может состоять только из латинских букв и цифр");
        }
    }
}

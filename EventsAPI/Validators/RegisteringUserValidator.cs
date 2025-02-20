using EventsAPI.Models;
using FluentValidation;

namespace EventsAPI.Validators
{
    public class RegisteringUserValidator : AbstractValidator<RegisterModel>
    {
        public RegisteringUserValidator()
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
                .Matches(@"^[A-Za-z\d]+$")
                .WithMessage("Пароль может состоять только из латинских букв и цифр");

            RuleFor(x => x.Name)
                .MinimumLength(2)
                .WithMessage("Длина имени должна составлять от 2 до 30 символов")
                .MaximumLength(30)
                .WithMessage("Длина имени должна составлять от 2 до 30 символов")
                .Matches("^[a-zA-Zа-яА-ЯёЁ]+$")
                .WithMessage("Имя должно содержать только буквы");

            RuleFor(x => x.LastName)
                .MinimumLength(2)
                .WithMessage("Длина фамилии должна составлять от 2 до 30 символов")
                .MaximumLength(30)
                .WithMessage("Длина фамилии должна составлять от 2 до 30 символов")
                .Matches("^[a-zA-Zа-яА-ЯёЁ]+$")
                .WithMessage("Фамилия должно содержать только буквы");

            RuleFor(x => x.BirthDateTime)
                .GreaterThanOrEqualTo(new DateTime(1900, 1, 1))
                .WithMessage("Дата рождения должна быть больше чем 01.01.1900")
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("Дата рождения не может быть позже текущей");

            RuleFor(x => x.Email)
                .Matches("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")
                .WithMessage("Неверный email");
        }
    }
}

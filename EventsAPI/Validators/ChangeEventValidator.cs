using EventsAPI.Models;
using FluentValidation;

namespace EventsAPI.Validators
{
    public class ChangeEventValidator : AbstractValidator<ChangeEventModel>
    {
        public ChangeEventValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Имя события не может быть пустым")
                .MaximumLength(500)
                .WithMessage("Максимальная длина описания события - 500 символов");

            RuleFor(x => x.EventDateTime)
                .GreaterThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Дата проведения события не может быть меньше текущей");

            RuleFor(x => x.Location)
                .NotEmpty()
                .WithMessage("Место проведения не может быть пустым")
                .MaximumLength(100)
                .WithMessage("Максимальная длина места проведения - 100 символов");

            RuleFor(x => x.Category)
                .NotEmpty()
                .WithMessage("Категория события не может быть пустой");

            RuleFor(x => x.MaximumParticipants)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Максимальное количество участников события должно быть больше 0");
        }
    }
}

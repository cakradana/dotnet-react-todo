using FluentValidation;
using TodoApp.Application.Contracts.Todos;

namespace TodoApp.Application.Contracts.Todos.Validators;

public class CreateTodoRequestValidator : AbstractValidator<CreateTodoRequest>
{
    public CreateTodoRequestValidator()
    {
        RuleFor(x => x.Task)
            .NotEmpty().WithMessage("Task cannot be empty.")
            .MaximumLength(500).WithMessage("Task cannot exceed 500 characters.");

        RuleFor(x => x.Priority)
            .NotEmpty()
            .Must(p => p == "Low" || p == "Medium" || p == "High")
            .WithMessage("Priority must be Low, Medium, or High.");

        RuleFor(x => x.Category).NotNull();
        When(x => x.Category != null, () =>
        {
            RuleFor(x => x.Category.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Category.Color).NotEmpty().Matches("^#[0-9A-Fa-f]{6}$").WithMessage("Color must be a valid hex code.");
        });
    }
}

public class UpdateTodoRequestValidator : AbstractValidator<UpdateTodoRequest>
{
    public UpdateTodoRequestValidator()
    {
        RuleFor(x => x.Task)
            .NotEmpty().WithMessage("Task cannot be empty.")
            .MaximumLength(500).WithMessage("Task cannot exceed 500 characters.");

        RuleFor(x => x.Priority)
            .NotEmpty()
            .Must(p => p == "Low" || p == "Medium" || p == "High")
            .WithMessage("Priority must be Low, Medium, or High.");

        RuleFor(x => x.Category).NotNull();
        When(x => x.Category != null, () =>
        {
            RuleFor(x => x.Category.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Category.Color).NotEmpty().Matches("^#[0-9A-Fa-f]{6}$").WithMessage("Color must be a valid hex code.");
        });
    }
}

using BlazorHero.CleanArchitecture.Application.Features.Statements.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Application.Validators.Features.Statements.Commands.AddEdit
{
    public class AddEditStatementCommandValidator : AbstractValidator<AddEditStatementCommand>
    {
        public AddEditStatementCommandValidator(IStringLocalizer<AddEditStatementCommandValidator> localizer)
        {
            RuleFor(request => request.Body)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Body is required!"]);
            RuleFor(request => request.TopicId)
                .GreaterThan(0).WithMessage(x => localizer["Topic is required!"]);
        }
    }
}
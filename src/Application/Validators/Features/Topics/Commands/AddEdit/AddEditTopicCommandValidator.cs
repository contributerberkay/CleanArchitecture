using BlazorHero.CleanArchitecture.Application.Features.Topics.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Application.Validators.Features.Topics.Commands.AddEdit
{
    public class AddEditTopicCommandValidator : AbstractValidator<AddEditTopicCommand>
    {
        public AddEditTopicCommandValidator(IStringLocalizer<AddEditTopicCommandValidator> localizer)
        {
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
        }
    }
}
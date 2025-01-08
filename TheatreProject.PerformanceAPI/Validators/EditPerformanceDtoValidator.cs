using FluentValidation;
using TheatreProject.PerformanceAPI.Models.DTOs;

namespace TheatreProject.PerformanceAPI.Validators;

public class EditPerformanceDtoValidator : AbstractValidator<EditPerformanceDto>
{
    public EditPerformanceDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.TheatreName).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
        RuleFor(x => x.Address).NotEmpty();
        RuleFor(x => x.BasePrice).GreaterThan(0);
        RuleFor(x => x.ShowDateTime).NotEmpty();
        RuleFor(x => x.Duration).GreaterThan(TimeSpan.Zero);
        RuleFor(x => x.Capacity).GreaterThan(0);
        RuleFor(x => x.AvailableSeats).GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(x => x.Capacity);
        RuleFor(x => x.Status).IsInEnum();
        RuleFor(x => x.Category).IsInEnum();
    }
}
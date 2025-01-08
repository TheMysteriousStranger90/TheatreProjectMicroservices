using FluentValidation;
using TheatreProject.PerformanceAPI.Models.Dto;

namespace TheatreProject.PerformanceAPI.Validators;

public class PerformanceDtoValidator : AbstractValidator<CreatePerformanceDto>
{
    public PerformanceDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.BasePrice).GreaterThan(0);
        RuleFor(x => x.ShowDateTime).GreaterThan(DateTime.Now);
        RuleFor(x => x.Duration).GreaterThan(TimeSpan.Zero);
        RuleFor(x => x.Capacity).GreaterThan(0);
    }
}
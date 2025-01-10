using FluentValidation;
using TheatreProject.ShoppingCartAPI.Models.DTOs;

namespace TheatreProject.ShoppingCartAPI.Validators;

public class CartDetailsValidator : AbstractValidator<CartDetailsDto>
{
    public CartDetailsValidator()
    {
        RuleFor(x => x.PerformanceId).NotEmpty();
        RuleFor(x => x.SeatNumbers).NotEmpty()
            .Matches(@"^[A-Z][0-9]+(,[A-Z][0-9]+)*$")
            .WithMessage("Seat numbers must be in format: A1,B2,C3");
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.PricePerTicket).GreaterThan(0);
        RuleFor(x => x.SubTotal).GreaterThan(0);
    }
}
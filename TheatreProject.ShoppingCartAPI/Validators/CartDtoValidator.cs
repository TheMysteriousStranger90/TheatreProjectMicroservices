using FluentValidation;
using TheatreProject.ShoppingCartAPI.Models.DTOs;

namespace TheatreProject.ShoppingCartAPI.Validators;

public class CartDtoValidator : AbstractValidator<CartDto>
{
    public CartDtoValidator()
    {
        RuleFor(x => x.CartHeader).NotNull();
        RuleFor(x => x.CartDetails).NotNull().NotEmpty();
        RuleForEach(x => x.CartDetails).SetValidator(new CartDetailsValidator());
    }
}
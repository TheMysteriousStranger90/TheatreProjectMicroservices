using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TheatreProject.ShoppingCartAPI.Models.DTOs;

public class CheckoutHeaderDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string? CouponCode { get; set; }
    public decimal DiscountTotal { get; set; }
    public decimal GrandTotal { get; set; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    
    [ValidateNever]
    public string CardNumber { get; set; }
    [ValidateNever]
    public string CVV { get; set; }
    [ValidateNever]
    public string ExpiryMonthYear { get; set; }
    
    public int TotalItems { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public string? OrderStatus { get; set; }
    
    [ValidateNever]
    public IEnumerable<CartDetailsDto>? CartDetails { get; set; }
}
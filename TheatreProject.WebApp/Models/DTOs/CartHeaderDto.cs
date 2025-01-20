namespace TheatreProject.WebApp.Models.DTOs;

public class CartHeaderDto
{
    public Guid Id { get; set; }
    public string? UserId { get; set; }
    public string? CouponCode { get; set; }
    public decimal DiscountTotal { get; set; }
    public decimal GrandTotal { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public string? CardNumber { get; set; }
    public string? CVV { get; set; }
    public string? ExpiryMonthYear { get; set; }

    public string Name => string.Format("{0} {1}",
        FirstName ?? string.Empty,
        LastName ?? string.Empty).Trim();
}
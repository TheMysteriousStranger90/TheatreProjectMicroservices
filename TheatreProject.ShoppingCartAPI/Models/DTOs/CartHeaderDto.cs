namespace TheatreProject.ShoppingCartAPI.Models.DTOs;

public class CartHeaderDto
{
    public Guid Id { get; set; }
    public string? UserId { get; set; }
    public string? CouponCode { get; set; }
    public decimal DiscountTotal { get; set; }
    public decimal GrandTotal { get; set; }
    public string? Email { get; set; }
}
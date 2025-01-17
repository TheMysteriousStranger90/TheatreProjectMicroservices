namespace TheatreProject.OrderAPI.Models.DTOs;

public class CartHeaderDto
{
    public Guid Id { get; set; }
    public string? UserId { get; set; }
    public string? CouponCode { get; set; }
    public double DiscountTotal { get; set; }
    public double GrandTotal { get; set; }

    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
}
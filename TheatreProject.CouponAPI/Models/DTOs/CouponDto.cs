namespace TheatreProject.CouponAPI.Models.DTOs;

public class CouponDto
{
    public Guid Id { get; set; }
    public string CouponCode { get; set; }
    public decimal DiscountAmount { get; set; }
}
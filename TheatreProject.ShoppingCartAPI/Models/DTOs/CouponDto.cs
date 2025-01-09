using TheatreProject.ShoppingCartAPI.Models.Enums;

namespace TheatreProject.ShoppingCartAPI.Models.DTOs;

public class CouponDto
{
    public Guid Id { get; set; }
    public string CouponCode { get; set; }
    public decimal DiscountAmount { get; set; }
    public DiscountType DiscountType { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public int MinimumTickets { get; set; }
    public decimal MinimumOrderAmount { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace TheatreProject.CouponAPI.Models;

public class Coupon
{
    [Key] public Guid Id { get; set; }
    [Required] public string CouponCode { get; set; }
    [Required] public decimal DiscountAmount { get; set; }
}
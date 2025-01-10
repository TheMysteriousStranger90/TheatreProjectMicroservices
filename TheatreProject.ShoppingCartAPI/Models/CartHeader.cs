using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheatreProject.ShoppingCartAPI.Models.Enums;

namespace TheatreProject.ShoppingCartAPI.Models;

public class CartHeader
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string? UserId { get; set; }
    public string? CouponCode { get; set; }
    
    [NotMapped]
    public decimal DiscountTotal { get; set; }
    
    [NotMapped]
    public decimal GrandTotal { get; set; }
    
    public PaymentStatus PaymentStatus { get; set; }
    public string? PaymentIntentId { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}
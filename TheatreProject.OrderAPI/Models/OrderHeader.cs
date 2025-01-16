using System.ComponentModel.DataAnnotations;

namespace TheatreProject.OrderAPI.Models;

public class OrderHeader
{
    [Key]
    public Guid Id { get; set; }
    public string? UserId { get; set; }
    public string? CouponCode { get; set; }
    public double GrandTotal { get; set; }
    public double DiscountTotal { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime OrderTime { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? CardNumber { get; set; }
    public string? CVV { get; set; }
    public string? ExpiryMonthYear { get; set; }
    public int CartTotalPerformances { get; set; }
    public IEnumerable<OrderDetails> OrderDetails { get; set; }
    public bool PaymentStatus { get; set; }
    
    public string? PaymentIntentId { get; set; }
    public string? StripeSessionId { get; set; }
}
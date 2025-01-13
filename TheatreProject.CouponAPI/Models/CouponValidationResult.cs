namespace TheatreProject.CouponAPI.Models;

public class CouponValidationResult
{
    public bool IsValid { get; set; }
    public string Message { get; set; }
    public List<string> ValidationErrors { get; set; } = new();
    public decimal DiscountAmount { get; set; }
}
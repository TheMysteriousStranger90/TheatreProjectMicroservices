namespace TheatreProject.ShoppingCartAPI.Models;

public class CouponValidationResult
{
    public bool IsValid { get; set; }
    public string Message { get; set; }
    public decimal DiscountAmount { get; set; }
    public List<string> ValidationErrors { get; set; } = new();
    public DateTime? ExpirationDate { get; set; }
    public bool RequiresMinimumPurchase { get; set; }
    public decimal MinimumPurchaseAmount { get; set; }
}
namespace TheatreProject.WebApp.Models.DTOs;

public class StripeRequestDto
{
    public string? StripeSessionUrl { get; set; }
    public string? StripeSessionId { get; set; }
    public string ApprovedUrl { get; set; }
    public string CancelUrl { get; set; }
    public OrderHeaderDto OrderHeader { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerName { get; set; }
    public string CustomerPhone { get; set; }
    public PaymentMethodDataDto PaymentMethodData { get; set; }
}
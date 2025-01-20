namespace TheatreProject.WebApp.Models.DTOs;

public class OrderConfirmationDto
{

    public Guid OrderId { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerName { get; set; }
    public List<OrderDetailsDto> OrderDetails { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
}
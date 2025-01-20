using TheatreProject.OrderAPI.Models.Enums;

namespace TheatreProject.OrderAPI.Models;

public class OrderDetails
{
    public Guid Id { get; set; }

    public Guid OrderHeaderId { get; set; }
    public OrderHeader? OrderHeader { get; set; }

    public Guid PerformanceId { get; set; }
    public string? PerformanceName { get; set; }

    public string SeatNumbers { get; set; }
    public TicketType TicketType { get; set; }
    public int Quantity { get; set; }
    public decimal PricePerTicket { get; set; }
    public decimal SubTotal { get; set; }
}
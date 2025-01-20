using TheatreProject.OrderAPI.Models.Enums;

namespace TheatreProject.OrderAPI.Models.DTOs;

public class CartDetailsDto
{
    public Guid Id { get; set; }
    public Guid CartHeaderId { get; set; }
    public CartHeaderDto? CartHeader { get; set; }
    public Guid PerformanceId { get; set; }
    public PerformanceDto? Performance { get; set; }
    public string SeatNumbers { get; set; }
    public TicketType TicketType { get; set; }
    public int Quantity { get; set; }
    public decimal PricePerTicket { get; set; }
    public decimal SubTotal { get; set; }
}
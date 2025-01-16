﻿using TheatreProject.OrderAPI.Models.Enums;

namespace TheatreProject.OrderAPI.Models.DTOs;

public class OrderDetailsDto
{
    public Guid Id { get; set; }

    public Guid OrderHeaderId { get; set; }
    public OrderHeader? OrderHeader { get; set; }

    public Guid PerformanceId { get; set; }
    public string? PerformanceName { get; set; }

    public string SeatNumbers { get; set; }
    public TicketType TicketType { get; set; }
    public int Quantity { get; set; }
    public double PricePerTicket { get; set; }
    public double SubTotal { get; set; }
}
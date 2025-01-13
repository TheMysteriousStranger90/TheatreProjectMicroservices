﻿namespace TheatreProject.WebApp.Models.DTOs;

public class CartHeaderDto
{
    public Guid Id { get; set; }
    public string? UserId { get; set; }
    public string? CouponCode { get; set; }
    public double DiscountTotal { get; set; }
    public double GrandTotal { get; set; }
    public string? Email { get; set; }
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime PickupDateTime { get; set; }
    public string? Phone { get; set; }
    public string? CardNumber { get; set; }
    public string? CVV { get; set; }
    public string? ExpiryMonthYear { get; set; }
}
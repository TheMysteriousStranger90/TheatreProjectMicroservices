using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheatreProject.ShoppingCartAPI.Models.Enums;

namespace TheatreProject.ShoppingCartAPI.Models;

public class CartDetails
{
    [Key]
    public Guid Id { get; set; }
    public Guid CartHeaderId { get; set; }
    public virtual CartHeader? CartHeader { get; set; }
    
    public Guid PerformanceId { get; set; }
    public virtual Performance? Performance { get; set; }
    
    [Required]
    [RegularExpression(@"^[A-Z0-9]+(,[A-Z0-9]+)*$")]
    public string SeatNumbers { get; set; }
    
    [Required]
    public TicketType TicketType { get; set; }
    
    [Required]
    [Range(1, 10)]
    public int Quantity { get; set; }
    
    [Required]
    [Range(0.01, 100000)]
    public decimal PricePerTicket { get; set; }
    
    [NotMapped]
    public decimal SubTotal => Quantity * PricePerTicket;
    
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}
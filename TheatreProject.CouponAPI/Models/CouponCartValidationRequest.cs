using TheatreProject.CouponAPI.Models.DTOs;

namespace TheatreProject.CouponAPI.Models;

public class CouponCartValidationRequest
{
    public string CouponCode { get; set; }
    public CartDto Cart { get; set; }
}
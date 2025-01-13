using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheatreProject.CouponAPI.Data;
using TheatreProject.CouponAPI.Models;
using TheatreProject.CouponAPI.Models.DTOs;
using TheatreProject.CouponAPI.Repositories.Interfaces;

namespace TheatreProject.CouponAPI.Repositories;

public class CouponRepository : ICouponRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    protected IMapper _mapper;

    public CouponRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<CouponDto> GetCouponByCode(string couponCode)
    {
        var coupon = await _applicationDbContext.Coupons
            .FirstOrDefaultAsync(c => c.CouponCode.ToLower() == couponCode.ToLower());
        return _mapper.Map<CouponDto>(coupon);
    }

    public async Task<bool> DoesCouponExist(string couponCode)
    {
        return await _applicationDbContext.Coupons
            .AnyAsync(c => c.CouponCode.ToLower() == couponCode.ToLower());
    }

    public async Task<IEnumerable<CouponDto>> GetAllCoupons()
    {
        var coupons = await _applicationDbContext.Coupons.ToListAsync();
        return _mapper.Map<IEnumerable<CouponDto>>(coupons);
    }

    public async Task<CouponDto> GetCouponById(Guid id)
    {
        var coupon = await _applicationDbContext.Coupons
            .FirstOrDefaultAsync(c => c.Id == id);
        return _mapper.Map<CouponDto>(coupon);
    }

    public async Task<ResponseDto> CreateCoupon(CouponDto couponDto)
    {
        try
        {
            var coupon = _mapper.Map<Coupon>(couponDto);
            _applicationDbContext.Coupons.Add(coupon);
            await _applicationDbContext.SaveChangesAsync();

            return new ResponseDto
            {
                IsSuccess = true,
                Result = _mapper.Map<CouponDto>(coupon),
                DisplayMessage = "Coupon created successfully"
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                ErrorMessages = new List<string> { ex.Message },
                DisplayMessage = "Error creating coupon"
            };
        }
    }

    public async Task<ResponseDto> UpdateCoupon(CouponDto couponDto)
    {
        try
        {
            var existingCoupon = await _applicationDbContext.Coupons
                .FirstOrDefaultAsync(c => c.Id == couponDto.Id);

            if (existingCoupon == null)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    DisplayMessage = "Coupon not found",
                    ErrorMessages = new List<string> { "Coupon with given ID does not exist" }
                };
            }

            existingCoupon.CouponCode = couponDto.CouponCode;
            existingCoupon.DiscountAmount = couponDto.DiscountAmount;

            _applicationDbContext.Coupons.Update(existingCoupon);
            await _applicationDbContext.SaveChangesAsync();

            return new ResponseDto
            {
                IsSuccess = true,
                Result = _mapper.Map<CouponDto>(existingCoupon),
                DisplayMessage = "Coupon updated successfully"
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                ErrorMessages = new List<string> { ex.Message },
                DisplayMessage = "Error updating coupon"
            };
        }
    }

    public async Task<ResponseDto> DeleteCoupon(Guid id)
    {
        try
        {
            var coupon = await _applicationDbContext.Coupons
                .FirstOrDefaultAsync(c => c.Id == id);

            if (coupon == null)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    DisplayMessage = "Coupon not found",
                    ErrorMessages = new List<string> { "Coupon with given ID does not exist" }
                };
            }

            _applicationDbContext.Coupons.Remove(coupon);
            await _applicationDbContext.SaveChangesAsync();

            return new ResponseDto
            {
                IsSuccess = true,
                DisplayMessage = "Coupon deleted successfully"
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                ErrorMessages = new List<string> { ex.Message },
                DisplayMessage = "Error deleting coupon"
            };
        }
    }

    public async Task<bool> ValidateCoupon(string couponCode, decimal orderAmount, int ticketCount)
    {
        var coupon = await _applicationDbContext.Coupons
            .FirstOrDefaultAsync(c => c.CouponCode.ToLower() == couponCode.ToLower());

        if (coupon == null) return false;

        if (orderAmount < 50) return false;

        if (ticketCount > 10) return false;

        var discountAmount = orderAmount * ((decimal)coupon.DiscountAmount / 100);
        if (discountAmount > (orderAmount * 0.5M)) return false;

        switch (coupon.CouponCode.ToUpper())
        {
            case "NEW10":
                return true;

            case "EARLY20":
                return true;

            case "GROUP15":
                return ticketCount >= 5;

            case "VIP25":
                return true;

            default:
                return true;
        }
    }

    public async Task<decimal> CalculateDiscount(string couponCode, decimal amount)
    {
        var coupon = await _applicationDbContext.Coupons
            .FirstOrDefaultAsync(c => c.CouponCode.ToLower() == couponCode.ToLower());

        if (coupon == null) return 0;

        return amount * ((decimal)coupon.DiscountAmount / 100);
    }

    public async Task<CouponValidationResult> ValidateCouponForCart(string couponCode, CartDto cart)
    {
        var result = new CouponValidationResult();
        var coupon = await _applicationDbContext.Coupons
            .FirstOrDefaultAsync(c => c.CouponCode.ToLower() == couponCode.ToLower());

        if (coupon == null)
        {
            result.ValidationErrors.Add("Invalid coupon code");
            return result;
        }

        var totalAmount = cart.CartDetails?.Sum(d => d.SubTotal) ?? 0;
        var totalTickets = cart.CartDetails?.Sum(d => d.Quantity) ?? 0;

        result.IsValid = true;
        result.DiscountAmount = totalAmount * ((decimal)coupon.DiscountAmount / 100);
        result.Message = "Coupon applied successfully";

        return result;
    }
}
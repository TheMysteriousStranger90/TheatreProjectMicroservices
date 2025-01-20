using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheatreProject.CouponAPI.Models;
using TheatreProject.CouponAPI.Models.DTOs;
using TheatreProject.CouponAPI.Repositories.Interfaces;

namespace TheatreProject.CouponAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CouponController : ControllerBase
{
    private readonly ICouponRepository _couponRepository;
    protected ResponseDto _response;

    public CouponController(ICouponRepository couponRepository)
    {
        _couponRepository = couponRepository;
        _response = new ResponseDto();
    }

    [HttpGet]
    public async Task<ResponseDto> GetAll()
    {
        try
        {
            _response.Result = await _couponRepository.GetAllCoupons();
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
        }

        return _response;
    }

    [HttpGet("{id}")]
    public async Task<ResponseDto> GetById(Guid id)
    {
        try
        {
            _response.Result = await _couponRepository.GetCouponById(id);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
        }

        return _response;
    }

    [HttpGet("GetByCode/{code}")]
    public async Task<ResponseDto> GetByCode(string code)
    {
        try
        {
            _response.Result = await _couponRepository.GetCouponByCode(code);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
        }

        return _response;
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<ResponseDto> Create([FromBody] CouponDto couponDto)
    {
        if (!ModelState.IsValid)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return _response;
        }

        return await _couponRepository.CreateCoupon(couponDto);
    }

    [HttpPut]
    [Authorize(Roles = "Administrator")]
    public async Task<ResponseDto> Update([FromBody] CouponDto couponDto)
    {
        if (!ModelState.IsValid)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return _response;
        }

        return await _couponRepository.UpdateCoupon(couponDto);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<ResponseDto> Delete(Guid id)
    {
        return await _couponRepository.DeleteCoupon(id);
    }

    [HttpGet("exists/{code}")]
    public async Task<ResponseDto> DoesCouponExist(string code)
    {
        try
        {
            _response.Result = await _couponRepository.DoesCouponExist(code);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
        }

        return _response;
    }

    [HttpGet("validate")]
    public async Task<ResponseDto> ValidateCoupon([FromQuery] string code,
        [FromQuery] decimal amount, [FromQuery] int tickets)
    {
        try
        {
            _response.Result = await _couponRepository.ValidateCoupon(code, amount, tickets);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
        }

        return _response;
    }

    [HttpGet("calculate")]
    public async Task<ResponseDto> CalculateDiscount([FromQuery] string code, [FromQuery] decimal amount)
    {
        try
        {
            _response.Result = await _couponRepository.CalculateDiscount(code, amount);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
        }

        return _response;
    }

    [HttpPost("validate-cart")]
    public async Task<ResponseDto> ValidateCartCoupon([FromBody] CouponCartValidationRequest request)
    {
        try
        {
            _response.Result = await _couponRepository.ValidateCouponForCart(request.CouponCode, request.Cart);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
        }

        return _response;
    }
}
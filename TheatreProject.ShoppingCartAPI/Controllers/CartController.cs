using Microsoft.AspNetCore.Mvc;
using TheatreProject.ShoppingCartAPI.Models.DTOs;
using TheatreProject.ShoppingCartAPI.Repositories.Interfaces;
using TheatreProject.ShoppingCartAPI.Services.Interfaces;
using TheatreProject.ShoppingCartAPI.Validators;

namespace TheatreProject.ShoppingCartAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(ValidationFilter))]
public class CartController : Controller
{
    private readonly ICartRepository _cartRepository;

    //private readonly ICouponRepository _couponRepository;
    //private readonly IMessageBus _messageBus;
    private readonly ICouponService _couponService;
    protected ResponseDto _response;

    public CartController(ICartRepository cartRepository,
        ICouponService couponService)
    {
        _cartRepository = cartRepository;
        _couponService = couponService;
        //_couponRepository = couponRepository;
        //_messageBus = messageBus;
        _response = new ResponseDto();
    }

    [HttpGet("GetCart/{userId}")]
    public async Task<ResponseDto> GetCart(string userId)
    {
        try
        {
            CartDto cartDto = await _cartRepository.GetCartByUserId(userId);

            _response.Result = cartDto;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return _response;
    }

    [HttpPost("AddCart")]
    public async Task<ResponseDto> AddCart([FromBody] CartDto cartDto)
    {
        try
        {
            if (string.IsNullOrEmpty(cartDto.CartHeader?.UserId))
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { "User ID is required" };
                return _response;
            }

            CartDto cartDt = await _cartRepository.CreateUpdateCart(cartDto);
            _response.Result = cartDt;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return _response;
    }

    [HttpPut("UpdateCart")]
    public async Task<ResponseDto> UpdateCart(CartDto cartDto)
    {
        try
        {
            CartDto cartDt = await _cartRepository.CreateUpdateCart(cartDto);

            _response.Result = cartDt;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return _response;
    }

    [HttpDelete("RemoveCart")]
    public async Task<ResponseDto> RemoveCart([FromBody] Guid cartId)
    {
        try
        {
            bool isSuccess = await _cartRepository.RemoveFromCart(cartId);

            _response.Result = isSuccess;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return _response;
    }

    [HttpDelete("ClearCart/{userId}")]
    public async Task<ResponseDto> ClearCart(string userId)
    {
        try
        {
            bool isSuccess = await _cartRepository.ClearCart(userId);

            _response.Result = isSuccess;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return _response;
    }

    [HttpPost("ApplyCoupon")]
    public async Task<ResponseDto> ApplyCoupon([FromBody] CartDto cartDto)
    {
        try
        {
            var couponCode = cartDto.CartHeader.CouponCode;
            bool doesExist = await _couponService.DoesCouponExist(couponCode);

            if (doesExist)
            {
                bool isSuccess = await _cartRepository.ApplyCoupon(cartDto.CartHeader.UserId, couponCode);
                _response.Result = isSuccess;
            }
            else
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { "Coupon does not exist" };
                _response.DisplayMessage = "Coupon does not exist";
            }
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
        }

        return _response;
    }

    [HttpPost("RemoveCoupon")]
    public async Task<ResponseDto> RemoveCoupon([FromBody] string userId)
    {
        try
        {
            bool isSuccess = await _cartRepository.RemoveCoupon(userId);
            _response.Result = isSuccess;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return _response;
    }
}
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
            if (string.IsNullOrEmpty(cartDto?.CartHeader?.CouponCode))
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { "Coupon code is required" };
                return _response;
            }

            var doesExist = await _couponService.DoesCouponExist(cartDto.CartHeader.CouponCode);
            if (!doesExist)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { "Invalid coupon code" };
                return _response;
            }

            var isSuccess = await _cartRepository.ApplyCoupon(cartDto.CartHeader.UserId, cartDto.CartHeader.CouponCode);
            _response.Result = isSuccess;
            return _response;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
            return _response;
        }
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
    
    
    
    
    [HttpGet("ValidateCart")]
    public async Task<ResponseDto> ValidateCart([FromQuery] string userId)
    {
        try
        {
            var cart = await _cartRepository.GetCartByUserId(userId);
            _response.Result = await _cartRepository.ValidateCart(cart);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
        }
        return _response;
    }

    [HttpPut("UpdateQuantity/{cartDetailId}")]
    public async Task<ResponseDto> UpdateQuantity(Guid cartDetailId, [FromBody] int quantity)
    {
        try
        {
            _response.Result = await _cartRepository.UpdateQuantity(cartDetailId, quantity);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
        }
        return _response;
    }
    
    [HttpGet("ValidateSeats")]
    public async Task<ResponseDto> ValidateSeats([FromQuery] Guid performanceId, [FromQuery] string seats)
    {
        try
        {
            _response.Result = await _cartRepository.ValidateSeats(performanceId, seats);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
        }
        return _response;
    }

    [HttpGet("CalculateTotal/{userId}")]
    public async Task<ResponseDto> CalculateTotal(string userId)
    {
        try
        {
            _response.Result = await _cartRepository.CalculateTotal(userId);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
        }
        return _response;
    }
    
    [HttpGet("Status/{userId}")]
    public async Task<ResponseDto> GetCartStatus(string userId)
    {
        try
        {
            _response.Result = await _cartRepository.GetCartStatus(userId);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
        }
        return _response;
    }

    [HttpPost("SaveForLater/{userId}")]
    public async Task<ResponseDto> SaveCartForLater(string userId)
    {
        try
        {
            _response.Result = await _cartRepository.SaveCartForLater(userId);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
        }
        return _response;
    }
    
    [HttpGet("Detail/{cartDetailId}")]
    public async Task<ResponseDto> GetCartDetail(Guid cartDetailId)
    {
        try
        {
            _response.Result = await _cartRepository.GetCartDetail(cartDetailId);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
        }
        return _response;
    }
    
    [HttpPost("ValidateCouponForCart")]
    public async Task<ResponseDto> ValidateCouponForCart([FromBody] CartDto cartDto)
    {
        try
        {
            var result = await _couponService.ValidateCouponForCart(
                cartDto.CartHeader.CouponCode, 
                cartDto);
            _response.Result = result;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
        }
        return _response;
    }
}
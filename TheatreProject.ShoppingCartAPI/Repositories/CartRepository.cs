using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheatreProject.ShoppingCartAPI.Data;
using TheatreProject.ShoppingCartAPI.Models;
using TheatreProject.ShoppingCartAPI.Models.DTOs;
using TheatreProject.ShoppingCartAPI.Models.Enums;
using TheatreProject.ShoppingCartAPI.Repositories.Interfaces;

namespace TheatreProject.ShoppingCartAPI.Repositories;

public class CartRepository : ICartRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private IMapper _mapper;

    public CartRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<CartDto> GetCartByUserId(string userId)
    {
        try
        {
            Cart cart = new()
            {
                CartHeader = await _applicationDbContext.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId),
            };

            cart.CartDetails = _applicationDbContext.CartDetails
                .Where(u => u.CartHeaderId == cart.CartHeader.Id);

            return _mapper.Map<CartDto>(cart);
        }
        catch (Exception)
        {
            return new CartDto();
        }
    }

    public async Task<CartDto> CreateUpdateCart(CartDto cartDto)
    {
        try
        {
            var cart = _mapper.Map<Cart>(cartDto);
            var performanceInDb = await _applicationDbContext.Performances
                .FirstOrDefaultAsync(p => p.Id == cartDto.CartDetails.First().PerformanceId);

            if (performanceInDb == null)
            {
                _applicationDbContext.Performances.Add(cart.CartDetails.First().Performance);
                await _applicationDbContext.SaveChangesAsync();
            }

            var cartHeaderFromDb = await _applicationDbContext.CartHeaders
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == cart.CartHeader.UserId);

            if (cartHeaderFromDb == null)
            {
                cart.CartHeader.CreatedDate = DateTime.Now;
                _applicationDbContext.CartHeaders.Add(cart.CartHeader);
                await _applicationDbContext.SaveChangesAsync();

                cart.CartDetails.First().CartHeaderId = cart.CartHeader.Id;
                cart.CartDetails.First().Performance = null;
                _applicationDbContext.CartDetails.Add(cart.CartDetails.First());
                await _applicationDbContext.SaveChangesAsync();
            }
            else
            {
                var cartDetailsFromDb = await _applicationDbContext.CartDetails
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p =>
                        p.PerformanceId == cart.CartDetails.First().PerformanceId &&
                        p.CartHeaderId == cartHeaderFromDb.Id);

                if (cartDetailsFromDb == null)
                {
                    cart.CartDetails.First().CartHeaderId = cartHeaderFromDb.Id;
                    cart.CartDetails.First().Performance = null;
                    _applicationDbContext.CartDetails.Add(cart.CartDetails.First());
                    await _applicationDbContext.SaveChangesAsync();
                }
                else
                {
                    cart.CartDetails.First().Performance = null;
                    cart.CartDetails.First().Quantity += cartDetailsFromDb.Quantity;
                    cart.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                    cart.CartDetails.First().Id = cartDetailsFromDb.Id;
                    _applicationDbContext.CartDetails.Update(cart.CartDetails.First());
                    await _applicationDbContext.SaveChangesAsync();
                }
            }

            return _mapper.Map<CartDto>(cart);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error creating/updating cart", ex);
        }
    }

    public async Task<bool> RemoveFromCart(Guid cartDetailsId)
    {
        try
        {
            var cartDetails = await _applicationDbContext.CartDetails
                .FirstOrDefaultAsync(u => u.Id == cartDetailsId);

            if (cartDetails == null)
                return false;

            int totalCountofCartItems = _applicationDbContext.CartDetails
                .Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();

            _applicationDbContext.CartDetails.Remove(cartDetails);

            if (totalCountofCartItems == 1)
            {
                var cartHeaderToRemove = await _applicationDbContext.CartHeaders
                    .FirstOrDefaultAsync(u => u.Id == cartDetails.CartHeaderId);
                _applicationDbContext.CartHeaders.Remove(cartHeaderToRemove);
            }

            await _applicationDbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> ApplyCoupon(string userId, string couponCode)
    {
        var cartFromDb = await _applicationDbContext.CartHeaders
            .FirstOrDefaultAsync(u => u.UserId == userId);

        if (cartFromDb == null)
            return false;

        cartFromDb.CouponCode = couponCode;
        _applicationDbContext.CartHeaders.Update(cartFromDb);
        await _applicationDbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveCoupon(string userId)
    {
        var cartFromDb = await _applicationDbContext.CartHeaders
            .FirstOrDefaultAsync(u => u.UserId == userId);

        if (cartFromDb == null)
            return false;

        cartFromDb.CouponCode = "";
        _applicationDbContext.CartHeaders.Update(cartFromDb);
        await _applicationDbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ClearCart(string userId)
    {
        var cartHeaderFromDb = await _applicationDbContext.CartHeaders
            .FirstOrDefaultAsync(u => u.UserId == userId);

        if (cartHeaderFromDb == null)
            return false;

        _applicationDbContext.CartDetails
            .RemoveRange(_applicationDbContext.CartDetails.Where(u => u.CartHeaderId == cartHeaderFromDb.Id));
        _applicationDbContext.CartHeaders.Remove(cartHeaderFromDb);
        await _applicationDbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ValidateCart(CartDto cartDto)
    {
        foreach (var item in cartDto.CartDetails)
        {
            var performance = await _applicationDbContext.Performances
                .FirstOrDefaultAsync(p => p.Id == item.PerformanceId);

            if (performance == null ||
                performance.AvailableSeats < item.Quantity ||
                performance.Status != PerformanceStatus.Scheduled)
                return false;
        }

        return true;
    }

    public async Task<CartDetailsDto> GetCartDetail(Guid cartDetailId)
    {
        var cartDetail = await _applicationDbContext.CartDetails
            .Include(cd => cd.Performance)
            .FirstOrDefaultAsync(cd => cd.Id == cartDetailId);

        return _mapper.Map<CartDetailsDto>(cartDetail);
    }


    public async Task<bool> UpdateQuantity(Guid cartDetailId, int quantity)
    {
        try
        {
            var cartDetail = await _applicationDbContext.CartDetails
                .Include(cd => cd.Performance)
                .FirstOrDefaultAsync(cd => cd.Id == cartDetailId);

            if (cartDetail == null || quantity < 1)
                return false;

            if (cartDetail.Performance.AvailableSeats < quantity)
                return false;

            cartDetail.Quantity = quantity;
            await _applicationDbContext.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ValidateSeats(Guid performanceId, string seats)
    {
        var performance = await _applicationDbContext.Performances
            .FirstOrDefaultAsync(p => p.Id == performanceId);

        if (performance == null)
            return false;

        var seatList = seats.Split(',').Select(s => s.Trim()).ToList();

        return seatList.Count <= performance.AvailableSeats;
    }

    public async Task<decimal> CalculateTotal(string userId)
    {
        var cart = await GetCartByUserId(userId);
        if (cart == null || cart.CartDetails == null)
            return 0;

        return cart.CartDetails.Sum(item => item.PricePerTicket * item.Quantity);
    }

    public async Task<bool> IsPerformanceAvailable(Guid performanceId, int quantity)
    {
        var performance = await _applicationDbContext.Performances
            .FirstOrDefaultAsync(p => p.Id == performanceId);

        return performance != null &&
               performance.Status == PerformanceStatus.Scheduled &&
               performance.AvailableSeats >= quantity;
    }

    public async Task<bool> LockSeats(Guid performanceId, string seats, TimeSpan duration)
    {
        try
        {
            var performance = await _applicationDbContext.Performances
                .FirstOrDefaultAsync(p => p.Id == performanceId);

            if (performance == null)
                return false;
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<CartStatus> GetCartStatus(string userId)
    {
        var cartHeader = await _applicationDbContext.CartHeaders
            .FirstOrDefaultAsync(ch => ch.UserId == userId);

        if (cartHeader == null)
            return CartStatus.Expired;

        if ((DateTime.Now - cartHeader.CreatedDate).TotalHours > 24)
            return CartStatus.Expired;

        return CartStatus.Active;
    }

    public async Task<bool> SaveCartForLater(string userId)
    {
        try
        {
            var cartHeader = await _applicationDbContext.CartHeaders
                .FirstOrDefaultAsync(ch => ch.UserId == userId);

            if (cartHeader == null)
                return false;

            cartHeader.UpdatedDate = DateTime.Now;
            await _applicationDbContext.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }
}
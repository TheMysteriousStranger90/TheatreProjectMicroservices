using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheatreProject.PerformanceAPI.Data;
using TheatreProject.PerformanceAPI.Models;
using TheatreProject.PerformanceAPI.Models.Dto;

namespace TheatreProject.PerformanceAPI.Repositories;

public class PerformanceRepository : IPerformanceRepository
{
    private readonly ApplicationDbContext _context;
    private IMapper _mapper;

    public PerformanceRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PerformanceDto> CreateUpdatePerformance(PerformanceDto performanceDto)
    {
        Performance performance = _mapper.Map<PerformanceDto, Performance>(performanceDto);

        if (performance.Id == Guid.Empty)
        {
            performance.Id = Guid.NewGuid();
            performance.CreatedDate = DateTime.UtcNow;
            performance.Status = PerformanceStatus.Scheduled;
            performance.AvailableSeats = performance.Capacity;
            await _context.Performances.AddAsync(performance);
        }
        else
        {
            performance.UpdatedDate = DateTime.UtcNow;
            _context.Performances.Update(performance);
        }

        await _context.SaveChangesAsync();
        return _mapper.Map<Performance, PerformanceDto>(performance);
    }

    public async Task<bool> DeletePerformance(Guid id)
    {
        try
        {
            Performance performance = await _context.Performances.FirstOrDefaultAsync(x => x.Id == id);
            if (performance == null) return false;

            _context.Performances.Remove(performance);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<PerformanceDto> GetPerformanceById(Guid id)
    {
        Performance performance = await _context.Performances
            .FirstOrDefaultAsync(p => p.Id == id);
        return _mapper.Map<PerformanceDto>(performance);
    }

    public async Task<IEnumerable<PerformanceDto>> GetPerformances()
    {
        List<Performance> performances = await _context.Performances.ToListAsync();
        return _mapper.Map<List<PerformanceDto>>(performances);
    }

    public async Task<IEnumerable<PerformanceDto>> GetUpcomingPerformances()
    {
        List<Performance> performances = await _context.Performances
            .Where(p => p.ShowDateTime > DateTime.Now)
            .OrderBy(p => p.ShowDateTime)
            .ToListAsync();
        return _mapper.Map<List<PerformanceDto>>(performances);
    }

    public async Task<PagedResponse<PerformanceDto>> GetFilteredPerformances(PerformanceQueryParameters parameters)
    {
        var query = _context.Performances.AsQueryable();

        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
        {
            query = query.Where(p => p.Name.Contains(parameters.SearchTerm) ||
                                     p.Description.Contains(parameters.SearchTerm));
        }

        if (parameters.Category.HasValue)
        {
            query = query.Where(p => p.Category == parameters.Category);
        }

        if (parameters.StartDate.HasValue)
        {
            query = query.Where(p => p.ShowDateTime >= parameters.StartDate);
        }

        if (parameters.EndDate.HasValue)
        {
            query = query.Where(p => p.ShowDateTime <= parameters.EndDate);
        }

        var totalRecords = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalRecords / (double)parameters.PageSize);

        var orderedQuery = parameters.SortBy.ToLower() switch
        {
            "price" => parameters.IsDescending
                ? query.OrderByDescending(p => p.BasePrice)
                : query.OrderBy(p => p.BasePrice),
            _ => parameters.IsDescending
                ? query.OrderByDescending(p => p.ShowDateTime)
                : query.OrderBy(p => p.ShowDateTime)
        };

        var performances = await orderedQuery
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        return new PagedResponse<PerformanceDto>
        {
            Data = _mapper.Map<IEnumerable<PerformanceDto>>(performances),
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize,
            TotalPages = totalPages,
            TotalRecords = totalRecords
        };
    }

    public async Task<PerformanceStatistics> GetPerformanceStatistics(Guid id)
    {
        var performance = await _context.Performances
            .FirstOrDefaultAsync(p => p.Id == id);

        if (performance == null)
            return null;

        return new PerformanceStatistics
        {
            TotalBookings = performance.Capacity - performance.AvailableSeats,
            TotalRevenue = (performance.Capacity - performance.AvailableSeats) * performance.BasePrice,
            AvailableSeats = performance.AvailableSeats,
            OccupancyRate = ((double)(performance.Capacity - performance.AvailableSeats) / performance.Capacity) * 100
        };
    }

    public async Task<bool> UpdatePerformanceStatus(Guid id, PerformanceStatus status)
    {
        var performance = await _context.Performances.FirstOrDefaultAsync(p => p.Id == id);

        if (performance == null)
            return false;

        performance.Status = status;
        performance.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsPerformanceSoldOut(Guid id)
    {
        var performance = await _context.Performances
            .FirstOrDefaultAsync(p => p.Id == id);

        return performance?.AvailableSeats == 0;
    }
}
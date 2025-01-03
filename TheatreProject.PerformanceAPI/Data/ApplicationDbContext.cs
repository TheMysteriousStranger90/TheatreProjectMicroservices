using Microsoft.EntityFrameworkCore;
using TheatreProject.PerformanceAPI.Models;

namespace TheatreProject.PerformanceAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Performance> Performances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Performance>()
            .Property(p => p.BasePrice)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Performance>()
            .Property(p => p.Revenue)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Performance>().HasData(
            new Performance
            {
                Id = Guid.NewGuid(),
                Name = "Romeo and Juliet",
                Description = "Shakespeare's classic tragedy of star-crossed lovers",
                TheatreName = "Royal Theatre",
                City = "London",
                Address = "123 Theatre St",
                Capacity = 500,
                BasePrice = 50.00M,
                ShowDateTime = DateTime.Now.AddDays(7),
                Duration = TimeSpan.FromHours(2.5),
                Category = TheatreCategory.Drama,
                AvailableSeats = 500,
                ImageUrl = "https://azurestorage90bh.blob.core.windows.net/theatre/Romeo%20and%20Juliet.jpg",
                Status = PerformanceStatus.Scheduled,
                CreatedDate = DateTime.UtcNow,
                TotalBookings = 0,
                Revenue = 0
            },
            new Performance
            {
                Id = Guid.NewGuid(),
                Name = "Swan Lake",
                Description = "Classic ballet by Tchaikovsky",
                TheatreName = "Bolshoi Theatre",
                City = "Moscow",
                Address = "1 Theatre Square",
                Capacity = 800,
                BasePrice = 75.00M,
                ShowDateTime = DateTime.Now.AddDays(14),
                Duration = TimeSpan.FromHours(3),
                Category = TheatreCategory.Ballet,
                AvailableSeats = 800,
                ImageUrl = "https://azurestorage90bh.blob.core.windows.net/theatre/Swan%20Lake.jpg",
                Status = PerformanceStatus.Scheduled,
                CreatedDate = DateTime.UtcNow,
                TotalBookings = 0,
                Revenue = 0
            },
            new Performance
            {
                Id = Guid.NewGuid(),
                Name = "The Phantom of the Opera",
                Description = "Andrew Lloyd Webber's masterpiece",
                TheatreName = "Broadway Theatre",
                City = "New York",
                Address = "1681 Broadway",
                Capacity = 600,
                BasePrice = 100.00M,
                ShowDateTime = DateTime.Now.AddDays(5),
                Duration = TimeSpan.FromHours(2.5),
                Category = TheatreCategory.Musical,
                AvailableSeats = 600,
                ImageUrl = "https://azurestorage90bh.blob.core.windows.net/theatre/The%20Phantom%20of%20the%20Opera.jpg",
                Status = PerformanceStatus.Scheduled,
                CreatedDate = DateTime.UtcNow,
                TotalBookings = 0,
                Revenue = 0
            },
            new Performance
            {
                Id = Guid.NewGuid(),
                Name = "The Comedy of Errors",
                Description = "Shakespeare's shortest and most farcical comedy",
                TheatreName = "Globe Theatre",
                City = "London",
                Address = "21 New Globe Walk",
                Capacity = 400,
                BasePrice = 40.00M,
                ShowDateTime = DateTime.Now.AddDays(10),
                Duration = TimeSpan.FromHours(2),
                Category = TheatreCategory.Comedy,
                AvailableSeats = 400,
                ImageUrl = "https://azurestorage90bh.blob.core.windows.net/theatre/The%20Comedy%20of%20Errors.jpg",
                Status = PerformanceStatus.Scheduled,
                CreatedDate = DateTime.UtcNow,
                TotalBookings = 0,
                Revenue = 0
            },
            new Performance
            {
                Id = Guid.NewGuid(),
                Name = "La Traviata",
                Description = "Verdi's beloved opera",
                TheatreName = "Teatro alla Scala",
                City = "Milan",
                Address = "Via Filodrammatici 2",
                Capacity = 700,
                BasePrice = 85.00M,
                ShowDateTime = DateTime.Now.AddDays(21),
                Duration = TimeSpan.FromHours(3),
                Category = TheatreCategory.Opera,
                AvailableSeats = 700,
                ImageUrl = "https://azurestorage90bh.blob.core.windows.net/theatre/La%20Traviata.jpg",
                Status = PerformanceStatus.Scheduled,
                CreatedDate = DateTime.UtcNow,
                TotalBookings = 0,
                Revenue = 0
            }
        );
    }
}
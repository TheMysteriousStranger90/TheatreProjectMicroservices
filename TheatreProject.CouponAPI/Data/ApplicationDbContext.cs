using Microsoft.EntityFrameworkCore;
using TheatreProject.CouponAPI.Models;

namespace TheatreProject.CouponAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Coupon> Coupons { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Coupon>().HasData(
            new Coupon
            {
                Id = Guid.NewGuid(),
                CouponCode = "NEW10",
                DiscountAmount = 10
            },
            new Coupon
            {
                Id = Guid.NewGuid(),
                CouponCode = "EARLY20",
                DiscountAmount = 20
            },
            new Coupon
            {
                Id = Guid.NewGuid(),
                CouponCode = "GROUP15",
                DiscountAmount = 15
            },
            new Coupon
            {
                Id = Guid.NewGuid(),
                CouponCode = "VIP25",
                DiscountAmount = 25
            }
        );
    }
}
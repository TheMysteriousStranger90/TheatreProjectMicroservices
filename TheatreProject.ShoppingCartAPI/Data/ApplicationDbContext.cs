using Microsoft.EntityFrameworkCore;
using TheatreProject.ShoppingCartAPI.Models;

namespace TheatreProject.ShoppingCartAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { }
    
    public DbSet<Performance> Performances { get; set; }
    public DbSet<CartHeader> CartHeaders { get; set; }
    public DbSet<CartDetails> CartDetails { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CartDetails>()
            .Property(c => c.PricePerTicket)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Performance>(entity =>
        {
            entity.Property(p => p.BasePrice)
                .HasPrecision(18, 2);

            entity.Property(p => p.Revenue)
                .HasPrecision(18, 2);
        });
    }
}
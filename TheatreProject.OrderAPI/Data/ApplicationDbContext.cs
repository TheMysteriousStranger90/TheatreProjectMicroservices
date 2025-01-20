using Microsoft.EntityFrameworkCore;
using TheatreProject.OrderAPI.Models;

namespace TheatreProject.OrderAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<OrderDetails> OrderDetails { get; set; }
    public DbSet<OrderHeader> OrderHeaders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
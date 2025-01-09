using Microsoft.EntityFrameworkCore;

namespace TheatreProject.ShoppingCartAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { }
}
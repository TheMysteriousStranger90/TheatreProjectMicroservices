using Microsoft.EntityFrameworkCore;
using TheatreProject.EmailAPI.Models;

namespace TheatreProject.EmailAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<EmailLogger> EmailLoggers { get; set; }
}
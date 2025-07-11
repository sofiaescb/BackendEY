using Microsoft.EntityFrameworkCore;
using BackendEY.Models;

namespace BackendEY.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

        public DbSet<Proveedor> Proveedores { get; set; } = null!;
    }
}

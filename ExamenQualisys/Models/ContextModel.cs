using Microsoft.EntityFrameworkCore;

namespace ExamenQualisys.Models
{
    public class ContextModel: DbContext
    {
        public ContextModel(DbContextOptions<ContextModel> options) : base(options) 
        { 
        }

        public DbSet<Articulos> Articulos { get; set; }
        public DbSet<Almacenes> Almacenes { get; set; }
        public DbSet<Stock> Stocks { get; set; }
    }
}
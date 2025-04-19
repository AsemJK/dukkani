using CatalogApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Infrastructure.Data
{
    public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
    }
}

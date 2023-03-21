using CreateCSV.Model;
using Microsoft.EntityFrameworkCore;

namespace CreateCSV
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
    }
}

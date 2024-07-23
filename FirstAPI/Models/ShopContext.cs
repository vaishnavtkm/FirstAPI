using Microsoft.EntityFrameworkCore;

namespace FirstAPI.Models
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {

        }
        /*For creating a relation between entities, define a modelbuilder*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(s => s.Category)
                .HasForeignKey(s => s.CategoryId);

            modelBuilder.Seed();
        }

        

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> categories { get; set; }

    }    
}

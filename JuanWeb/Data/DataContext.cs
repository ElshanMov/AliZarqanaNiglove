using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Data
{
    public class DataContext: IdentityDbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext>options):base(options)
        {
            
        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }  
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Brand> Brands{ get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<ProductSizes> ProductSizes { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}

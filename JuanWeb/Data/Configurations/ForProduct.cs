using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configurations
{
    public class ForProduct : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("tbl_products_shoes");
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(500);
            builder.Property(x => x.CategoryId).IsRequired();
            builder.Property(x => x.CostPrice).IsRequired();
            builder.Property(x => x.SalePrice).IsRequired();
            builder.Property(x => x.InStock).IsRequired();





        }
    }
}

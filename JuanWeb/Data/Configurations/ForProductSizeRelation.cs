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
    public class ForProductSizeRelation : IEntityTypeConfiguration<ProductSizes>
    {
        public void Configure(EntityTypeBuilder<ProductSizes> builder)
        {
            builder.ToTable("tbl_ProductSizeRelatiobs");
        }
    }
}

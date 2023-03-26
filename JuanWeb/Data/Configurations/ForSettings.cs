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
    public class ForSettings : IEntityTypeConfiguration<Settings>
    {
        public void Configure(EntityTypeBuilder<Settings> builder)
        {
            builder.ToTable("tbl_allSettings");
            builder.Property(x => x.Key).IsRequired().HasMaxLength(45);
            builder.Property(x => x.Value).IsRequired().HasMaxLength(245);

        }
    }
}

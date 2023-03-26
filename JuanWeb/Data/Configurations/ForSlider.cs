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
    public class ForSlider : IEntityTypeConfiguration<Slider>
    {
        public void Configure(EntityTypeBuilder<Slider> builder)
        {
            builder.ToTable("tbl_home_Slider");
            builder.Property(x => x.Title).IsRequired().HasMaxLength(50);
            builder.Property(x => x.SecondTitle).IsRequired().HasMaxLength(120);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Image).IsRequired();




        }
    }
}

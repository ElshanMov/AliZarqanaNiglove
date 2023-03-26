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
    public class ForContact : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("tbl_contacts");
            builder.Property(x=>x.Name).IsRequired();
            builder.Property(x=>x.Email).IsRequired().HasMaxLength(120);
            builder.Property(x => x.Subject).IsRequired().HasMaxLength(100);
            builder.Property(x=>x.Message).IsRequired().HasMaxLength(400);
        }
    }
}

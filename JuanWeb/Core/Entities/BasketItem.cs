using Core.Common;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class BasketItem:BaseEntity
    {
        public int ProductId { get; set; }
        public string AppUserId { get; set; }
        public int Count { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; }
        public Product Product { get; set; }
        public AppUser AppUser { get; set; }
    }
}

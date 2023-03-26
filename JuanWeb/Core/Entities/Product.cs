using Core.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; } 
        public Category Category { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountPrice { get; set; }
        public int InStock { get; set; }
        public int SellCount { get; set; }
        public string Information { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        [NotMapped]
        public IFormFile PosterImage { get; set; }
        [NotMapped]
        public List<IFormFile> ImageFiles { get; set; }
        [NotMapped]
        public List<int> ProductImageIds { get; set; } = new List<int>();
        public Color Color { get; set; }
        public int ColorId { get; set; }
        public List<ProductSizes> Sizes { get; set; }
        [NotMapped]
        public List<int> SizeIds { get; set; } = new List<int>();
        public List<ProductComment> ProductComments { get; set; }
    }
}

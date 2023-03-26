using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class ProductViewModel
    {

        public Product Product { get; set; }
        public ProductComment ProductComment { get; set; }
        public List<Product> RelatedProduct { get; set; }
    }
}

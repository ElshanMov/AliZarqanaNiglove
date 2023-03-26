using Core.Entities;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Implementations
{
    public class ProductImageRepository:Repository<ProductImage>,IProductImagesRepository
    {
        public ProductImageRepository(DataContext context):base(context)
        {
            
        }
    }
}

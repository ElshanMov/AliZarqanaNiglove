using Core.Entities;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Implementations
{
    public class ProductCommentRepository : Repository<ProductComment>, IProductCommentRepository
    {
        public ProductCommentRepository(DataContext context) : base(context)
        {
        }
    }
}

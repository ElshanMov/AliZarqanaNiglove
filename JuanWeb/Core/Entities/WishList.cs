using Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class WishList:BaseEntity
    {
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}

using Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ProductComment:BaseEntity
    {
        public string AppUserId { get; set; }
        public DateTime SendTime { get; set; }
        public int ProductId { get; set; }
        public string Text { get; set; }
        public bool Status { get; set; }
        public string FullName { get; set; }
        public Product Product { get; set; }
        public AppUser AppUser { get; set; }
    }
}

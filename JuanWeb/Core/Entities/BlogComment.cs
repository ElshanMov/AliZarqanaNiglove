using Core.Common;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class BlogComment : BaseEntity
    {
        public DateTime SendTime { get; set; }

        public int BlogId { get; set; }
        public Blog Blog{ get; set; }


        public string Text { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
    }
}

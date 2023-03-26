using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class BlogViewModel
    {
        public List<Blog> Blogs { get; set; }
        public Blog Blog { get; set; }
        public List<Category> Categories { get; set; }
        public List<int> ProductCounts { get; set; }
        public List<BlogComment> BlogComments { get; set; }
        public BlogComment BlogComment { get; set; }

    }
}

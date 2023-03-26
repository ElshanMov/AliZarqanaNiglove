using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class HomeViewModel
    {

        public List<Brand> Brands { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Product> Products { get; set; }
        public List<Product> TopSeller { get; set; }
        public List<Slider> Sliders { get; set; }
    }
}

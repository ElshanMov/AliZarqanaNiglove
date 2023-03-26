using Core.Entities;
using Data;
using Data.Repositories.Implementations;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Implementations
{
    public class SliderService : Repository<Slider>, ISliderService
    {
        public SliderService(DataContext context) : base(context)
        {
        }
    }
}

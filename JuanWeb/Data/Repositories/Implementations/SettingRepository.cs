using Core.Entities;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Implementations
{
    public class SettingRepository : Repository<Settings>, ISettingRepository
    {
        public SettingRepository(DataContext context) : base(context)
        {
        }
    }
}

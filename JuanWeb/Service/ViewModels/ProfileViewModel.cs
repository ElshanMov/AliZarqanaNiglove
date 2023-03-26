using Core.Entities;
using Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class ProfileViewModel
    {
        public MemberUpdateDto Member { get; set; }
        public List<Order> Orders { get; set; }    
    }
}

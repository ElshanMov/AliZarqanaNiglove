using Core.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Settings:BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}

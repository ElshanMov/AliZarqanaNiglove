using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class MemberUpdateDto
    {
        [Required]
        [StringLength(maximumLength: 30, MinimumLength = 5)]
        public string FullName { get; set; }
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress), StringLength(maximumLength: 50, MinimumLength = 4)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password), StringLength(maximumLength: 25, MinimumLength = 8)]
        public string Password { get; set; }
        [Required, DataType(DataType.Password), StringLength(maximumLength: 25, MinimumLength = 8)]
        public string RepeatPassword { get; set; }
        [Required, DataType(DataType.Password), StringLength(maximumLength: 25, MinimumLength = 8)]
        public string CurrentPassword { get; set; }
    }
}

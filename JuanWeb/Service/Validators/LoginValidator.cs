using FluentValidation;
using Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Validators
{
    public class LoginValidator:AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x=>x.Email).NotNull().NotEmpty().WithMessage("Email boş buraxıla bilməz.").MinimumLength(4).WithMessage("Email ən azı 4 simvoldan ibarət olmalıdır.").MaximumLength(50).WithMessage("Email 50 simvoldan artıq ola bilməz");
            RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Şifrə boş buraxıla bilməz.").MinimumLength(8).WithMessage("Şifrənin ən azı 8 simvoldan ibarət olmalıdır.").MaximumLength(25).WithMessage("Şifrənin 25 simvoldan artıq ola bilməz");
        }
    }
}

using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Validators
{
    public class FileValidator:AbstractValidator<IFormFile>
    {
        //todo Must be refactor
        public FileValidator() {
            RuleFor(x => x.Length).NotNull().WithMessage("Şəkil boş buraxıla bilməz.").LessThanOrEqualTo(2097152).WithMessage("Şəkilin ölçüsü 2MB-dan artıq ola bilməz.");
            RuleFor(x => x.ContentType).NotNull().Must(x => x.Equals("image/jpeg") || x.Equals("image/jpg") || x.Equals("image/png")).WithMessage("Fayl Tipi düzgün deyil");
        }
       

        
    }
}

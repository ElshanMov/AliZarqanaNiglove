using Core.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Validators
{
    public class SliderCreateValidator:AbstractValidator<Slider>
    {
        public SliderCreateValidator()
        {          
            RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage("Başlıq boş buraxıla bilməz.").MaximumLength(50).WithMessage("Başlıq 50 simvoldan artıq ola bilməz");
            RuleFor(x => x.SecondTitle).NotNull().NotEmpty().WithMessage("Ikinci başlıq boş buraxıla bilməz.").MaximumLength(120).WithMessage("Ikinci başlıq 120 simvoldan artıq ola bilməz.");
            RuleFor(x => x.Description).NotNull().NotEmpty().WithMessage("Təsvir boş buraxıla bilməz.").MaximumLength(250).WithMessage("Təsvir 250 simvoldan artıq ola bilməz.");
            RuleFor(x => x.ImageFile).SetValidator(new FileValidator());
        }

        
    }
}

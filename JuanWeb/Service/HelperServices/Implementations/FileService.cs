using Core.Entities;
using Microsoft.AspNetCore.Http;
using Service.CustomExceptions;
using Service.HelperServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.HelperServices.Implementations
{
    public class FileService : IFileService
    {
        public void CheckFileLenght(long value)
        {
            if (value > 2097152)
            {
                throw new ImageFileLenghtException("Şəkilin ölçüsü 2MB-dan artıq ola bilməz");
            }
        }

        public void CheckImageFile(IFormFile file)
        {
            if (file.ContentType != "image/jpeg" && file.ContentType != "image/png")
            {
                throw new CheckImageFileTypeException("Fayl Tipi düzgün deyil");
            }
        }

    }
}

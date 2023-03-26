using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.HelperServices.Interfaces
{
    public interface IFileService
    {
        void CheckFileLenght(long value);
        void CheckImageFile(IFormFile file);
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.HelperServices.Interfaces
{
    public interface ILocalService
    {
        Task<string> SaveAsync(string path, string folder,IFormFile file);
        Task<bool> DeleteAsync(string rootPath, string folder, string fileName);
    }
}

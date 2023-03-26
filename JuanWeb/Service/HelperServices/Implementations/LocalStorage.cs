using Microsoft.AspNetCore.Http;
using Service.HelperServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Service.HelperServices.Implementations
{
    public class LocalStorage : ILocalService
    {
        public async Task<string> SaveAsync(string rootPath, string folder,IFormFile file)
        {
            string fileName = file.FileName;
            fileName=await FileRenameAsync(Path.Combine(rootPath,folder), fileName);
            string path = Path.Combine(rootPath, folder, fileName);
            using FileStream stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
            return fileName;
        }

        public async Task<bool> DeleteAsync(string rootPath, string folder, string fileName)
        {
            string pathDelete=Path.Combine(rootPath, folder,fileName);
            if (File.Exists(pathDelete))
            {
                File.Delete(pathDelete);
                return true;
            }
            return false;

        }

        private async Task<string> FileRenameAsync(string path, string fileName)
        {
            var result = Task.Run<string>(() =>
            {

                string extension = Path.GetExtension(fileName);

                string oldName = Path.GetFileNameWithoutExtension(fileName);

                Regex regex = new("[ *'\",+-._&#^@|/<>~]");

                string ceoFriendlyName = regex.Replace(oldName, "-");

                var files = Directory.GetFiles(path, ceoFriendlyName + "*");

                if (files.Length == 0) return ceoFriendlyName + "-1" + extension;

                int[] fileNumbers = new int[files.Length];

                int lastHyphenIndex;
                for (int i = 0; i < fileNumbers.Length; i++)
                {
                    lastHyphenIndex = files[i].LastIndexOf("-");
                    fileNumbers[i] = int.Parse(files[i].Substring(lastHyphenIndex + 1, files[i].Length - extension.Length - lastHyphenIndex - 1));
                }
                var biggestNumber = fileNumbers.Max();
                biggestNumber += 1;
                return ceoFriendlyName + "-" + biggestNumber + extension;
            });
            return await result;
        }

    }
}

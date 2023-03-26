using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.CustomExceptions
{
    public class ImageFileLenghtException : Exception
    {
        public ImageFileLenghtException(string err):base(err)
        {

        }
    }
}

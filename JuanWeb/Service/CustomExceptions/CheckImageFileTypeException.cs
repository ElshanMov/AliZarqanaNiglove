using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.CustomExceptions
{
    public class CheckImageFileTypeException : Exception
    {

        public CheckImageFileTypeException(string err):base(err)
        {

        }

    }
}

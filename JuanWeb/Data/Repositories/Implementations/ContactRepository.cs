using Core.Entities;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Implementations
{
    public class ContactRepository:Repository<Contact>,IContactRepository
    {
        public ContactRepository(DataContext context):base(context)
        {
            
        }
    }
}

using EasySave.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.DataAccess.Services
{
    public class JsonFileService : IFileService
    {
        public JsonFileService() 
        {
            
        }

        public List<T>? Read<T>()
        {
            throw new NotImplementedException();
        }

        public void Write<T>(List<T> list)
        {
            throw new NotImplementedException();
        }
    }
}

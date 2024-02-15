using EasySave.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.DataAccess.Services.Factories
{
    public interface IFileServiceFactory
    {
        public abstract IFileService CreateFileService(string type, string filePath);
    }
}

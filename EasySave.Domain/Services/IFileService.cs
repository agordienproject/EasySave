using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Domain.Services
{
    public interface IFileService
    {
        List<T>? Read<T>();
        void Write<T>(List<T> list);
    }
}

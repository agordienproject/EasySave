using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Domain.Services
{
    public interface IFileService
    {
        Task<List<T>?> Read<T>();
        Task Write<T>(List<T> list);
    }
}

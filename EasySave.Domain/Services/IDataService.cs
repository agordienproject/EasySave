using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Domain.Services
{
    public interface IDataService<T>
    {
        Task<IEnumerable<T>> GetAll();

        Task<T?> Get(string name);

        Task<T> Create(T entity);

        Task<T?> Update(string name, T entity);

        Task<bool> Delete(string name);
    }
}

using EasySave.Domain.Models;
using EasySave.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.DataAccess.Services
{
    public class LogService : ILogService
    {
        public Task<Log> Create(Log entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Log> Get(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Log>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Log> Update(string name, Log entity)
        {
            throw new NotImplementedException();
        }
    }
}

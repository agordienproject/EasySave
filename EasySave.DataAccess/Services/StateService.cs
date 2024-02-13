using EasySave.Domain.Models;
using EasySave.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.DataAccess.Services
{
    public class StateService : IStateService
    {
        public Task<State> Create(State entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string name)
        {
            throw new NotImplementedException();
        }

        public Task<State> Get(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<State>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<State> Update(string name, State entity)
        {
            throw new NotImplementedException();
        }
    }
}

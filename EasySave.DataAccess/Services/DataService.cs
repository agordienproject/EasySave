using EasySave.Domain.Models;
using EasySave.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.DataAccess.Services
{
    public class DataService<T> : IDataService<T> where T : NamedEntity
    {
        protected IFileService _fileService { get; set; }

        public DataService() { }

        public async Task<IEnumerable<T>> GetAll()
        {
            List<T> list = await _fileService.Read<T>();

            if(list == null)
                return new List<T>();

            return list;
        }

        public async Task<T?> Get(string name)
        {
            List<T> list = (List<T>)await GetAll();

            T? entity = list.FirstOrDefault(entity => entity.BackupName == name); ;

            return entity;
        }

        public async Task<T> Create(T entity)
        {
            List<T> list = (List<T>)await GetAll();

            list.Add(entity);

            await _fileService.Write<T>(list);

            return entity;
        }

        public async Task<T?> Update(T entity)
        {
            List<T> list = (List<T>)await GetAll();
            T? existingEntity = list.FirstOrDefault(x => x.BackupName == entity.BackupName);

            if (existingEntity != null)
            {
                int index = list.IndexOf(existingEntity);
                list[index] = entity;

                await _fileService.Write(list);

                return entity;
            }

            return null;
        }

        public async Task<bool> Delete(string name)
        {
            List<T> list = (List<T>)await GetAll();

            T? entityToDelete = list.FirstOrDefault(entity => entity.BackupName == name);

            if (entityToDelete != null)
            {
                list.Remove(entityToDelete);

                await _fileService.Write(list);

                return true;
            }

            return false;
        }

    }
}

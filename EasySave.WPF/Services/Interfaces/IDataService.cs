namespace EasySave.Services.Interfaces
{
    public interface IDataService<T>
    {
        Task<IEnumerable<T>> GetAll();

        Task<T?> Get(string name);

        Task<T> Create(T entity);

        Task<T?> Update(T entity);

        Task<bool> Delete(string name);
    }
}

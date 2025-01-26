namespace ControlStoreAPI.Data.Interface
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetItems();
        Task<T> GetItem(int id);
        Task<IEnumerable<T>> GenericQuery(IQueryable<T> query);
        Task Put(T imovel);
        Task<T> Post(T item);
        Task Delete(int id);
        Task<bool> Exists(int id);
        void RemoveContex(T item);
        void Detached(T item);
        IQueryable<T> Query();
        int GetLasdOrOne();
    }
}

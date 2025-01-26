using ControlStoreAPI.Data.Model;

namespace ControlStoreAPI.Services.Interface
{
    public interface IStatusClientService
    {
        Task<IEnumerable<StatusCliente>> GetItems();
        Task<StatusCliente> GetItem(int id);
        Task Put(StatusCliente item);
        Task<StatusCliente> Post(StatusCliente item);
        Task Delete(int id);
        Task <bool> Exists(int id);
        Task RemoveContex(StatusCliente item);
        Task Detached(StatusCliente item);
        Task<int> GetLasdOrOne();
    }
}

using ControlStoreAPI.Data.Model;

namespace ControlStoreAPI.Services.Interface
{
    public interface IClientService
    {
        Task<IEnumerable<Cliente>> GetItems();
        Task<Cliente> GetItem(int id);
        Task<Cliente> GetItemByCPF(string cpf);
        Task<Cliente> GetItemByRG(string rg);
        Task<Cliente> GetItemByNome(string rg);
        Task<Cliente> GetItemByTelefone(string rg);
        Task<Cliente> GetItemByUsuario(int userId);
        Task Put(Cliente item);
        Task<Cliente> Post(Cliente item);
        Task Delete(int id);
        Task<bool> Exists(int id);
        Task RemoveContex(Cliente item);
        Task<int> GetLasdOrOne();
        Task Detached(Cliente item);
    }
}

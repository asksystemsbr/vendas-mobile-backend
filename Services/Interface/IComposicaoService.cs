using ControlStoreAPI.Data.Model;

namespace ControlStoreAPI.Services.Interface
{
    public interface IComposicaoService
    {
        Task<IEnumerable<Composicao>> GetItems();
        Task<Composicao> GetItem(int id);
        Task Put(Composicao item);
        Task<Composicao> Post(Composicao item);
        Task Delete(int id);
        Task <bool> Exists(int id);
        Task RemoveContex(Composicao item);
        Task Detached(Composicao item);
        Task<int> GetLasdOrOne();
    }
}

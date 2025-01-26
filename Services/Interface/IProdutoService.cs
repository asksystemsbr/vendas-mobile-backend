using ControlStoreAPI.Data.Model;

namespace ControlStoreAPI.Services.Interface
{
    public interface IProdutoService
    {
        Task<IEnumerable<Produto>> GetItems();
        Task<Produto> GetItem(int id);
        Task Put(Produto item);
        Task<Produto> Post(Produto item);
        Task Delete(int id);
        Task <bool> Exists(int id);
        Task RemoveContex(Produto item);
        Task Detached(Produto item);
        Task<int> GetLasdOrOne();
        Task<List<Produto>> GetByGrupo(int grupoId);

    }
}

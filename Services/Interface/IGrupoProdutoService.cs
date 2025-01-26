using ControlStoreAPI.Data.Model;

namespace ControlStoreAPI.Services.Interface
{
    public interface IGrupoProdutoService
    {
        Task<IEnumerable<GrupoProduto>> GetItems();
        Task<GrupoProduto> GetItem(int id);
        Task Put(GrupoProduto item);
        Task<GrupoProduto> Post(GrupoProduto item);
        Task Delete(int id);
        Task <bool> Exists(int id);
        Task RemoveContex(GrupoProduto item);
        Task Detached(GrupoProduto item);
        Task<int> GetLasdOrOne();
    }
}

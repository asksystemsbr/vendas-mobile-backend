using ControlStoreAPI.Data.Model;

namespace ControlStoreAPI.Services.Interface
{
    public interface IComposicaoToProdutoService
    {
        Task<IEnumerable<ComposicaoToProduto>> GetItems();
        Task<IEnumerable<ComposicaoToProduto>> GetItemsByProduto(int produtoId);
        Task<ComposicaoToProduto> GetItem(int id);
        Task Put(ComposicaoToProduto item);
        Task<ComposicaoToProduto> Post(ComposicaoToProduto item);
        Task Delete(int id);
        Task <bool> Exists(int id);
        Task RemoveContex(ComposicaoToProduto item);
        Task Detached(ComposicaoToProduto item);
        Task<int> GetLasdOrOne();
    }
}

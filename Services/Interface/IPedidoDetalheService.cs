using ControlStoreAPI.Data.Model;

namespace ControlStoreAPI.Services.Interface
{
    public interface IPedidoDetalheService
    {
        Task<IEnumerable<PedidoDetalhe>> GetItems();
        Task<IEnumerable<PedidoDetalhe>> GetItemsByCabecalho(int idCabecalho);
        Task<PedidoDetalhe> GetItem(int id);
        Task Put(PedidoDetalhe item);
        Task<PedidoDetalhe> Post(PedidoDetalhe item);
        Task Delete(int id);
        Task<bool> Exists(int id);
        Task RemoveContex(PedidoDetalhe item);
        Task Detached(PedidoDetalhe item);
        Task<int> GetLasdOrOne();
        Task ClearDetail(List<Produto> items, PedidoCabecalho itemCabecalho);
        Task ClearDetailDeep(List<Produto> items, PedidoCabecalho itemCabecalho);
        Task SaveDetail(List<Produto> items, PedidoCabecalho itemCabecalho);

        Task DebitStock(List<Produto> items, PedidoCabecalho itemCabecalho);
        

    }
}
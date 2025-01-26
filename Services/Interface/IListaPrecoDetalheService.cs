using ControlStoreAPI.Data.Model;

namespace ControlStoreAPI.Services.Interface
{
    public interface IListaPrecoDetalheService
    {
        Task<IEnumerable<ListaPrecoDetalhe>> GetItems();
        Task<ListaPrecoDetalhe> GetItem(int id);
        Task Put(ListaPrecoDetalhe item);
        Task<ListaPrecoDetalhe> Post(ListaPrecoDetalhe item);
        Task Delete(int id);
        Task<bool> Exists(int id);
        Task RemoveContex(ListaPrecoDetalhe item);
        Task Detached(ListaPrecoDetalhe item);
        Task<int> GetLasdOrOne();
        Task SaveDetail(List<Produto> items
            , ListaPrecoCabecalho itemCabecalho);

    }
}
using ControlStoreAPI.Data.Model;
using Microsoft.AspNetCore.Mvc;

namespace ControlStoreAPI.Services.Interface
{
    public interface IListaPrecoCabecalhoService
    {
        Task<IEnumerable<ListaPrecoCabecalho>> GetItems();
        Task<ListaPrecoCabecalho> GetItem(int id);
        Task Put(ListaPrecoCabecalho item);
        Task<ListaPrecoCabecalho> Post(ListaPrecoCabecalho item);
        Task Delete(int id);
        Task<bool> Exists(int id);
        Task RemoveContex(ListaPrecoCabecalho item);
        Task Detached(ListaPrecoCabecalho item);
        Task<int> GetLasdOrOne();
        Task<IEnumerable<Produto>> GetProdutosPorCategoriaECliente(int categoriaId, int clienteId);

        Task<IEnumerable<Produto>> GetProdutosPorCliente(int clienteId);
        Task<ListaPrecoCabecalho> SaveList(int clientId);
    }
}
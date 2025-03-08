using ControlStoreAPI.Data.Model;
using Microsoft.AspNetCore.Mvc;

namespace ControlStoreAPI.Services.Interface
{
    public interface IPedidoCabecalhoService
    {
        Task<IEnumerable<PedidoCabecalho>> GetItems();
        Task<PedidoCabecalho> GetItem(int id);
        Task Put(PedidoCabecalho item);
        Task<PedidoCabecalho> Post(PedidoCabecalho item);
        Task Delete(int id);
        Task<bool> Exists(int id);
        Task RemoveContex(PedidoCabecalho item);
        Task Detached(PedidoCabecalho item);
        Task<int> GetLasdOrOne();
        Task<IEnumerable<Produto>> GetProdutosPorCategoriaECliente(int categoriaId, int clienteId);

        Task<IEnumerable<PedidoCabecalho>> GetPedidosByStatus(string status, int clienteId);
        Task<IEnumerable<PedidoCabecalho>> GetPedidosAllUsersByStatus(string status);

        Task<IEnumerable<Produto>> GetProdutosPorCliente(int clienteId);
        Task<PedidoCabecalho> SaveOrder(int clientId);

        Task SalvarArquivo(PedidoCabecalho pedido, string fileUrl, string folder);

        Task<IEnumerable<FilesOrder>> GetFilesByPedido(int pedidoId,string type,string extension);
    }
}
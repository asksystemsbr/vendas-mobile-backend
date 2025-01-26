using ControlStoreAPI.Data.Interface;
using ControlStoreAPI.Data.Model;
using ControlStoreAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ControlStoreAPI.Services
{
    public class PedidoDetalheService : IPedidoDetalheService
    {
        private readonly ILoggerService _loggerService;
        private readonly IRepository<PedidoDetalhe> _repository;
        private readonly IRepository<PedidoCabecalho> _repositoryCabecalho;
        private readonly IRepository<Produto> _repositoryProduto;

        public PedidoDetalheService(ILoggerService loggerService,
            IRepository<PedidoDetalhe> repository
            ,IRepository<PedidoCabecalho> repositoryCabecalho
            , IRepository<Produto> repositoryProduto
            )
        {
            _loggerService = loggerService;
            _repository = repository;
            _repositoryCabecalho = repositoryCabecalho;
            _repositoryProduto = repositoryProduto;
        }

        public async Task<IEnumerable<PedidoDetalhe>> GetItems()
        {
            var items = await _repository.GetItems();
            return items.OrderBy(x => x.ID);
        }

        public async Task<PedidoDetalhe> GetItem(int id)
        {
            var item = await _repository.GetItem(id);
            return item;
        }

        public async Task Put(PedidoDetalhe item)
        {
            await _repository.Put(item);
        }

        public async Task<PedidoDetalhe> Post(PedidoDetalhe item)
        {
            return await _repository.Post(item);
        }

        public async Task ClearDetail(List<Produto> items
            , PedidoCabecalho itemCabecalho)
        {

                //var prod = items.FirstOrDefault();
            //get products by category
            //var query = from detalhe in _repository.Query()
            //            join produto in _repositoryProduto.Query()
            //            on detalhe.ProdutoId equals produto.ID
            //            where produto.CategoriaProdutoId == prod.CategoriaProdutoId
            //            select detalhe;

            var produtosIds = items.Select(x => x.ID).ToList();


            var query = await _repository.Query().
                Where(x=>x.PedidoCabecalhoId==itemCabecalho.ID
                && produtosIds.Contains(x.ProdutoId))
                .ToListAsync();

                //clean items
                foreach (var item in query)
                {
                    await _repository.Delete(item.ID);
                }
        }

        public async Task ClearDetailDeep(List<Produto> items
           , PedidoCabecalho itemCabecalho)
        {
            var query = await _repository.Query().
                Where(x => x.PedidoCabecalhoId == itemCabecalho.ID)
                .ToListAsync();

            //clean items
            foreach (var item in query)
            {
                await _repository.Delete(item.ID);
            }
        }

        public async Task SaveDetail(List<Produto> items
            , PedidoCabecalho itemCabecalho)
        {

            decimal total = 0;

           
            //save items
            foreach (var item in items)
            {
                PedidoDetalhe itemDet = new PedidoDetalhe();
                itemDet.PedidoCabecalhoId = itemCabecalho.ID;
                itemDet.ProdutoId = item.ID;
                itemDet.ID = 0;
                itemDet.EstoqueMinimo = item.EstoqueMin ?? 0;
                itemDet.EstoqueMaximo = item.EstoqueMax ?? 0;
                itemDet.Quantidade = item.QuantidadeEstoque ?? 0;
                itemDet.Preco = (item.ValorVenda ?? 0) * itemDet.Quantidade;
                await _repository.Post(itemDet);
                total = total + itemDet.Preco;
            }

            //atualiza o cabecalho
            itemCabecalho.Total = total;
            await _repositoryCabecalho.Put(itemCabecalho);
        }



        public async Task Delete(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<bool> Exists(int id)
        {
            return await _repository.Exists(id);
        }

        public async Task RemoveContex(PedidoDetalhe item)
        {
            _repository.RemoveContex(item);
        }
        public async Task Detached(PedidoDetalhe item)
        {
            _repository.Detached(item);
        }
        public async Task<int> GetLasdOrOne()
        {
            return _repository.GetLasdOrOne();
        }

    }
}

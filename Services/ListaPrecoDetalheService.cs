using ControlStoreAPI.Data.Interface;
using ControlStoreAPI.Data.Model;
using ControlStoreAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ControlStoreAPI.Services
{
    public class ListaPrecoDetalheService : IListaPrecoDetalheService
    {
        private readonly ILoggerService _loggerService;
        private readonly IRepository<ListaPrecoDetalhe> _repository;
        private readonly IRepository<Produto> _repositoryProduto;

        public ListaPrecoDetalheService(ILoggerService loggerService,
            IRepository<ListaPrecoDetalhe> repository
            , IRepository<Produto> repositoryProduto
            )
        {
            _loggerService = loggerService;
            _repository = repository;
            _repositoryProduto = repositoryProduto;
        }

        public async Task<IEnumerable<ListaPrecoDetalhe>> GetItems()
        {
            var items = await _repository.GetItems();
            return items.OrderBy(x => x.ID);
        }

        public async Task<ListaPrecoDetalhe> GetItem(int id)
        {
            var item = await _repository.GetItem(id);
            return item;
        }

        public async Task Put(ListaPrecoDetalhe item)
        {
            await _repository.Put(item);
        }

        public async Task<ListaPrecoDetalhe> Post(ListaPrecoDetalhe item)
        {
            return await _repository.Post(item);
        }

        public async Task SaveDetail(List<Produto> items
            , ListaPrecoCabecalho itemCabecalho)
        {

                var prod = items.FirstOrDefault();


                //get products by category
                var query = from detalhe in _repository.Query()
                            join produto in _repositoryProduto.Query()
                            on detalhe.ProdutoId equals produto.ID
                            where detalhe.ListaPrecoId == itemCabecalho.ID
                            select detalhe;

                //clean items
                foreach (var item in query)
                {
                    await _repository.Delete(item.ID);
                }

            //save items
            foreach (var item in items)
            {
                ListaPrecoDetalhe itemDet = new ListaPrecoDetalhe();
                itemDet.ListaPrecoId = itemCabecalho.ID;
                itemDet.ProdutoId = item.ID;
                itemDet.ID = 0;
                itemDet.EstoqueMinimo = item.EstoqueMin??0;
                itemDet.EstoqueMaximo = item.EstoqueMax ?? 0;
                itemDet.Preco = item.ValorVenda ?? 0;
                itemDet.Quantidade = item.QuantidadeEstoque ?? 0;
                await _repository.Post(itemDet);

                //save product
                var prodFromDB = await _repositoryProduto.GetItem(item.ID);
                prodFromDB.TotalizadorParcial = item.TotalizadorParcial;
                await _repositoryProduto.Put(prodFromDB);
            }

        }

        public async Task Delete(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<bool> Exists(int id)
        {
            return await _repository.Exists(id);
        }

        public async Task RemoveContex(ListaPrecoDetalhe item)
        {
            _repository.RemoveContex(item);
        }
        public async Task Detached(ListaPrecoDetalhe item)
        {
            _repository.Detached(item);
        }
        public async Task<int> GetLasdOrOne()
        {
            return _repository.GetLasdOrOne();
        }

    }
}

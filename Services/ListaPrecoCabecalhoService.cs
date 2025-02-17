using ControlStoreAPI.Data.Interface;
using ControlStoreAPI.Data.Model;
using ControlStoreAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControlStoreAPI.Services
{
    public class ListaPrecoCabecalhoService : IListaPrecoCabecalhoService
    {
        private readonly ILoggerService _loggerService;
        private readonly IRepository<ListaPrecoCabecalho> _repository;
        private readonly IRepository<ListaPrecoDetalhe> _repositoryDetalhe;
        private readonly IRepository<Produto> _repositoryProduto;

        public ListaPrecoCabecalhoService(ILoggerService loggerService,
            IRepository<ListaPrecoCabecalho> repository,
            IRepository<ListaPrecoDetalhe> repositoryDetalhe,
            IRepository<Produto> repositoryProduto
            )
        {
            _loggerService = loggerService;
            _repository = repository;
            _repositoryDetalhe = repositoryDetalhe;
            _repositoryProduto = repositoryProduto;
        }

        public async Task<IEnumerable<ListaPrecoCabecalho>> GetItems()
        {
            var items = await _repository.GetItems();
            return items.OrderBy(x => x.ID);
        }

        public async Task<ListaPrecoCabecalho> GetItem(int id)
        {
            var item = await _repository.GetItem(id);
            return item;
        }

        public async Task Put(ListaPrecoCabecalho item)
        {
            await _repository.Put(item);
        }

        public async Task<ListaPrecoCabecalho> SaveList(int clientId)
        {
            var list = await _repository.Query().Where(x=>x.ClienteId == clientId).FirstOrDefaultAsync();
            if (list!=null)
                return list;

            ListaPrecoCabecalho item = new ListaPrecoCabecalho();
            item.ClienteId = clientId;
            item.ID = 0;
            return await _repository.Post(item);
        }

        public async Task<ListaPrecoCabecalho> Post(ListaPrecoCabecalho item)
        {
            return await _repository.Post(item);
        }

        public async Task Delete(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<bool> Exists(int id)
        {
            return await _repository.Exists(id);
        }

        public async Task RemoveContex(ListaPrecoCabecalho item)
        {
            _repository.RemoveContex(item);
        }
        public async Task Detached(ListaPrecoCabecalho item)
        {
            _repository.Detached(item);
        }
        public async Task<int> GetLasdOrOne()
        {
            return _repository.GetLasdOrOne();
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaECliente(int categoriaId, int clienteId)
        {

            //obtendo o cabecalho da tabela de preço
            var cabecalho = await _repository
                .Query()
                .Where(x => x.ClienteId == clienteId)
                .FirstOrDefaultAsync();

            if (cabecalho == null)
                return Enumerable.Empty<Produto>();

            //obtendo os detalhes
            var detalhes = await _repositoryDetalhe
                .Query()
                .Where(x => x.ListaPrecoId == cabecalho.ID)
                .Select(x => new
                {
                    x.ProdutoId,
                    x.EstoqueMinimo,
                    x.EstoqueMaximo,
                    x.Quantidade
                })
                .ToListAsync();

            if (!detalhes.Any())
                return Enumerable.Empty<Produto>();

            // Filtra os produtos que pertencem à categoria
            var produtos = await _repositoryProduto
                .Query()
                 .Where(p => detalhes
                            .Select(d => d.ProdutoId)
                            .Contains(p.ID) && p.CategoriaProdutoId == categoriaId)
                .ToListAsync();

            // Combinando os produtos com os detalhes
            var produtosComDetalhes = produtos.Select(produto =>
            {
                var detalhe = detalhes.FirstOrDefault(d => d.ProdutoId == produto.ID);
                if (detalhe != null)
                {
                    produto.EstoqueMin = detalhe.EstoqueMinimo;
                    produto.EstoqueMax = detalhe.EstoqueMaximo;
                    produto.QuantidadeEstoqueAnterior = produto.QuantidadeEstoque;
                    produto.QuantidadeEstoque = detalhe.Quantidade;
                }
                return produto;
            });

            return produtosComDetalhes;            
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorCliente(int clienteId)
        {

            //obtendo o cabecalho da tabela de preço
            var cabecalho = await _repository
                .Query()
                .Where(x => x.ClienteId == clienteId)
                .FirstOrDefaultAsync();

            if (cabecalho == null)
                return Enumerable.Empty<Produto>();

            //obtendo os detalhes
            var detalhes = await _repositoryDetalhe
                .Query()
                .Where(x => x.ListaPrecoId == cabecalho.ID)
                .Select(x => new
                {
                    x.ProdutoId,
                    x.EstoqueMinimo,
                    x.EstoqueMaximo,
                    x.Quantidade,
                    x.Preco
                })
                .ToListAsync();

            if (!detalhes.Any())
                return Enumerable.Empty<Produto>();

            // Filtra os produtos que pertencem à categoria
            var produtos = await _repositoryProduto
                .Query()
                 .Where(p => detalhes.Select(d => d.ProdutoId).Contains(p.ID))
                .ToListAsync();

            // Combinando os produtos com os detalhes
            var produtosComDetalhes = produtos.Select(produto =>
            {
                var detalhe = detalhes.FirstOrDefault(d => d.ProdutoId == produto.ID);
                if (detalhe != null)
                {
                    produto.EstoqueMin = detalhe.EstoqueMinimo;
                    produto.EstoqueMax = detalhe.EstoqueMaximo;
                    produto.QuantidadeEstoqueAnterior = produto.QuantidadeEstoque;
                    produto.QuantidadeEstoque = detalhe.Quantidade;
                    produto.ValorVenda = detalhe.Preco;
                }
                return produto;
            });

            return produtosComDetalhes;
        }
    }
}

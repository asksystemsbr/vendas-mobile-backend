using ControlStoreAPI.Data.Interface;
using ControlStoreAPI.Data.Model;
using ControlStoreAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControlStoreAPI.Services
{
    public class PedidoCabecalhoService : IPedidoCabecalhoService
    {
        private readonly ILoggerService _loggerService;
        private readonly IRepository<PedidoCabecalho> _repository;
        private readonly IRepository<PedidoDetalhe> _repositoryDetalhe;
        private readonly IRepository<Produto> _repositoryProduto;

        public PedidoCabecalhoService(ILoggerService loggerService,
            IRepository<PedidoCabecalho> repository,
            IRepository<PedidoDetalhe> repositoryDetalhe,
            IRepository<Produto> repositoryProduto
            )
        {
            _loggerService = loggerService;
            _repository = repository;
            _repositoryDetalhe = repositoryDetalhe;
            _repositoryProduto = repositoryProduto;
        }

        public async Task<IEnumerable<PedidoCabecalho>> GetItems()
        {
            var items = await _repository.GetItems();
            return items.OrderBy(x => x.ID);
        }

        public async Task<PedidoCabecalho> GetItem(int id)
        {
            var item = await _repository.GetItem(id);
            return item;
        }

        public async Task Put(PedidoCabecalho item)
        {
            await _repository.Put(item);
        }

        public async Task<PedidoCabecalho> SaveOrder(int clientId)
        {
            var list = await _repository.Query().Where(x=>x.ClienteId == clientId
            && x.Status.ToUpper()=="PENDENTE").FirstOrDefaultAsync();
            if (list!=null)
                return list;

            PedidoCabecalho item = new PedidoCabecalho();
            item.ClienteId = clientId;
            item.Data = DateTime.Now;
            item.Status = "PENDENTE";
            item.ID = 0;
            return await _repository.Post(item);
        }

        public async Task<PedidoCabecalho> Post(PedidoCabecalho item)
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

        public async Task RemoveContex(PedidoCabecalho item)
        {
            _repository.RemoveContex(item);
        }
        public async Task Detached(PedidoCabecalho item)
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
                .Where(x => x.PedidoCabecalhoId == cabecalho.ID)
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
                    produto.QuantidadeEstoque = detalhe.Quantidade;
                }
                return produto;
            });

            return produtosComDetalhes;            
        }

        public async Task<IEnumerable<PedidoCabecalho>> GetPedidosByStatus(string status, int clienteId)
        {

            //obtendo o cabecalho da tabela de preço
            var cabecalhos = await _repository
                .Query()
                .Where(x => x.ClienteId == clienteId && x.Status.ToUpper()==status.ToUpper())
                .ToListAsync();

            if (cabecalhos == null)
                return Enumerable.Empty<PedidoCabecalho>();

            foreach (var cabecalho in cabecalhos)
            {
                //obtendo os detalhes
                var detalhes = await _repositoryDetalhe
                .Query()
                .Where(x => x.PedidoCabecalhoId == cabecalho.ID)                
                .ToListAsync();

                var produtoIds = detalhes.Select(d => d.ProdutoId).ToList();

                var produtos = await _repositoryProduto
                    .Query()
                    .Where(p => produtoIds.Contains(p.ID))
                    .ToListAsync();


                cabecalho.Itens = detalhes.Select(detalhe =>
                {
                    var produto = produtos.FirstOrDefault(p => p.ID == detalhe.ProdutoId);
                    return new Produto
                    {
                        ID = produto?.ID ?? 0,
                        Nome = produto?.Nome ?? string.Empty,
                        CodigoInterno = produto?.CodigoInterno,
                        ValorVenda = produto?.ValorVenda,
                        EstoqueMin = detalhe.EstoqueMinimo,
                        EstoqueMax = detalhe.EstoqueMaximo,
                        QuantidadeEstoque = detalhe.Quantidade
                    };
                }).ToList();
            }

            return cabecalhos;


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
                .Where(x => x.PedidoCabecalhoId == cabecalho.ID)
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
                    produto.QuantidadeEstoque = detalhe.Quantidade;
                }
                return produto;
            });

            return produtosComDetalhes;
        }
    }
}

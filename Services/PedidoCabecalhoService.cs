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
        private readonly IRepository<Cliente> _repositoryCliente;
        private readonly IRepository<PedidoCabecalho> _repository;
        private readonly IRepository<PedidoDetalhe> _repositoryDetalhe;
        private readonly IRepository<Produto> _repositoryProduto;
        private readonly IRepository<FilesOrder> _repositoryFilesOrder;

        public PedidoCabecalhoService(ILoggerService loggerService,
            IRepository<Cliente> repositoryCliente,
            IRepository<PedidoCabecalho> repository,
            IRepository<PedidoDetalhe> repositoryDetalhe,
            IRepository<Produto> repositoryProduto,
            IRepository<FilesOrder> repositoryFilesOrder
            )
        {
            _loggerService = loggerService;
            _repositoryCliente = repositoryCliente;
            _repository = repository;
            _repositoryDetalhe = repositoryDetalhe;
            _repositoryProduto = repositoryProduto;
            _repositoryFilesOrder = repositoryFilesOrder;
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
            && x.Status.ToUpper()=="APROVAR").FirstOrDefaultAsync();
            if (list!=null)
                return list;

            PedidoCabecalho item = new PedidoCabecalho();
            item.ClienteId = clientId;
            item.Data = DateTime.Now;
            item.Status = "APROVAR";
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
                        QuantidadeEstoque = detalhe.Quantidade,
                        TotalizadorParcial = produto?.TotalizadorParcial
                    };
                }).ToList();
            }

            return cabecalhos;


        }

        public async Task<IEnumerable<PedidoCabecalho>> GetPedidosAllUsersByStatus(string status)
        {

            //obtendo o cabecalho da tabela de preço
            var cabecalhos = await _repository
                .Query()
                .Where(x =>x.Status.ToUpper() == status.ToUpper())
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
                        QuantidadeEstoque = detalhe.Quantidade,
                        TotalizadorParcial = produto?.TotalizadorParcial
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


        public async Task SalvarArquivo(PedidoCabecalho pedido, string fileUrl, string folder)
        {
            var files = await _repositoryFilesOrder.Query()
                .Where(x =>
                          (pedido.ID > 0 ? x.PedidoId == pedido.ID : true)
                            && (pedido.ClienteId > 0 ? x.ClienteId == pedido.ClienteId : true)
                            && x.Path == fileUrl
                            && x.Type==folder).ToListAsync();

            if(!files.Any())
            {
                FilesOrder filesOrders = new FilesOrder();
                filesOrders.ID = 0;
                filesOrders.Path = fileUrl;
                filesOrders.Type = folder;
                filesOrders.PedidoId = pedido.ID;
                filesOrders.ClienteId= pedido.ClienteId;
                filesOrders.Data = DateTime.Now;
                await _repositoryFilesOrder.Post(filesOrders);
            }
            return;
        }

        public async Task<IEnumerable<FilesOrder>> GetFilesByPedido(int pedidoId,string type,string extension)
        {
            int id = pedidoId;
            bool isBoleto = false;
            if(type.ToLower().Contains("boleto"))
            {
                var cliente = await _repositoryCliente.Query()
                    .Where(x=>x.FUNCIONARIO_ID== id).FirstOrDefaultAsync(); 

                if(cliente!=null)
                    id = cliente.ID;

                type = "PDF";
                isBoleto = true;
            }
            return  await _repositoryFilesOrder.Query()
                .Where(f => 
                (isBoleto ? f.ClienteId == id && f.PedidoId==0 : f.PedidoId == id)
                && f.Type == type
                )
                .OrderByDescending(f => f.Data)
                .ToListAsync();            
        }
    }
}

using ControlStoreAPI.Data.Interface;
using ControlStoreAPI.Data.Model;
using ControlStoreAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ControlStoreAPI.Services
{
    public class ComposicaoToProdutoService : IComposicaoToProdutoService
    {
        private readonly ILoggerService _loggerService;
        private readonly IRepository<ComposicaoToProduto> _repository;

        public ComposicaoToProdutoService(ILoggerService loggerService,
            IRepository<ComposicaoToProduto> repository
            )
        {
            _loggerService = loggerService;
            _repository = repository;
        }

        public async Task<IEnumerable<ComposicaoToProduto>> GetItems()
        {
            var items = await _repository.GetItems();
            return items.OrderBy(x => x.ComposicaoId);
        }

        public async Task<ComposicaoToProduto> GetItem(int id)
        {
            var item = await _repository.GetItem(id);
            return item;
        }

        public async Task<IEnumerable<ComposicaoToProduto>> GetItemsByProduto(int produtoId)
        {
            var items = await _repository.GetItems();
            return items.Where(x=>x.ProdutoId==produtoId).OrderBy(x => x.ComposicaoId);
        }

        public async Task Put(ComposicaoToProduto item)
        {
            await _repository.Put(item);
        }

        public async Task<ComposicaoToProduto> Post(ComposicaoToProduto item)
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

        public async Task RemoveContex(ComposicaoToProduto item)
        {
            _repository.RemoveContex(item);
        }
        public async Task Detached(ComposicaoToProduto item)
        {
            _repository.Detached(item);
        }
        public async Task<int> GetLasdOrOne()
        {
            return _repository.GetLasdOrOne();
        }

    }
}

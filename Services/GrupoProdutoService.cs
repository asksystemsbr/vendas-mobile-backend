using ControlStoreAPI.Data.Interface;
using ControlStoreAPI.Data.Model;
using ControlStoreAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ControlStoreAPI.Services
{
    public class GrupoProdutoService : IGrupoProdutoService
    {
        private readonly ILoggerService _loggerService;
        private readonly IRepository<GrupoProduto> _repository;

        public GrupoProdutoService(ILoggerService loggerService,
            IRepository<GrupoProduto> repository
            )
        {
            _loggerService = loggerService;
            _repository = repository;
        }

        public async Task<IEnumerable<GrupoProduto>> GetItems()
        {
            var items = await _repository.GetItems();
            return items.OrderBy(x => x.Descricao);
        }

        public async Task<GrupoProduto> GetItem(int id)
        {
            var item = await _repository.GetItem(id);
            return item;
        }

        public async Task Put(GrupoProduto item)
        {
            await _repository.Put(item);
        }

        public async Task<GrupoProduto> Post(GrupoProduto item)
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

        public async Task RemoveContex(GrupoProduto item)
        {
            _repository.RemoveContex(item);
        }
        public async Task Detached(GrupoProduto item)
        {
            _repository.Detached(item);
        }
        public async Task<int> GetLasdOrOne()
        {
            return _repository.GetLasdOrOne();
        }

    }
}

using ControlStoreAPI.Data.Interface;
using ControlStoreAPI.Data.Model;
using ControlStoreAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ControlStoreAPI.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly ILoggerService _loggerService;
        private readonly IRepository<Produto> _repository;

        public ProdutoService(ILoggerService loggerService,
            IRepository<Produto> repository
            )
        {
            _loggerService = loggerService;
            _repository = repository;
        }

        public async Task<IEnumerable<Produto>> GetItems()
        {
            var items = await _repository.GetItems();
            return items.OrderBy(x => x.Nome);
        }

        public async Task<Produto> GetItem(int id)
        {
            var item = await _repository.GetItem(id);
            return item;
        }

        public async Task<List<Produto>> GetByGrupo(int grupoId)
        {
            var item = await _repository.Query().Where(x=>x.CategoriaProdutoId == grupoId).ToListAsync();
            return item;
        }

        public async Task Put(Produto item)
        {
            await _repository.Put(item);
        }

        public async Task<Produto> Post(Produto item)
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

        public async Task RemoveContex(Produto item)
        {
            _repository.RemoveContex(item);
        }
        public async Task Detached(Produto item)
        {
            _repository.Detached(item);
        }
        public async Task<int> GetLasdOrOne()
        {
            return _repository.GetLasdOrOne();
        }

    }
}

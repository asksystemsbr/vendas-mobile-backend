using ControlStoreAPI.Data.Interface;
using ControlStoreAPI.Data.Model;
using ControlStoreAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ControlStoreAPI.Services
{
    public class ComposicaoService : IComposicaoService
    {
        private readonly ILoggerService _loggerService;
        private readonly IRepository<Composicao> _repository;

        public ComposicaoService(ILoggerService loggerService,
            IRepository<Composicao> repository
            )
        {
            _loggerService = loggerService;
            _repository = repository;
        }

        public async Task<IEnumerable<Composicao>> GetItems()
        {
            var items = await _repository.GetItems();
            return items.OrderBy(x => x.ID);
        }

        public async Task<Composicao> GetItem(int id)
        {
            var item = await _repository.GetItem(id);
            return item;
        }

        public async Task Put(Composicao item)
        {
            await _repository.Put(item);
        }

        public async Task<Composicao> Post(Composicao item)
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

        public async Task RemoveContex(Composicao item)
        {
            _repository.RemoveContex(item);
        }
        public async Task Detached(Composicao item)
        {
            _repository.Detached(item);
        }
        public async Task<int> GetLasdOrOne()
        {
            return _repository.GetLasdOrOne();
        }

    }
}

using ControlStoreAPI.Data.Interface;
using ControlStoreAPI.Data.Model;
using ControlStoreAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ControlStoreAPI.Services
{
    public class StatusClientService : IStatusClientService
    {
        private readonly ILoggerService _loggerService;
        private readonly IRepository<StatusCliente> _repository;

        public StatusClientService(ILoggerService loggerService,
            IRepository<StatusCliente> repository
            )
        {
            _loggerService = loggerService;
            _repository = repository;
        }

        public async Task<IEnumerable<StatusCliente>> GetItems()
        {
            var items = await _repository.GetItems();
            return items.OrderBy(x => x.Descricao);
        }

        public async Task<StatusCliente> GetItem(int id)
        {
            var item = await _repository.GetItem(id);
            return item;
        }

        public async Task Put(StatusCliente item)
        {
            await _repository.Put(item);
        }

        public async Task<StatusCliente> Post(StatusCliente item)
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

        public async Task RemoveContex(StatusCliente item)
        {
            _repository.RemoveContex(item);
        }
        public async Task Detached(StatusCliente item)
        {
            _repository.Detached(item);
        }
        public async Task<int> GetLasdOrOne()
        {
            return _repository.GetLasdOrOne();
        }

    }
}

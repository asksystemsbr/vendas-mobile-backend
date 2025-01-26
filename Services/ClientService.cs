using ControlStoreAPI.Data.Interface;
using ControlStoreAPI.Data.Model;
using ControlStoreAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ControlStoreAPI.Services
{
    public class ClientService : IClientService
    {
        private readonly ILoggerService _loggerService;
        private readonly IRepository<Cliente> _repository;
        private readonly IRepository<StatusCliente> _repositorySituacaoCliente;

        public ClientService(ILoggerService loggerService,
            IRepository<Cliente> repository,
            IRepository<StatusCliente> repositorySituacaoCliente
            )
        {
            _loggerService = loggerService;
            _repository = repository;
            _repositorySituacaoCliente = repositorySituacaoCliente;
        }

        public async Task<IEnumerable<Cliente>> GetItems()
        {
            var items = await _repository.GetItems();
            return items.OrderBy(x => x.NOME);
        }

        public async Task<Cliente> GetItem(int id)
        {
            var item = await _repository.GetItem(id);
            return item;
        }

        public async Task<Cliente> GetItemByCPF(string cpf)
        {
            string noMaks = cpf.Replace(".", "").Replace("-", "");
            return await _repository.Query()
                .Where(x => x.CpfCnpj != null 
                && (x.CpfCnpj == cpf || x.CpfCnpj == noMaks))
                .FirstOrDefaultAsync();
        }

        public async Task<Cliente> GetItemByRG(string rg)
        {
            string noMaks = rg.Replace(".", "").Replace("-", "");
            return await _repository.Query()
                .Where(x => x.RG != null && (x.RG == rg || x.RG == noMaks))
                .FirstOrDefaultAsync();
        }

        public async Task<Cliente> GetItemByNome(string nome)
        {
            string noMaks = nome.Replace(".", "").Replace("-", "");
            return await _repository.Query()
                .Where(x => x.NOME != null && (x.NOME == nome || x.NOME == noMaks))
                .FirstOrDefaultAsync();
        }

        public async Task<Cliente> GetItemByTelefone(string telefone)
        {
            string noMaks = telefone.Replace(".", "")
                            .Replace("-", "")
                            .Replace("(", "")
                            .Replace(")", "")
                            .Replace(" ", "");
            return await _repository.Query()
                .Where(
                x => x.FoneOne != null
                        && (x.FoneOne == telefone
                                || x.FoneOne
                                .Replace(".", "")
                                .Replace("-", "")
                                .Replace("(", "")
                                .Replace(")", "")
                                .Replace(" ", "") == noMaks)
                || (x.CELULAR != null
                        && (x.CELULAR == telefone
                            || x.CELULAR
                                .Replace(".", "")
                                .Replace("-", "")
                                .Replace("(", "")
                                .Replace(")", "")
                                .Replace(" ", "") == noMaks))
                )
                .FirstOrDefaultAsync();
        }        
        public async Task<Cliente> GetItemByUsuario(int userId)
        {
            return await _repository.Query()
                .Where(x => x.FUNCIONARIO_ID == userId)
                .FirstOrDefaultAsync();
        }

        public async Task Put(Cliente item)
        {
            await _repository.Put(item);
        }

        public async Task<Cliente> Post(Cliente item)
        {
            if(item.ID==0)
                item.ID = await GetLasdOrOne();

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

        public async Task RemoveContex(Cliente item)
        {
            _repository.RemoveContex(item);
        }

        public async Task<int> GetLasdOrOne()
        {
            return _repository.GetLasdOrOne();
        }
        public async Task Detached(Cliente item)
        {
            _repository.Detached(item);
        }
    }
}

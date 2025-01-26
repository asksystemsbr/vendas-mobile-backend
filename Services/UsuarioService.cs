using ControlStoreAPI.Data.Interface;
using ControlStoreAPI.Data.Model;
using ControlStoreAPI.Extensions;
using ControlStoreAPI.Models;
using ControlStoreAPI.Service.Interface;
using ControlStoreAPI.Services.Interface;
using ControlStoreAPI.View.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ControlStoreAPI.Service
{
    public class UsuarioService:IUsuarioService
    {
        private readonly ILoggerService _loggerService;
        private readonly IRepository<Usuario> _repository;
        private readonly IRepository<Cliente> _repositoryCliente;
        private readonly IRepository<GrupoUsuario> _repositoryGrupoUsuario;
        private readonly IRepository<Permissao> _repositoryPermissao;
        private readonly IRepository<TipoPermissao> _repositoryTipoPermissao;
        private readonly IRepository<Modulo> _repositoryModulos;

        public UsuarioService(ILoggerService loggerService,
        IRepository<Usuario> repository,
        IRepository<Cliente> repositoryCliente,
        IRepository<GrupoUsuario> repositoryGrupoUsuario,
        IRepository<Permissao> repositoryPermissao,
        IRepository<TipoPermissao> repositoryTipoPermissao,
        IRepository<Modulo> repositoryModulos)
        {
            _loggerService = loggerService;
            _repository = repository;
            _repositoryCliente = repositoryCliente;
            _repositoryGrupoUsuario = repositoryGrupoUsuario;
            _repositoryPermissao = repositoryPermissao;
            _repositoryTipoPermissao = repositoryTipoPermissao;
            _repositoryModulos = repositoryModulos;
        }
        public async Task<LoginCredentials> Authenticate(LoginCredentials credentials)
        {
            string token = string.Empty;

            var usuario = await _repository.Query().FirstOrDefaultAsync(u => u.Login == credentials.Nome && u.Senha == credentials.Senha);

            var lstPermissoes = new List<string>();
            if (usuario == null)
            {
                throw new InvalidOperationException("Usuário ou senha inválidos.");
            }
            else
            {
                //obter as permissões do usuário
                var grupo = await _repositoryGrupoUsuario.Query().Where(x => x.ID == usuario.GrupoUsuarioId).FirstOrDefaultAsync();
                if (grupo == null)
                    throw new InvalidOperationException("Usuário sem grupo.");

                var permissoesformBD = await _repositoryPermissao.GetItems();
                var pemissoes = permissoesformBD.Where(x => x.GrupoUsuarioId == usuario.GrupoUsuarioId).ToList();
                if (pemissoes == null)
                    throw new InvalidOperationException("Permissões não encontradas.");

                var claims = new List<Claim>();

                var tiposPermissoes = await _repositoryTipoPermissao.Query().ToListAsync();
                var modulos = await _repositoryModulos.Query().ToListAsync();

                foreach (var perm in pemissoes)
                {
                    var tipoPermissao = tiposPermissoes.Where(x => x.ID == perm.TipoPermissaoId).FirstOrDefault();

                    var modulo = modulos.Where(x => x.ID == perm.ModuloId).FirstOrDefault();

                    string descricaoPermsisao = tipoPermissao != null && tipoPermissao.Descricao.ToLower() == "leitura" ? "Read" : "Write";
                    string descricaomodulo = modulo != null ? modulo.Descricao : "";

                    string permClaim = $"{descricaomodulo}.{descricaoPermsisao}"; // Formato: "Modulo.Permissao"
                    lstPermissoes.Add(permClaim);
                    claims.Add(new Claim("Permission", permClaim));
                }
                token = JWTExtensions.GenerateJwtToken(claims);
            }

            // Idealmente, você não deve retornar a senha. Considere usar um DTO para esconder a senha.
            usuario.Senha = null;
            return new LoginCredentials()
            {
                Senha = "",
                Nome = (usuario == null ? "" : usuario.Login),
                token = token,
                permissions = lstPermissoes,
                Id = (usuario == null ? "0" : usuario.ID.ToString()),
            };

        }

        public async Task<Usuario> GetItem(int id)
        {
            var item = await _repository.GetItem(id);
            return item;
        }

        public async Task<Usuario> Post(Usuario item)
        {
            return await _repository.Post(item);
        }

        public async Task Put(Usuario item)
        {
            await _repository.Put(item);
        }
    }
}

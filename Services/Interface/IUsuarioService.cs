using ControlStoreAPI.Models;
using ControlStoreAPI.View.ViewModel;
using System.Security.Claims;

namespace ControlStoreAPI.Service.Interface
{
    public interface IUsuarioService
    {
        Task<LoginCredentials> Authenticate(LoginCredentials credentials);
        Task<Usuario> GetItem(int id);
        Task<Usuario> Post(Usuario item);
        Task Put(Usuario item);
    }
}

using ControlStoreAPI.Data.Model;
using System.Security.Claims;

namespace ControlStoreAPI.Services.Interface
{
    public interface ILoggerService
    {
        Task LogAuditAsync(AuditLog log);
        Task LogOperationAsync(OperationLog log);

        Task LogError<T>(string operationType, T contextType, ClaimsPrincipal user, Exception ex);
    }
}

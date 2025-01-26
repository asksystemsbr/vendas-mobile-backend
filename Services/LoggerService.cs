using ControlStoreAPI.Data.Model;
using ControlStoreAPI.Data;
using ControlStoreAPI.Services.Interface;
using System.Security.Claims;
using System.Text;

namespace ControlStoreAPI.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly APIDbContext _context;

        public LoggerService(APIDbContext context)
        {
            _context = context;
        }

        public async Task LogAuditAsync(AuditLog log)
        {
            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task LogOperationAsync(OperationLog log)
        {
            _context.OperationLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        private async Task SaveFullLogOperationAsync(string operationType, string contextType, string errMsg)
        {
            await LogOperationAsync(new OperationLog
            {
                Timestamp = DateTime.UtcNow,
                OperationType = operationType,
                Details = $"Attempted to {operationType} a {contextType} type",
                IsSuccess = false,
                ErrorMessage = errMsg
            });
        }
        private async Task SaveFullLogAuditAsync<T>(string operationType, T contextType, ClaimsPrincipal user)
        {
            await LogAuditAsync(new AuditLog
            {
                Timestamp = DateTime.UtcNow,
                ActionType = operationType,
                TableName = typeof(T).Name,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(contextType),
                UserId = user.Identity?.Name ?? "" // Uso do operador condicional para verificar se Identity é nulo
            });
        }

        public async Task LogError<T>(string operationType, T contextType, ClaimsPrincipal user, Exception ex)
        {
            string allMessages = GetAllExceptionMessages(ex);
            await SaveFullLogOperationAsync(operationType, typeof(T).Name, allMessages);

            await SaveFullLogAuditAsync<T>(operationType, contextType, user);
        }
        private string GetAllExceptionMessages(Exception ex)
        {
            var messages = new StringBuilder();
            while (ex != null)
            {
                messages.AppendLine(ex.Message);
                ex = ex.InnerException;
            }
            return messages.ToString();
        }

        private List<Exception> GetInnerExceptions(Exception ex)
        {
            var exceptions = new List<Exception>();
            exceptions.Add(ex);

            var innerException = ex.InnerException;
            while (innerException != null)
            {
                exceptions.Add(innerException);
                innerException = innerException.InnerException;
            }

            return exceptions;
        }

    }
}

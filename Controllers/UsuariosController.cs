using ControlStoreAPI.Service.Interface;
using ControlStoreAPI.Services.Interface;
using ControlStoreAPI.View.ViewModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ControlStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ILoggerService _loggerService;
        private readonly IUsuarioService _service;
        public UsuariosController(ILoggerService loggerService, IUsuarioService service)
        {
            _loggerService = loggerService;
            _service = service;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<LoginCredentials>> Authenticate([FromBody] LoginCredentials credentials)
        {
            try
            {
                var items = await _service.Authenticate(credentials);
                return Ok(items);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(InvalidOperationException))
                {
                    return NotFound(ex.Message);
                }
                else
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }
    }
}

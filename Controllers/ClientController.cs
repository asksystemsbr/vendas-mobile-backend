using AutoMapper;
using ControlStoreAPI.Data.DTO;
using ControlStoreAPI.Data.Model;
using ControlStoreAPI.Models;
using ControlStoreAPI.Service.Interface;
using ControlStoreAPI.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControlStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ILoggerService _loggerService;
        private readonly IClientService _service;
        private readonly IUsuarioService _serviceUsuario;
        private readonly IStatusClientService _serviceStatusClient;
        private readonly IMapper _mapper;

        public ClientController(ILoggerService loggerService
            , IClientService service
            , IUsuarioService serviceUsuario
            , IStatusClientService serviceStatusClient
            , IMapper mapper)
        {
            _loggerService = loggerService;
            _service = service;
            _serviceUsuario = serviceUsuario;
            _serviceStatusClient = serviceStatusClient;
            _mapper = mapper;
        }

        // GET: api/Cliente
        [HttpGet]
        //[Authorize(Policy = "CanRead")]
        public async Task<ActionResult<IList<ClientDTO>>> GetClientes()
        {
            try
            {
                List<ClientDTO> lstReturn = null;
                var items = await _service.GetItems();
                if (items == null || !items.Any())
                {
                    return NotFound();
                }

                lstReturn = new List<ClientDTO>();
                //get usuario e senha
                foreach (var item in items)
                {
                    var clienteDto = _mapper.Map<ClientDTO>(item);
                    if (clienteDto.FUNCIONARIO_ID != null)
                    {
                        var usuario = await _serviceUsuario.GetItem(clienteDto.FUNCIONARIO_ID.Value);
                        if (usuario != null)
                        {
                            clienteDto.Usuario = usuario.Login;
                            clienteDto.Senha = usuario.Senha;
                        }
                    }
                    
                    lstReturn.Add(clienteDto);
                }

                return Ok(lstReturn);
            }
            catch (Exception ex)
            {

                await _loggerService.LogError<string>(HttpContext.Request.Method, "", User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Cliente/5
        [HttpGet("{id}")]
        //[Authorize(Policy = "CanRead")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            try
            {
                var item = await _service.GetItem(id);
                if (item == null)
                {
                    return NotFound();
                }
                return item;
            }
            catch (Exception ex)
            {

                await _loggerService.LogError<string>(HttpContext.Request.Method, "", User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("clienteByCPF/{cpf}")]
        //[Authorize(Policy = "CanRead")]
        public async Task<ActionResult<Cliente>> GetByCPF(string cpf)
        {
            var item = await _service.GetItemByCPF(cpf);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpGet("clienteByRG/{rg}")]
        //[Authorize(Policy = "CanRead")]
        public async Task<ActionResult<Cliente>> GetByRG(string rg)
        {
            var item = await _service.GetItemByRG(rg);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpGet("clienteByNome/{nome}")]
        //[Authorize(Policy = "CanRead")]
        public async Task<ActionResult<Cliente>> GetByNome(string nome)
        {
            var item = await _service.GetItemByNome(nome);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpGet("clienteByTelefone/{telefone}")]
        //[Authorize(Policy = "CanRead")]
        public async Task<ActionResult<Cliente>> GetByTelefone(string telefone)
        {
            var item = await _service.GetItemByTelefone(telefone);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpGet("existsByCPF/{cpf}")]
        //[Authorize(Policy = "CanRead")]
        public async Task<ActionResult<bool>> ExistsByCPF(string cpf)
        {
            var item = await _service.GetItemByCPF(cpf);
            if (item == null)
            {
                return Ok(false);
            }
            return Ok(true);
        }

        // PUT: api/Cliente/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        //[Authorize(Policy = "CanWrite")]
        public async Task<IActionResult> PutCliente(int id, ClientDTO clienteDto)
        {
            if (id != clienteDto.ID)
            {
                return BadRequest();
            }

            // Mapeamento ClientDTO -> Client
            var cliente = _mapper.Map<Cliente>(clienteDto);
            try
            {
                Usuario usuario = await _serviceUsuario.GetItem(clienteDto.FUNCIONARIO_ID??0);
                if (usuario != null)
                {
                    usuario.Senha = clienteDto.Senha ?? "";
                    usuario.Login = clienteDto.Usuario ?? "";
                    await _serviceUsuario.Put(usuario);
                }
                else
                {
                    usuario = new Usuario();
                    usuario.ID = 0;
                    usuario.GrupoUsuarioId = 2;
                    usuario.Ativo = true;
                    usuario.Email = clienteDto.EMAIL;
                    usuario.Nome = clienteDto.NOME ?? clienteDto.Usuario ?? "";
                    usuario.Senha = clienteDto.Senha ?? "";
                    usuario.Login = clienteDto.Usuario ?? "";
                    var usuarioCreated = await _serviceUsuario.Post(usuario);
                    if (usuarioCreated != null)
                        cliente.FUNCIONARIO_ID = usuarioCreated.ID;
                }


                await _service.Put(cliente);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await _service.RemoveContex(cliente);
                await _loggerService.LogError<Cliente>(HttpContext.Request.Method, cliente, User, ex);
                if (!await ClienteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                await _service.RemoveContex(cliente);

                await _loggerService.LogError<Cliente>(HttpContext.Request.Method, cliente, User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return NoContent();
        }

        // POST: api/Cliente
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //[Authorize(Policy = "CanWrite")]
        public async Task<ActionResult<Cliente>> PostCliente(ClientDTO clienteDto)
        {

            // Mapeamento ClientDTO -> Client
            var cliente = _mapper.Map<Cliente>(clienteDto);
            try
            {

                cliente.DATA_CADASTRO = DateTime.Now;
                
                Usuario usuario = new Usuario();
                usuario.ID = 0;
                usuario.GrupoUsuarioId = 2;
                usuario.Ativo = true;
                usuario.Email = clienteDto.EMAIL;
                usuario.Nome = clienteDto.NOME??clienteDto.Usuario??"";
                usuario.Senha = clienteDto.Senha??"";
                usuario.Login = clienteDto.Usuario ?? "";
                var usuarioCreated = await _serviceUsuario.Post(usuario);
                if(usuarioCreated != null)
                    cliente.FUNCIONARIO_ID = usuarioCreated.ID;

                var createdImovel = await _service.Post(cliente);
                
                return CreatedAtAction("GetCliente", new { id = createdImovel.ID }, createdImovel);


            }
            catch (Exception ex)
            {
                await _service.RemoveContex(cliente);

                await _loggerService.LogError<Cliente>(HttpContext.Request.Method, cliente, User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("PostMobile")]
        //[Authorize(Policy = "CanWrite")]
        public async Task<ActionResult<Cliente>> PostMobileCliente(ClientDTO clienteDto)
        {
            // Mapeamento ClientDTO -> Client
            var cliente = _mapper.Map<Cliente>(clienteDto);
            try
            {                
                var exists = await _service.Exists(clienteDto.ID);

                Cliente created = null;

                if (!exists)
                {
                    cliente.DATA_CADASTRO = DateTime.Now;
                    created = await _service.Post(cliente);
                }
                else
                {
                    await _service.Detached(cliente);
                    await _service.Put(cliente);
                    created = cliente;
                }

                return CreatedAtAction("PostMobileCliente", new { id = created.ID }, created);
            }
            catch (Exception ex)
            {
                await _service.RemoveContex(cliente);

                await _loggerService.LogError<Cliente>(HttpContext.Request.Method, cliente, User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("PostStatusMobile")]
        //[Authorize(Policy = "CanWrite")]
        public async Task<ActionResult<StatusCliente>> PostStatusMobile(StatusClienteDTO itemDTO)
        {
            // Mapeamento ClientDTO -> Client
            var item = _mapper.Map<StatusCliente>(itemDTO);
            try
            {
                var exists = await _serviceStatusClient.Exists(itemDTO.ID);

                StatusCliente created = null;

                if (!exists)
                    created = await _serviceStatusClient.Post(item);
                else
                {
                    await _serviceStatusClient.Detached(item);
                    await _serviceStatusClient.Put(item);
                    created = item;
                }

                return CreatedAtAction("PostStatusMobile", new { id = created.ID }, created);
            }
            catch (Exception ex)
            {
                await _serviceStatusClient.RemoveContex(item);

                await _loggerService.LogError<StatusCliente>(HttpContext.Request.Method, item, User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("createPortal")]
        //[Authorize(Policy = "CanWrite")]
        public async Task<ActionResult<Cliente>> PostClientePortal(ClientDTO clienteDto)
        {

            var cliente = _mapper.Map<Cliente>(clienteDto);
            try
            {

                cliente.DATA_CADASTRO = DateTime.Now;

                var createdImovel = await _service.Post(cliente);
                return CreatedAtAction("GetCliente", new { id = createdImovel.ID }, createdImovel);


            }
            catch (Exception ex)
            {
                await _service.RemoveContex(cliente);

                await _loggerService.LogError<Cliente>(HttpContext.Request.Method, cliente, User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Cliente/5
        [HttpDelete("{id}")]
        //[Authorize(Policy = "CanWrite")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var item = await _service.GetItem(id);
            if (item == null)
            {
                return NotFound();
            }

            try
            {
                await _service.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                await _service.RemoveContex(item);

                await _loggerService.LogError<Cliente>(HttpContext.Request.Method, item, User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return NoContent();
        }

        private async Task<bool> ClienteExists(int id)
        {
            return await _service.Exists(id);
        }
    }
}

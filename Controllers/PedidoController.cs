using AutoMapper;
using ControlStoreAPI.Controllers.Request;
using ControlStoreAPI.Data.DTO;
using ControlStoreAPI.Data.Model;
using ControlStoreAPI.Models;
using ControlStoreAPI.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControlStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly ILoggerService _loggerService;
        private readonly IPedidoCabecalhoService _service;
        private readonly IPedidoDetalheService _serviceDetalhe;
        private readonly IClientService _serviceClient;
        private readonly IMapper _mapper;

        public PedidoController(ILoggerService loggerService
            , IPedidoCabecalhoService service
            , IPedidoDetalheService serviceDetalhe
            , IClientService serviceClient
            , IMapper mapper)
        {
            _loggerService = loggerService;
            _service = service;
            _serviceDetalhe = serviceDetalhe;
            _serviceClient = serviceClient;
            _mapper = mapper;
        }

        // GET: api/Cliente
        [HttpGet]
        //[Authorize(Policy = "CanRead")]
        public async Task<ActionResult<IEnumerable<PedidoCabecalho>>> GetCabecalhos()
        {
            try
            {
                var items = await _service.GetItems();
                if (items == null || !items.Any())
                {
                    return NotFound();
                }
                return Ok(items);
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
        public async Task<ActionResult<PedidoCabecalho>> GetCabecalho(int id)
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

        [HttpGet("getProdutosByClienteCategoria")]
        //[Authorize(Policy = "CanRead")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutosByClienteCategoria([FromQuery] int categoriaId, [FromQuery] int usuarioId)
        {
            try
            {
                var cliente = await _serviceClient.GetItemByUsuario(usuarioId);
                var items = await _service.GetProdutosPorCategoriaECliente(categoriaId, cliente?.ID ?? 0); 
                if (items == null || !items.Any())
                {
                    return NotFound(new { Message = "Nenhum produto encontrado para os critérios fornecidos." });
                }
                return Ok(items);
            }
            catch (Exception ex)
            {

                await _loggerService.LogError<string>(HttpContext.Request.Method, "", User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetPedidosByStatus")]
        //[Authorize(Policy = "CanRead")]
        public async Task<ActionResult<IEnumerable<PedidoCabecalho>>> GetPedidosByStatus([FromQuery] int usuarioId, [FromQuery] string status )
        {
            try
            {
                var cliente = await _serviceClient.GetItemByUsuario(usuarioId);
                var items = await _service.GetPedidosByStatus(status, cliente?.ID ?? 0);
                if (items == null || !items.Any())
                {
                    return NotFound(new { Message = "Nenhum produto encontrado para os critérios fornecidos." });
                }
                return Ok(items);
            }
            catch (Exception ex)
            {

                await _loggerService.LogError<string>(HttpContext.Request.Method, "", User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getProdutosByCliente")]
        //[Authorize(Policy = "CanRead")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutosByCliente([FromQuery] int clienteId)
        {
            try
            {
                var items = await _service.GetProdutosPorCliente(clienteId);
                if (items == null || !items.Any())
                {
                    return NotFound(new { Message = "Nenhum produto encontrado para os critérios fornecidos." });
                }
                return Ok(items);
            }
            catch (Exception ex)
            {

                await _loggerService.LogError<string>(HttpContext.Request.Method, "", User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("SalvarPedido")]
        //[Authorize(Policy = "CanWrite")]
        public async Task<IActionResult> SalvarPedido([FromBody] SalvarListaRequest request)
        {
            if (request == null || request.ClienteId <= 0 || request.Items == null || !request.Items.Any())
            {
                return BadRequest("Dados inválidos. Verifique o cliente e os produtos.");
            }

            var cliente = await _serviceClient.GetItemByUsuario(request.ClienteId);

            try
            {
                var itemCabecalho = await _service.SaveOrder(cliente.ID);

                await _serviceDetalhe.ClearDetail(request.Items, itemCabecalho);
                await _serviceDetalhe.SaveDetail(request.Items, itemCabecalho);
                return Ok();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await _loggerService.LogError<SalvarListaRequest>(HttpContext.Request.Method, request, User, ex);
                    throw;
            }
            catch (Exception ex)
            {

                await _loggerService.LogError<SalvarListaRequest>(HttpContext.Request.Method, request, User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        [HttpPost("AprovarPedido")]
        //[Authorize(Policy = "CanWrite")]
        public async Task<IActionResult> AprovarPedido([FromBody] SalvarListaRequest request)
        {
            if (request == null || request.ClienteId <= 0 || request.Id<=0
                || request.Items == null || !request.Items.Any())
            {
                return BadRequest("Dados inválidos. Verifique o cliente e os produtos.");
            }

            var cliente = await _serviceClient.GetItemByUsuario(request.ClienteId);

            try
            {
                var itemCabecalho = await _service.GetItem(request.Id);
                itemCabecalho.Status = "ABERTO";
                await _service.Put(itemCabecalho);

                await _serviceDetalhe.ClearDetailDeep(request.Items, itemCabecalho);
                await _serviceDetalhe.SaveDetail(request.Items, itemCabecalho);
                await _serviceDetalhe.DebitStock(request.Items, itemCabecalho);
                return Ok();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await _loggerService.LogError<SalvarListaRequest>(HttpContext.Request.Method, request, User, ex);
                throw;
            }
            catch (Exception ex)
            {

                await _loggerService.LogError<SalvarListaRequest>(HttpContext.Request.Method, request, User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        [HttpPut("{id}")]
        //[Authorize(Policy = "CanWrite")]
        public async Task<IActionResult> PutCabecalho(int id, PedidoCabecalhoDTO itemDTO)
        {
            if (id != itemDTO.ID)
            {
                return BadRequest();
            }

            // Mapeamento ClientDTO -> Client
            var item = _mapper.Map<PedidoCabecalho>(itemDTO);
            try
            {
                await _service.Put(item);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await _service.RemoveContex(item);
                await _loggerService.LogError<PedidoCabecalho>(HttpContext.Request.Method, item, User, ex);
                if (!await ProdutoExists(id))
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
                await _service.RemoveContex(item);

                await _loggerService.LogError<PedidoCabecalho>(HttpContext.Request.Method, item, User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return NoContent();
        }

        // POST: api/Cliente
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //[Authorize(Policy = "CanWrite")]
        public async Task<ActionResult<PedidoCabecalho>> PostProduto(PedidoCabecalhoDTO itemDTO)
        {
            var item = _mapper.Map<PedidoCabecalho>(itemDTO);
            try
            {
                var created = await _service.Post(item);
                return CreatedAtAction("PostProduto", new { id = created.ID }, created);


            }
            catch (Exception ex)
            {
                await _service.RemoveContex(item);

                await _loggerService.LogError<PedidoCabecalho>(HttpContext.Request.Method, item, User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }        

        // DELETE: api/Cliente/5
        [HttpDelete("{id}")]
        //[Authorize(Policy = "CanWrite")]
        public async Task<IActionResult> DeleteProduto(int id)
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

                await _loggerService.LogError<PedidoCabecalho>(HttpContext.Request.Method, item, User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return NoContent();
        }

        private async Task<bool> ProdutoExists(int id)
        {
            return await _service.Exists(id);
        }
    }
}

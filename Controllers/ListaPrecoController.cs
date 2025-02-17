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
    //[EnableCors("AllowAll")]
    [ApiController]
    public class ListaPrecoController : ControllerBase
    {
        private readonly ILoggerService _loggerService;
        private readonly IListaPrecoCabecalhoService _service;
        private readonly IListaPrecoDetalheService _serviceDetalhe;
        private readonly IClientService _serviceClient;
        private readonly IMapper _mapper;

        public ListaPrecoController(ILoggerService loggerService
            , IListaPrecoCabecalhoService service
            , IListaPrecoDetalheService serviceDetalhe
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
        public async Task<ActionResult<IEnumerable<ListaPrecoCabecalho>>> GetListasCabecalho()
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
        public async Task<ActionResult<ListaPrecoCabecalho>> GetListaCabecalho(int id)
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

        [HttpGet("getProdutosByUser")]
        //[Authorize(Policy = "CanRead")]
        public async Task<ActionResult<IEnumerable<Produto>>> getProdutosByUser([FromQuery] int usuarioId)
        {
            try
            {
                var cliente = await _serviceClient.GetItemByUsuario(usuarioId);
                var items = await _service.GetProdutosPorCliente(cliente.ID);
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

        [HttpPost("SalvarLista")]
        //[Authorize(Policy = "CanWrite")]
        public async Task<IActionResult> SalvarLista([FromBody] SalvarListaRequest request)
        {
            if (request == null || request.ClienteId <= 0 || request.Items == null || !request.Items.Any())
            {
                return BadRequest("Dados inválidos. Verifique o cliente e os produtos.");
            }


            try
            {
                var itemCabecalho = await _service.SaveList(request.ClienteId);

                await _serviceDetalhe.SaveDetail(request.Items
                    , itemCabecalho);
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
        public async Task<IActionResult> PutCabecalho(int id, ListaPrecoCabecalhoDTO itemDTO)
        {
            if (id != itemDTO.ID)
            {
                return BadRequest();
            }

            // Mapeamento ClientDTO -> Client
            var item = _mapper.Map<ListaPrecoCabecalho>(itemDTO);
            try
            {
                await _service.Put(item);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await _service.RemoveContex(item);
                await _loggerService.LogError<ListaPrecoCabecalho>(HttpContext.Request.Method, item, User, ex);
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

                await _loggerService.LogError<ListaPrecoCabecalho>(HttpContext.Request.Method, item, User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return NoContent();
        }

        // POST: api/Cliente
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //[Authorize(Policy = "CanWrite")]
        public async Task<ActionResult<ListaPrecoCabecalho>> PostProduto(ListaPrecoCabecalho itemDTO)
        {
            var item = _mapper.Map<ListaPrecoCabecalho>(itemDTO);
            try
            {
                var created = await _service.Post(item);
                return CreatedAtAction("PostProduto", new { id = created.ID }, created);


            }
            catch (Exception ex)
            {
                await _service.RemoveContex(item);

                await _loggerService.LogError<ListaPrecoCabecalho>(HttpContext.Request.Method, item, User, ex);
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

                await _loggerService.LogError<ListaPrecoCabecalho>(HttpContext.Request.Method, item, User, ex);
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

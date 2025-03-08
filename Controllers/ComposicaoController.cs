using AutoMapper;
using ControlStoreAPI.Data.DTO;
using ControlStoreAPI.Data.Model;
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
    public class ComposicaoController : ControllerBase
    {
        private readonly ILoggerService _loggerService;
        private readonly IComposicaoService _service;
        private readonly IComposicaoToProdutoService _serviceComposicaoToProduto;
        private readonly IMapper _mapper;

        public ComposicaoController(ILoggerService loggerService
            , IComposicaoService service
            , IComposicaoToProdutoService serviceComposicaoToProduto
            , IMapper mapper)
        {
            _loggerService = loggerService;
            _service = service;
            _serviceComposicaoToProduto = serviceComposicaoToProduto;
            _mapper = mapper;
        }

        // GET: api/Cliente
        [HttpGet]
        //[Authorize(Policy = "CanRead")]
        public async Task<ActionResult<IEnumerable<Composicao>>> GetComposicoes()
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
        public async Task<ActionResult<Composicao>> GetComposicao(int id)
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

        // PUT: api/Cliente/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        //[Authorize(Policy = "CanWrite")]
        public async Task<IActionResult> PutComposicao(int id, ComposicaoDTO itemDTO)
        {
            if (id != itemDTO.ID)
            {
                return BadRequest();
            }

            // Mapeamento ClientDTO -> Client
            var item = _mapper.Map<Composicao>(itemDTO);
            try
            {
                await _service.Put(item);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await _service.RemoveContex(item);
                await _loggerService.LogError<Composicao>(HttpContext.Request.Method, item, User, ex);
                if (!await ComposicaoExists(id))
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

                await _loggerService.LogError<Composicao>(HttpContext.Request.Method, item, User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return NoContent();
        }

        // POST: api/Cliente
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //[Authorize(Policy = "CanWrite")]
        public async Task<ActionResult<Composicao>> PostComposicao(ComposicaoDTO itemDTO)
        {
            var item = _mapper.Map<Composicao>(itemDTO);
            try
            {
                var created = await _service.Post(item);
                return CreatedAtAction("PostComposicao", new { id = created.ID }, created);


            }
            catch (Exception ex)
            {
                await _service.RemoveContex(item);

                await _loggerService.LogError<Composicao>(HttpContext.Request.Method, item, User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("ClearMobile")]
        //[Authorize(Policy = "CanWrite")]
        public async Task<ActionResult<ComposicaoToProduto>> ClearMobile(List<ComposicaoToProdutoDTO> itemsDTO)
        {
            var items = _mapper.Map<List<ComposicaoToProduto>>(itemsDTO);
            ComposicaoToProduto item = null;

            try
            {
                var itemsFromBD = await _serviceComposicaoToProduto.GetItemsByProduto(items[0].ProdutoId);
                foreach (var itemFromDB in itemsFromBD)
                {
                    item = itemFromDB;
                    await _service.Delete(item.ComposicaoId);
                    await _serviceComposicaoToProduto.Delete(item.ID);
                }


                return CreatedAtAction("ClearMobile", new { id = item.ID }, item);
            }
            catch (Exception ex)
            {
                if(item!=null)
                    await _serviceComposicaoToProduto.RemoveContex(item);

                await _loggerService.LogError<ComposicaoToProduto>(HttpContext.Request.Method, item, User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("PostMobile")]
        //[Authorize(Policy = "CanWrite")]
        public async Task<ActionResult<Composicao>> PostComposicaoMobile(List<ComposicaoDTO> itemsDTO)
        {
            var items = _mapper.Map<List<Composicao>>(itemsDTO);
            Composicao item = null;
            try
            {

                foreach (var itemCurrent in items)
                {
                    item = itemCurrent;
                    var exists = await _service.Exists(item.ID);

                    Composicao created = null;

                    if (!exists)
                        created = await _service.Post(item);
                }

                 

                return CreatedAtAction("PostComposicaoMobile", new { id = item.ID }, item);
            }
            catch (Exception ex)
            {
                if(item!=null)
                    await _service.RemoveContex(item);

                await _loggerService.LogError<Composicao>(HttpContext.Request.Method, item, User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("PostComposicaoItensMobile")]
        //[Authorize(Policy = "CanWrite")]
        public async Task<ActionResult<ComposicaoToProduto>> PostComposicaoItemMobile(List<ComposicaoToProdutoDTO> itensDTO)
        {


            ComposicaoToProduto itemNow = null;
            try
            {               
                //limpando a lista
                foreach (var itemListaDTO in itensDTO)
                {
                    var item = _mapper.Map<ComposicaoToProduto>(itemListaDTO);

                    itemNow = item;
                    var exists = await _serviceComposicaoToProduto.Exists(itemListaDTO.ID);

                    ComposicaoToProduto created = null;

                    if (!exists)
                        created = await _serviceComposicaoToProduto.Post(item);
                    else
                    {
                        await _serviceComposicaoToProduto.Delete(item.ID);
                        created = await _serviceComposicaoToProduto.Post(item);
                    }
                }

                return CreatedAtAction("PostComposicaoItemMobile", new { id = itemNow.ID }, itemNow);
            }
            catch (Exception ex)
            {
                await _serviceComposicaoToProduto.RemoveContex(itemNow);

                await _loggerService.LogError<ComposicaoToProduto>(HttpContext.Request.Method, itemNow, User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Cliente/5
        [HttpDelete("{id}")]
        //[Authorize(Policy = "CanWrite")]
        public async Task<IActionResult> DeleteComposicao(int id)
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

                await _loggerService.LogError<Composicao>(HttpContext.Request.Method, item, User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return NoContent();
        }

        private async Task<bool> ComposicaoExists(int id)
        {
            return await _service.Exists(id);
        }
    }
}

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
    public class ProdutoController : ControllerBase
    {
        private readonly ILoggerService _loggerService;
        private readonly IProdutoService _service;
        private readonly IGrupoProdutoService _serviceGrupo;
        private readonly IMapper _mapper;

        public ProdutoController(ILoggerService loggerService
            , IProdutoService service
            , IGrupoProdutoService serviceGrupo
            , IMapper mapper)
        {
            _loggerService = loggerService;
            _service = service;
            _serviceGrupo = serviceGrupo;
            _mapper = mapper;
        }

        // GET: api/Cliente
        [HttpGet]
        //[Authorize(Policy = "CanRead")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
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

        [HttpGet("getGrupos")]
        //[Authorize(Policy = "CanRead")]
        public async Task<ActionResult<IEnumerable<GrupoProduto>>> GetGrupos()
        {
            try
            {
                var items = await _serviceGrupo.GetItems();
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

        [HttpGet("getGrupo/{grupoId}")]
        //[Authorize(Policy = "CanRead")]
        public async Task<ActionResult<IEnumerable<GrupoProduto>>> GetGrupo(int grupoId)
        {
            try
            {
                var items = await _serviceGrupo.GetItem(grupoId);
                if (items == null )
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

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { Message = "Nenhum arquivo enviado." });
            }

            try
            {
                // Caminho onde o arquivo será salvo (exemplo simples, mas prefira usar serviços externos)
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                // Certifique-se de que o diretório existe
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }


                // Nome único para o arquivo
                //var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsPath, file.FileName);


                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Salvar o arquivo
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Retornar a URL da imagem
                var imageUrl = $"{Request.Scheme}://{Request.Host}/uploads/{file.FileName}";

                // Atualiza o produto com a nova URL da imagem no banco de dados
                int produtoId = Convert.ToInt32(file.FileName.Substring(0, file.FileName.Length - 4));
                var produto = await _service.GetItem(produtoId);
                if (produto != null)
                {
                    produto.Foto = imageUrl; // Certifique-se de que a entidade tem esse campo
                    await _service.Put(produto);
                }

                return Ok(new { ImageUrl = imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Erro ao fazer upload: {ex.Message}" });
            }
        }

        // GET: api/Cliente/5
        [HttpGet("{id}")]
        //[Authorize(Policy = "CanRead")]
        public async Task<ActionResult<Produto>> GetProduto(int id)
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

        [HttpGet("getByGrupo/{id}")]
        //[Authorize(Policy = "CanRead")]
        public async Task<ActionResult<List<Produto>>> GetByGrupo(int id)
        {
            try
            {
                var item = await _service.GetByGrupo(id);
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
        public async Task<IActionResult> PutProduto(int id, ProdutoDTO produtoDTO)
        {
            if (id != produtoDTO.ID)
            {
                return BadRequest();
            }

            // Mapeamento ClientDTO -> Client
            var produto = _mapper.Map<Produto>(produtoDTO);
            try
            {
                await _service.Put(produto);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await _service.RemoveContex(produto);
                await _loggerService.LogError<Produto>(HttpContext.Request.Method, produto, User, ex);
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
                await _service.RemoveContex(produto);

                await _loggerService.LogError<Produto>(HttpContext.Request.Method, produto, User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return NoContent();
        }

        // POST: api/Cliente
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //[Authorize(Policy = "CanWrite")]
        public async Task<ActionResult<Produto>> PostProduto(ProdutoDTO produtoDTO)
        {
            var produto = _mapper.Map<Produto>(produtoDTO);
            try
            {
                var created = await _service.Post(produto);
                return CreatedAtAction("PostProduto", new { id = created.ID }, created);


            }
            catch (Exception ex)
            {
                await _service.RemoveContex(produto);

                await _loggerService.LogError<Produto>(HttpContext.Request.Method, produto, User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("PostMobile")]
        //[Authorize(Policy = "CanWrite")]
        public async Task<ActionResult<Produto>> PostProdutoMobile(ProdutoDTO produtoDTO)
        {
            var produto = _mapper.Map<Produto>(produtoDTO);
            try
            {
                var exists = await _service.Exists(produtoDTO.ID);                

                Produto created = null;

                if(!exists)
                    created = await _service.Post(produto);
                else
                {
                    await _service.Detached(produto);
                    await _service.Put(produto);
                    created = produto;
                }                    

                return CreatedAtAction("PostProdutoMobile", new { id = created.ID }, created);
            }
            catch (Exception ex)
            {
                await _service.RemoveContex(produto);

                await _loggerService.LogError<Produto>(HttpContext.Request.Method, produto, User, ex);
                // Retorna uma resposta de erro com o código 500 e a mensagem de exceção
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("PostGrupoMobile")]
        //[Authorize(Policy = "CanWrite")]
        public async Task<ActionResult<GrupoProduto>> PostGrupoMobile(GrupoProdutoDTO itemDTO)
        {
            var item = _mapper.Map<GrupoProduto>(itemDTO);
            try
            {
                var exists = await _serviceGrupo.Exists(itemDTO.ID);

                GrupoProduto created = null;

                if (!exists)
                    created = await _serviceGrupo.Post(item);
                else
                {
                    await _serviceGrupo.Detached(item);
                    await _serviceGrupo.Put(item);
                    created = item;
                }

                return CreatedAtAction("PostGrupoMobile", new { id = created.ID }, created);
            }
            catch (Exception ex)
            {
                await _serviceGrupo.RemoveContex(item);

                await _loggerService.LogError<GrupoProduto>(HttpContext.Request.Method, item, User, ex);
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

                await _loggerService.LogError<Produto>(HttpContext.Request.Method, item, User, ex);
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

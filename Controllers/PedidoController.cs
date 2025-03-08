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
    [EnableCors("AllowAll")]
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

        [HttpGet("GetPedidosAllUsersByStatus")]
        //[Authorize(Policy = "CanRead")]
        public async Task<ActionResult<IEnumerable<PedidoCabecalho>>> GetPedidosAllUsersByStatus([FromQuery] string status)
        {
            try
            {
                var items = await _service.GetPedidosAllUsersByStatus(status);
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
                await _serviceDetalhe.SaveDetail(request.Items, itemCabecalho,"APROVAR");
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

        [HttpPost("AtualizarPedido")]
        //[Authorize(Policy = "CanWrite")]
        public async Task<IActionResult> AtualizarPedido([FromBody] SalvarPedidoRequest request)
        {
            if (request == null || request.ClienteId <= 0 || request.Items == null || !request.Items.Any())
            {
                return BadRequest("Dados inválidos. Verifique o cliente e os produtos.");
            }


            try
            {
                var pedido = await _service.GetItem(request.Id);
                if (pedido == null)
                    return BadRequest("Pedido não encontrado");

                pedido.Status = request.Status;

                 await _service.Put(pedido);
                var detalhes = await  _serviceDetalhe.GetItemsByCabecalho(request.Id);
                foreach (var item in detalhes)
                {
                    bool exist = request.Items.Any(x => x.ID == item.ProdutoId);
                    if (exist)
                    {
                        item.Status = request.Status;
                    }
                    else
                    {
                        item.Status = "PENDENTE";
                    }
                    await _serviceDetalhe.Put(item);
                }
                return Ok();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await _loggerService.LogError<SalvarPedidoRequest>(HttpContext.Request.Method, request, User, ex);
                    throw;
            }
            catch (Exception ex)
            {

                await _loggerService.LogError<SalvarPedidoRequest>(HttpContext.Request.Method, request, User, ex);
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
                await _serviceDetalhe.SaveDetail(request.Items, itemCabecalho, "ABERTO");
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

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file, [FromQuery] int pedidoId)
        {
            bool boleto = false;

            if (file == null || file.Length == 0)
            {
                return BadRequest(new { Message = "Nenhum arquivo enviado." });
            }

            // Verifica a extensão do arquivo
            var permittedExtensions = new[] { ".pdf", ".xml" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            

            if (!permittedExtensions.Contains(fileExtension))
            {
                return BadRequest(new { Message = "Apenas arquivos PDF ou XML são permitidos." });
            }

            var folder = fileExtension.Contains("pdf") ? "PDF" : "XML";

            try
            {
                // Define o caminho para salvar o arquivo
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads","pedidos", folder);

                // Garante que o diretório existe
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                // Gera um nome único para o arquivo
                //var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var filename = file.FileName;
                if (filename.ToLower().Contains("boleto"))
                {
                    boleto = true;
                    filename = filename.Replace("boleto_", "");
                }

                var filePath = Path.Combine(uploadsPath, filename);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Salva o arquivo
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Retorna a URL do arquivo
                var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/pedidos/{folder}/{filename}";

                // Atualiza o pedido com a URL do arquivo no banco de dados
                PedidoCabecalho pedido = null;
                if (!boleto)
                {
                    pedido = await _service.GetItem(pedidoId);
                }
                else
                {
                    pedido = new PedidoCabecalho
                    {
                        ClienteId = pedidoId,
                        ID=0
                    };

                }

                if (pedido != null)
                {
                    await _service.SalvarArquivo(pedido, fileUrl, folder);
                }

                return Ok(new { FileUrl = fileUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Erro ao fazer upload: {ex.Message}" });
            }
        }

        [HttpGet("GetFilesByPedido")]
        public async Task<IActionResult> GetFilesByPedido([FromQuery] int pedidoId,[FromQuery] string type, [FromQuery] string extension)
        {
            var files =await _service.GetFilesByPedido(pedidoId,type, extension);

            if (files == null || !files.Any())
            {
                return NotFound(new { Message = "Nenhum arquivo encontrado para este pedido." });
            }

            return Ok(files);
        }

        [HttpGet("DownloadFile")]
        public IActionResult DownloadFile([FromQuery] string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return BadRequest(new { Message = "Parâmetro filePath inválido." });
            }

            if (Uri.TryCreate(filePath, UriKind.Absolute, out Uri uri))
            {
                filePath = Uri.UnescapeDataString(uri.AbsolutePath.TrimStart('/')); // Remove a barra inicial
            }

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath);

            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound(new { Message = "Arquivo não encontrado." });
            }

            var fileBytes = System.IO.File.ReadAllBytes(fullPath);
            var fileName = Path.GetFileName(fullPath);
            var contentType = "application/octet-stream"; // Ajustar conforme necessário

            return File(fileBytes, contentType, fileName);
        }
    }
}

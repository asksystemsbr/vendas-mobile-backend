using ControlStoreAPI.Data.Model;

namespace ControlStoreAPI.Controllers.Request
{
    public class SalvarPedidoRequest
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Status { get; set; }
        public List<PedidoDetalheDTO> Items { get; set; } = new();
    }
}

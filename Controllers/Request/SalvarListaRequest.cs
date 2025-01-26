using ControlStoreAPI.Data.Model;

namespace ControlStoreAPI.Controllers.Request
{
    public class SalvarListaRequest
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public List<Produto> Items { get; set; } = new();
    }
}

using ControlStoreAPI.Data.Interface;

namespace ControlStoreAPI.Data.Model
{

    public class PedidoCabecalhoDTO
    {
        public int ID { get; set; }
        public DateTime Data { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public int ClienteId { get; set; }
    }
}

using ControlStoreAPI.Data.Interface;

namespace ControlStoreAPI.Data.Model
{

    public class PedidoDetalheDTO
    {
        public int ID { get; set; }
        public int PedidoCabecalhoId { get; set; }
        public int ProdutoId { get; set; }
        public decimal EstoqueMinimo { get; set; }
        public decimal EstoqueMaximo { get; set; }
        public decimal Preco { get; set; }
        public decimal Quantidade { get; set; }
        public string Status { get; set; }
    }
}

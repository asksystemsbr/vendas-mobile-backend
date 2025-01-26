using ControlStoreAPI.Data.Interface;

namespace ControlStoreAPI.Data.Model
{

    public class ComposicaoDTO
    {
        public int ID { get; set; }

        public double Quantidade { get; set; }
        public int ProdutoId { get; set; }
    }
}

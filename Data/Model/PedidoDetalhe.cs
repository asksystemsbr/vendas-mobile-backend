using ControlStoreAPI.Data.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ControlStoreAPI.Data.Model
{
    [Table("pedido_detalhe")]
    public class PedidoDetalhe : IIdentifiable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int ID { get; set; }

        [Column("pedido_cabecalho_id")]
        public int PedidoCabecalhoId { get; set; }

        [Column("produto_id")]
        public int ProdutoId { get; set; }

        [Column("estoque_min")]
        public decimal EstoqueMinimo { get; set; }

        [Column("estoque_max")]
        public decimal EstoqueMaximo { get; set; }

        [Column("preco")]
        public decimal Preco { get; set; }

        [Column("quantidade")]
        public decimal Quantidade { get; set; }


    }
}

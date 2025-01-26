using ControlStoreAPI.Data.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ControlStoreAPI.Data.Model
{
    [Table("pedido_cabecalho")]
    public class PedidoCabecalho : IIdentifiable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int ID { get; set; }

        [Column("data")]
        public DateTime Data { get; set; }

        [Column("total")]
        public decimal Total { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("cliente_id")]
        public int ClienteId { get; set; }

        [NotMapped]
        public List<Produto> Itens { get; set; } = new List<Produto>();
    }
}

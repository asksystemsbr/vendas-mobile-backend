using ControlStoreAPI.Data.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ControlStoreAPI.Data.Model
{
    [Table("composicao")]
    public class Composicao : IIdentifiable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int ID { get; set; }

        [Column("QUANTIDADE")]
        public double Quantidade { get; set; }

        [Column("PRODUTO_id")]
        public int ProdutoId { get; set; }
    }
}

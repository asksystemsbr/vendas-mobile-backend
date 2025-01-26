using ControlStoreAPI.Data.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ControlStoreAPI.Data.Model
{
    [Table("composicaotoproduto")]
    public class ComposicaoToProduto:IIdentifiable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int ID { get; set; }

        [Column("Composicao_id")]
        public int ComposicaoId { get; set; }

        [Column("Produto_id")]
        public int ProdutoId { get; set; }
    }
}

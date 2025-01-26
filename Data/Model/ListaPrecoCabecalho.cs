using ControlStoreAPI.Data.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ControlStoreAPI.Data.Model
{
    [Table("lista_preco_cabecalho")]
    public class ListaPrecoCabecalho : IIdentifiable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int ID { get; set; }

        [Column("cliente_id")]
        public int ClienteId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ControlStoreAPI.Data.Interface;

namespace ControlStoreAPI.Data.Model
{
    [Table("grupoproduto")]
    public class GrupoProduto:IIdentifiable
    {
        [Key]
        [Column("Id")]
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Descricao { get; set; } = string.Empty;
    }
}

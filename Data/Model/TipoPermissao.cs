using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ControlStoreAPI.Data.Interface;

namespace ControlStoreAPI.Models
{
    [Table("tipo_permissao")]
    public class TipoPermissao : IIdentifiable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int ID { get; set; }

        [StringLength(255)]
        [Column("descricao")]
        public string? Descricao { get; set; }
    }
}
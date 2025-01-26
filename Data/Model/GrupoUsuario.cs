using ControlStoreAPI.Data.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlStoreAPI.Models
{
    [Table("grupo_usuario")]
    public class GrupoUsuario : IIdentifiable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int ID { get; set; }

        [StringLength(255)]
        [Column("descricao")]
        public string? Descricao { get; set; }  // Campo opcional
    }
}
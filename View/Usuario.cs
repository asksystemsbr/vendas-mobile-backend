using ControlStoreAPI.Data.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlStoreAPI.Models
{
    [Table("usuarios")]
    public class Usuario : IIdentifiable
    {
        [Key]
        [Column("usuario_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Column("nome")]
        [MaxLength(255)]
        public string Nome { get; set; }

        [Required]
        [Column("login")]
        [MaxLength(100)]
        public string Login { get; set; }

        [Required]
        [Column("senha")]
        [MaxLength(255)]
        public string Senha { get; set; }

        [Column("email")]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Column("ativo")]
        public bool? Ativo { get; set; } 

        [Column("GrupoUsuario_Id")]
        public int? GrupoUsuarioId { get; set; }
    }
}
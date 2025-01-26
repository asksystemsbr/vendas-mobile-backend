using ControlStoreAPI.Data.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlStoreAPI.Models
{
    [Table("permissoes")]
    public class Permissao : IIdentifiable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int ID { get; set; }

        [Column("tipo_permissao_id")]
        public int? TipoPermissaoId { get; set; }

        // Propriedades de navegação

        [ForeignKey("TipoPermissaoId")]
        public virtual TipoPermissao TipoPermissao { get; set; }  

        [Column("modulo_id")]
        public int? ModuloId { get; set; }

        [ForeignKey("ModuloId")]
        public virtual Modulo Modulo { get; set; }  

        [Column("grupo_usuario_id")]
        public int? GrupoUsuarioId { get; set; }

        [ForeignKey("GrupoUsuarioId")]
        public virtual GrupoUsuario GrupoUsuario { get; set; } 
    }
}
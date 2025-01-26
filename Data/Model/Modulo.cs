using ControlStoreAPI.Data.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlStoreAPI.Models
{
    [Table("modulo")]
    public class Modulo : IIdentifiable
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
using ControlStoreAPI.Data.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlStoreAPI.Data.Model
{
    [Table("files_order")]
    public class FilesOrder : IIdentifiable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int ID { get; set; }

        [Column("path")]
        public string Path { get; set; }

        [Column("pedido_id")]
        public int PedidoId { get; set; }

        [Column("cliente_id")]
        public int ClienteId { get; set; }

        [Column("datahora")]
        public DateTime Data { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("source")]
        public string Source { get; set; }

    }
}

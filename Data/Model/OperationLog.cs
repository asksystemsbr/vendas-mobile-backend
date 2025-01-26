using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ControlStoreAPI.Data.Model
{
    [Table("operationlog")]
    public class OperationLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string OperationType { get; set; }
        public string Details { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}

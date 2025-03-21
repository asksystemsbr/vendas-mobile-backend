using Microsoft.AspNetCore.Mvc;

namespace ControlStoreAPI.Data.DTO
{
    public class ParametersRequestDTO
    {
        public int ID { get; set; }
        public string Descricao { get; set; }

        [FromForm]
        public IFormFile file { get; set; }
    }
}

using AutoMapper;
using ControlStoreAPI.Data.DTO;
using ControlStoreAPI.Data.Model;

namespace ControlStoreAPI.Data.Mappers
{
    public class PedidoCabecalhoMappingProfile : Profile
    {
        public PedidoCabecalhoMappingProfile()
        {
            CreateMap<PedidoCabecalho, PedidoCabecalhoDTO>().ReverseMap();
        }
    }
}

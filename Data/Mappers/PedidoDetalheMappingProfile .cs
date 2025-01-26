using AutoMapper;
using ControlStoreAPI.Data.DTO;
using ControlStoreAPI.Data.Model;

namespace ControlStoreAPI.Data.Mappers
{
    public class PedidoDetalheMappingProfile : Profile
    {
        public PedidoDetalheMappingProfile()
        {
            CreateMap<PedidoDetalhe, PedidoDetalheDTO>().ReverseMap();
        }
    }
}

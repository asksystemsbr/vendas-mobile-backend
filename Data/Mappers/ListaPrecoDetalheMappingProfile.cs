using AutoMapper;
using ControlStoreAPI.Data.DTO;
using ControlStoreAPI.Data.Model;

namespace ControlStoreAPI.Data.Mappers
{
    public class ListaPrecoDetalheMappingProfile : Profile
    {
        public ListaPrecoDetalheMappingProfile()
        {
            CreateMap<ListaPrecoDetalhe, ListaPrecoDetalheDTO>().ReverseMap();
        }
    }
}

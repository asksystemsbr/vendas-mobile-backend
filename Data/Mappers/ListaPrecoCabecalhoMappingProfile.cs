using AutoMapper;
using ControlStoreAPI.Data.DTO;
using ControlStoreAPI.Data.Model;

namespace ControlStoreAPI.Data.Mappers
{
    public class ListaPrecoCabecalhoMappingProfile : Profile
    {
        public ListaPrecoCabecalhoMappingProfile()
        {
            CreateMap<ListaPrecoCabecalho, ListaPrecoCabecalhoDTO>().ReverseMap();
        }
    }
}

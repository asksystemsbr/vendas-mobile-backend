using AutoMapper;
using ControlStoreAPI.Data.DTO;
using ControlStoreAPI.Data.Model;

namespace ControlStoreAPI.Data.Mappers
{
    public class ComposicaoMappingProfile : Profile
    {
        public ComposicaoMappingProfile()
        {
            CreateMap<Composicao, ComposicaoDTO>().ReverseMap();
        }
    }
}

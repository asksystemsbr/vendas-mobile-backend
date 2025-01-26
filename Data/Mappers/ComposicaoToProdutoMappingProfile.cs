using AutoMapper;
using ControlStoreAPI.Data.DTO;
using ControlStoreAPI.Data.Model;

namespace ControlStoreAPI.Data.Mappers
{
    public class ComposicaoToProdutoMappingProfile : Profile
    {
        public ComposicaoToProdutoMappingProfile()
        {
            CreateMap<ComposicaoToProduto, ComposicaoToProdutoDTO>().ReverseMap();
        }
    }
}

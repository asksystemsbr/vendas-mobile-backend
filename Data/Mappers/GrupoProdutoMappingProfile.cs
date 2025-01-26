using AutoMapper;
using ControlStoreAPI.Data.DTO;
using ControlStoreAPI.Data.Model;

namespace ControlStoreAPI.Data.Mappers
{
    public class GrupoProdutoMappingProfile : Profile
    {
        public GrupoProdutoMappingProfile()
        {
            CreateMap<GrupoProduto, GrupoProdutoDTO>().ReverseMap();
        }
    }
}

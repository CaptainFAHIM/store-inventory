using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;

namespace BLL
{
    public class MapperConfig
    {
        public static MapperConfiguration config = new MapperConfiguration(cfg => {
            cfg.CreateMap<Category, CategoryDTO>().ReverseMap();
            cfg.CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CidNavigation != null ? src.CidNavigation.Name : null))
                .ReverseMap();

        });

        public static Mapper GetMapper() {
            return new Mapper(config);
        }
    }
}

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

            cfg.CreateMap<PurchaseOrder, PurchaseOrderDTO>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Items.Select(x => x.ProductId ?? 0).FirstOrDefault()))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Items.Select(x => x.Product != null ? x.Product.Name : x.ProductName).FirstOrDefault()))
                .ForMember(dest => dest.Qty, opt => opt.MapFrom(src => src.Items.Select(x => x.Qty).FirstOrDefault()))
                .ForMember(dest => dest.UnitCost, opt => opt.MapFrom(src => src.Items.Select(x => x.UnitCost).FirstOrDefault()));

            cfg.CreateMap<StockMovement, StockMovementDTO>();

        });

        public static Mapper GetMapper() {
            return new Mapper(config);
        }
    }
}

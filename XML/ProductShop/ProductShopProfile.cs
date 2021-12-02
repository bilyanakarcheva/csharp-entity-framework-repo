using AutoMapper;
using ProductShop.Dtos.Import;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<ImportUsersDto, User>();

            this.CreateMap<ImportProductDto, Product>();

            this.CreateMap<ImportCategoriesDto, Category>();
        }
    }
}

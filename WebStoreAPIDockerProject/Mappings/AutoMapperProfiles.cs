using AutoMapper;
using StoreWebAPIApplication.DataTransferObjects;
using StoreWebAPIApplication.DomainModels;

namespace StoreWebAPIApplication.Mappings
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles() 
        { 
            CreateMap<ProductDto,Product>().ReverseMap();
            CreateMap<AddProductDto, Product>().ReverseMap();
            CreateMap<UpdateProductRequestDto, Product>().ReverseMap();
        }

    }
}

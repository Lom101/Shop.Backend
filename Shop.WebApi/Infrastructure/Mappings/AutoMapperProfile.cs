using AutoMapper;
using Shop.WebAPI.Dtos;
using Shop.WebAPI.Dtos.Address;
using Shop.WebAPI.Dtos.Address.Requests;
using Shop.WebAPI.Dtos.Address.Responses;
using Shop.WebAPI.Dtos.Brand;
using Shop.WebAPI.Dtos.Category;
using Shop.WebAPI.Dtos.Category.Requests;
using Shop.WebAPI.Dtos.Category.Responses;
using Shop.WebAPI.Dtos.Comment;
using Shop.WebAPI.Dtos.Comment.Requests;
using Shop.WebAPI.Dtos.Comment.Responses;
using Shop.WebAPI.Dtos.Order.Requests;
using Shop.WebAPI.Dtos.Order.Responses;
using Shop.WebAPI.Dtos.Product;
using Shop.WebAPI.Dtos.Product.Requests;
using Shop.WebAPI.Dtos.Product.Responses;
using Shop.WebAPI.Entities;

namespace Shop.WebAPI.Infrastructure.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Color, ColorDto>();
        CreateMap<Model, ModelDto>();
        CreateMap<ModelSize, ModelSizeDto>();
        CreateMap<Size, SizeDto>();
        CreateMap<Photo, PhotoDto>();
        CreateMap<Brand, BrandDto>();
        CreateMap<OrderItem, OrderItemDto>();
        
        // Маппинги для Address
        CreateMap<Address, AddressDto>();
        
        // Маппинги для Category
        CreateMap<Category, CategoryDto>();

        
        // Маппинги для Comment
        CreateMap<Comment, CommentDto>();

        // Маппинги для Order
        CreateMap<Order, OrderDto>();
        
        // Маппинги для Product
        CreateMap<Product, ProductDto>();
        
        // Маппинги для User
        // CreateMap<UserDto, ApplicationUser>()
        //     .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username)) 
        //     .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email)) 
        //     // Игнорируем остальные свойства
        //     .ForMember(dest => dest.Id, opt => opt.Ignore())
        //     .ForMember(dest => dest.Created, opt => opt.Ignore())
        //     .ForMember(dest => dest.Orders, opt => opt.Ignore())
        //     .ForMember(dest => dest.Comments, opt => opt.Ignore())
        //     .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
        //     .ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
        //     .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
        //     .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
        //     .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
        //     .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
        //     .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
        //     .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
        //     .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
        //     .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
        //     .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
        //     .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore());
        CreateMap<ApplicationUser, UserDto>();
    }
}

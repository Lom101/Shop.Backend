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
        // Маппинги для Brand
        CreateMap<Brand, BrandDto>();
        CreateMap<BrandDto, Brand>()
            .ForMember(dest => dest.Products, opt => opt.Ignore());
        
        // Маппинги для OrderItem
        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<OrderItemDto, OrderItem>();
        
        // Маппинги для User
        CreateMap<UserDto, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username)) // Сопоставляем Username с UserName
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email)) // Сопоставляем Email с Email
            // Игнорируем остальные свойства
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Orders, opt => opt.Ignore())
            .ForMember(dest => dest.Comments, opt => opt.Ignore())
            .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
            .ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
            .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
            .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
            .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
            .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
            .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
            .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
            .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore());
        CreateMap<ApplicationUser, UserDto>();
        
        // Маппинги для Address
        CreateMap<CreateAddressRequest, Address>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());
        CreateMap<UpdateAddressRequest, Address>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());
        CreateMap<Address, GetAddressResponse>();
        CreateMap<Address, AddressDto>();
        
        // Маппинги для Category
        CreateMap<CreateCategoryRequest, Category>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Products, opt => opt.Ignore());
        CreateMap<UpdateCategoryRequest, Category>()
            .ForMember(dest => dest.Products, opt => opt.Ignore());
        CreateMap<Category, GetCategoryResponse>();
        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryDto, Category>();

        
        // Маппинги для Comment
        CreateMap<CreateCommentRequest, Comment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());
        CreateMap<UpdateCommentRequest, Comment>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());
        CreateMap<Comment, GetCommentResponse>();

        CreateMap<CommentDto, Comment>();
        CreateMap<Comment, CommentDto>();

        // Маппинги для Order
        CreateMap<CreateOrderRequest, Order>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.OrderDate, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.TotalAmount, opt => opt.Ignore());
        CreateMap<UpdateOrderRequest, Order>()
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.OrderDate, opt => opt.Ignore())
            .ForMember(dest => dest.TotalAmount, opt => opt.Ignore());
        CreateMap<Order, GetOrderResponse>();
        CreateMap<Order, OrderDto>();
        CreateMap<OrderDto, Order>();

        
        // Маппинги для Product
        CreateMap<CreateProductRequest, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Comments, opt => opt.Ignore())
            .ForMember(dest => dest.Brand, opt => opt.Ignore());

        CreateMap<UpdateProductRequest, Product>()
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Comments, opt => opt.Ignore())
            .ForMember(dest => dest.Brand, opt => opt.Ignore());
        CreateMap<Product, GetProductResponse>();
        CreateMap<Product, ProductDto>();
        CreateMap<ProductDto, Product>()
            .ForMember(dest => dest.Brand, opt => opt.Ignore());
    }
}

//CreateMap<IEnumerable<Category>, List<GetCategoryResponse>>();

// {
// "street": "lenina",
// "city": "aznakaevo",
// "state": "russia",
// "zipCode": "423330",
// "userId": "1"
// }
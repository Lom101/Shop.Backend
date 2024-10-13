using AutoMapper;
using Shop.WebAPI.Dtos;
using Shop.WebAPI.Dtos.Address;
using Shop.WebAPI.Dtos.Address.Requests;
using Shop.WebAPI.Dtos.Address.Responses;
using Shop.WebAPI.Dtos.Brand;
using Shop.WebAPI.Dtos.Brand.Response;
using Shop.WebAPI.Dtos.Category;
using Shop.WebAPI.Dtos.Category.Requests;
using Shop.WebAPI.Dtos.Category.Responses;
using Shop.WebAPI.Dtos.Color.Responses;
using Shop.WebAPI.Dtos.Model.Request;
using Shop.WebAPI.Dtos.Model.Response;
using Shop.WebAPI.Dtos.Order.Requests;
using Shop.WebAPI.Dtos.Order.Responses;
using Shop.WebAPI.Dtos.OrderItem;
using Shop.WebAPI.Dtos.OrderItem.Responses;
using Shop.WebAPI.Dtos.Photo.Responses;
using Shop.WebAPI.Dtos.Product;
using Shop.WebAPI.Dtos.Product.Requests;
using Shop.WebAPI.Dtos.Product.Responses;
using Shop.WebAPI.Dtos.Review.Responses;
using Shop.WebAPI.Dtos.Size.Responses;
using Shop.WebAPI.Dtos.User.Responses;
using Shop.WebAPI.Entities;
using CreateOrderRequest = Shop.WebAPI.Dtos.Order.Requests.CreateOrderRequest;

namespace Shop.WebAPI.Infrastructure.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        #region Color
        CreateMap<Color, GetColorResponse>();
        #endregion

        #region Model
        CreateMap<Model, GetModelResponse>()
            .ForMember(dest => dest.Sizes, opt
                => opt.MapFrom(src => src.ModelSizes.Select(ms => ms.Size)))
            .ForMember(dest => dest.IsAvailable, opt
                => opt.MapFrom(src => src.IsAvailable))
            .ForMember(dest => dest.IsAvailable, opt
                => opt.MapFrom(src => src.ModelSizes.Any(ms => ms.StockQuantity > 0)))
            .ForMember(dest => dest.Sizes, opt =>
                opt.MapFrom(src => src.ModelSizes.Select(ms => new GetSizeResponse
                {
                    Id = ms.Size.Id,
                    Name = ms.Size.Name,
                    StockQuantity = ms.StockQuantity // Include stock quantity here
                }).ToList()))
            .ForMember(dest => dest.Name, opt
                => opt.MapFrom(src => src.Product.Name));

        CreateMap<CreateModelRequest, Model>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.Color, opt => opt.Ignore())
            .ForMember(dest => dest.Photos, opt => opt.Ignore())
            .ForMember(m => m.ModelSizes, opt => opt.Ignore())
            .AfterMap((mr, m) => {
                // Remove unselected ModelSizes
                var removedModelSizes = new List<ModelSize>();
                foreach(var ms in m.ModelSizes)
                {
                    if (!mr.SizeIds.Contains(ms.SizeId))
                    {
                        removedModelSizes.Add(ms);
                    }
                }

                foreach(var modelSize in removedModelSizes)
                {
                    m.ModelSizes.Remove(modelSize);
                }

                // Add new ModelSizes
                foreach(var sizeId in mr.SizeIds)
                {
                    if (!m.ModelSizes.Any(ms => ms.SizeId == sizeId))
                    {
                        m.ModelSizes.Add(new ModelSize() { SizeId = sizeId });
                    }
                }
            });

        CreateMap<UpdateModelRequest, Model>()
            .ForMember(m => m.ModelSizes, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.Color, opt => opt.Ignore())
            .ForMember(dest => dest.Photos, opt => opt.Ignore())
            .AfterMap((mr, m) => {
                // Remove unselected ModelSizes
                var removedModelSizes = new List<ModelSize>();
                foreach(var ms in m.ModelSizes)
                {
                    if (!mr.SizeIds.Contains(ms.SizeId))
                    {
                        removedModelSizes.Add(ms);
                    }
                }

                foreach(var modelSize in removedModelSizes)
                {
                    m.ModelSizes.Remove(modelSize);
                }

                // Add new ModelSizes
                foreach(var sizeId in mr.SizeIds)
                {
                    if (!m.ModelSizes.Any(ms => ms.SizeId == sizeId))
                    {
                        m.ModelSizes.Add(new ModelSize() { SizeId = sizeId });
                    }
                }
            });
        #endregion

        #region Size
        CreateMap<Size, GetSizeResponse>()
            .ForMember(m => m.StockQuantity, opt => opt.Ignore());
        #endregion

        #region Address
        CreateMap<CreateAddressRequest, Address>()
            .ForMember(pr => pr.Id, opt => opt.Ignore())
            .ForMember(pr => pr.User, opt => opt.Ignore());

        CreateMap<Address, GetAddressResponse>();
        #endregion

        #region Order

        CreateMap<CreateOrderRequest, Order>();
        CreateMap<Order, GetOrderResponse>();
        #endregion

        #region OrderItem
        CreateMap<CreateOrderItemRequest, OrderItem>()
            .ForMember(pr => pr.Id, opt => opt.Ignore())
            .ForMember(pr => pr.Amount, opt => opt.Ignore())
            .ForMember(pr => pr.OrderId, opt => opt.Ignore())
            .ForMember(pr => pr.Order, opt => opt.Ignore())
            .ForMember(pr => pr.Model, opt => opt.Ignore())
            .ForMember(pr => pr.Size, opt => opt.Ignore());

        CreateMap<OrderItem, GetOrderItemResponse>();
        #endregion

        #region Photo
        CreateMap<Photo, GetPhotoResponse>()
            .ForMember(pr => pr.Url, opt 
                => opt.MapFrom(pr => $"images/{pr.FileName}"));
        #endregion

        #region Brand
        CreateMap<Brand, GetBrandResponse>();
        #endregion

        #region Category
        CreateMap<Category, GetCategoryResponse>();
        CreateMap<CreateCategoryRequest, Category>()
            .ForMember(pr => pr.Id, opt => opt.Ignore());
        #endregion

        #region Review
        CreateMap<Review, GetReviewResponse>();
        #endregion

        #region Product
        CreateMap<Product, GetProductResponse>()
            // Маппинг для поля среднего рейтинга
            .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src =>
                src.Comments.Any() ? src.Comments.Average(c => c.Rating) : 0)) // Рассчитываем средний рейтинг
            // Маппинг для количества комментариев
            .ForMember(dest => dest.CommentsCount, opt => opt.MapFrom(src => src.Comments.Count)) // Количество комментариев
            .ForMember(dest => dest.IsAvailable, opt 
                => opt.MapFrom(src => src.IsAvailable));
        #endregion

        #region User
        CreateMap<ApplicationUser, GetApplicationUserResponse>();
        #endregion
    }
}

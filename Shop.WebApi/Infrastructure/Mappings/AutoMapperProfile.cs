﻿using AutoMapper;
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
using Shop.WebAPI.Dtos.Comment;
using Shop.WebAPI.Dtos.Comment.Requests;
using Shop.WebAPI.Dtos.Comment.Responses;
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
using Shop.WebAPI.Dtos.Size.Responses;
using Shop.WebAPI.Dtos.User.Responses;
using Shop.WebAPI.Entities;

namespace Shop.WebAPI.Infrastructure.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Color, GetColorResponse>();

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
                }).ToList()));
        
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
        
        CreateMap<Size, GetSizeResponse>()
            .ForMember(m => m.StockQuantity, opt => opt.Ignore())
            ;
        
        CreateMap<CreateAddressRequest, Address>()
            .ForMember(pr => pr.Id, opt => opt.Ignore())
            .ForMember(pr => pr.User, opt => opt.Ignore());

        CreateMap<CreateOrderRequest, Order>()
            .ForMember(pr => pr.Id, opt => opt.Ignore())
            .ForMember(pr => pr.Created, opt => opt.Ignore())
            .ForMember(pr => pr.TotalAmount, opt => opt.Ignore())
            .ForMember(pr => pr.User, opt => opt.Ignore())
            .ForMember(pr => pr.ShippingAddress, opt => opt.Ignore()); // 

        CreateMap<CreateOrderItemRequest, OrderItem>()
            .ForMember(pr => pr.Id, opt => opt.Ignore())
            .ForMember(pr => pr.Amount, opt => opt.Ignore()) //
            .ForMember(pr => pr.OrderId, opt => opt.Ignore()) //
            .ForMember(pr => pr.Order, opt => opt.Ignore())
            .ForMember(pr => pr.Model, opt => opt.Ignore())
            .ForMember(pr => pr.Size, opt => opt.Ignore())
            ;
        
        CreateMap<Photo, GetPhotoResponse>()
            .ForMember(pr => pr.Url, opt => opt.Ignore());
        CreateMap<Brand, GetBrandResponse>();
        CreateMap<OrderItem, GetOrderItemResponse>();
        
        // Маппинги для Address
        CreateMap<Address, GetAddressResponse>();
        
        // Маппинги для Category
        CreateMap<Category, GetCategoryResponse>();

        
        // Маппинги для Comment
        CreateMap<Comment, GetCommentResponse>();

        // Маппинги для Order
        CreateMap<Order, GetOrderResponse>();
        
        // Маппинги для Product
        CreateMap<Product, GetProductResponse>()
            // Маппинг для поля среднего рейтинга
            .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src =>
                src.Comments.Any() ? src.Comments.Average(c => c.Rating) : 0)) // Рассчитываем средний рейтинг
            // Маппинг для количества комментариев
            .ForMember(dest => dest.CommentsCount, opt => opt.MapFrom(src => src.Comments.Count)) // Количество комментариев
            .ForMember(dest => dest.IsAvailable, opt 
                => opt.MapFrom(src => src.IsAvailable));
            // .ForMember(dest => dest.AllAvailableSizes, opt => opt.MapFrom(src => 
            //     src.Models.SelectMany(m => m.ModelSizes).Select(ms => ms.Size).Distinct()));
        
        CreateMap<ApplicationUser, GetApplicationUserResponse>();
    }
}

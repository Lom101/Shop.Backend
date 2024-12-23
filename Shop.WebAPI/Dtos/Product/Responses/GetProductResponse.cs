﻿using Shop.WebAPI.Dtos.Brand;
using Shop.WebAPI.Dtos.Brand.Response;
using Shop.WebAPI.Dtos.Category;
using Shop.WebAPI.Dtos.Category.Responses;
using Shop.WebAPI.Dtos.Model.Response;
using Shop.WebAPI.Dtos.Review.Responses;
using Shop.WebAPI.Dtos.Size.Responses;

namespace Shop.WebAPI.Dtos.Product.Responses;

public class GetProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; set; }
    
    public double AverageRating { get; set; } // Поле для среднего рейтинга
    public int CommentsCount { get; set; }  // Количество комментариев

    public bool IsAvailable { get; set; } // Это свойство будет возвращать true, если есть хоть одна доступная модель у продукт

    
    public int CategoryId { get; set; }
    public GetCategoryResponse Category { get; set; }
    
    public int BrandId { get; set; }
    public GetBrandResponse Brand { get; set; }
    
    public ICollection<GetModelResponse> Models { get; set; }
    public List<GetReviewResponse> Comments { get; set; }
    
}

﻿namespace Shop.WebAPI.Dtos.Review.Requests;

public class CreateReviewRequest
{
    public string Text { get; set; }
    public int Rating { get; set; }  // Значение рейтинга от 1 до 5
    public int ProductId { get; set; }
    public string UserId { get; set; }
}
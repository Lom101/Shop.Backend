using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.WebAPI.Dtos.Review.Requests;
using Shop.WebAPI.Dtos.Review.Responses;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;

namespace Shop.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        // Получить отзыв по продукту и ID пользователя
        // GET api/review?productId={productId}
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetReviewByProductId([FromQuery] int productId)
        {
            // Получаем userId из claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            var review = await _reviewRepository.GetReviewByProductAndUserAsync(productId, userId);

            if (review == null)
            {
                return NotFound();
            }
            return Ok(review);
        }


        // Добавить новый отзыв
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateReview([FromBody] CreateReviewRequest request)
        {
            // Проверка на дубликаты
            var existingReview = await _reviewRepository.GetReviewByProductAndUserAsync(request.ProductId, request.UserId);

            if (existingReview != null)
            {
                return BadRequest("Отзыв уже существует для этого продукта от этого пользователя.");
            }

            // Используем AutoMapper для преобразования
            var newReview = _mapper.Map<Review>(request);

            await _reviewRepository.AddReviewAsync(newReview);

            return CreatedAtAction(nameof(GetReviewByProductId), new { productId = newReview.ProductId, userId = newReview.UserId }, newReview);
        }

        // Получить все отзывы
        [HttpGet("all")]
        public async Task<IActionResult> GetAllReviewsByProductId([FromQuery] int productId)
        {
            var reviews = await _reviewRepository.GetAllReviewsByProductIdAsync(productId);
            
            var reviewsDtos = _mapper.Map<IEnumerable<GetReviewResponse>>(reviews);
            return Ok(reviewsDtos);
        }
    }
}

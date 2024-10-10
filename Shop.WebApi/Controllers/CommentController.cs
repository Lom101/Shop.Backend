using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.WebAPI.Data;
using Shop.WebAPI.Dtos.Comment.Requests;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;
    private ShopApplicationContext _context;

    public CommentController(ICommentService commentService,  ShopApplicationContext context)
    {
        _commentService = commentService;
        _context = context;
    }
    
    // Получить отзыв по продукту и ID пользователя
    [HttpGet]
    public async Task<ActionResult> GetReviewByProductId(int productId, string userId)
    {
        var review = await _context.Comments
            .FirstOrDefaultAsync(r => r.ProductId == productId && r.UserId == userId);
    
        if (review == null)
        {
            return NotFound();
        }
        return Ok(review);
    }


    // Добавить новый отзыв
    [HttpPost]
    public async Task<ActionResult> CreateReview(CreateCommentRequest comment)
    {
        var comment2 = new Comment
        {
            Text = comment.Text,
            Rating = comment.Rating,
            ProductId = comment.ProductId,
            UserId = comment.UserId
        };
        
        // Проверка на дубликаты
        var existingComment = await _context.Comments
            .FirstOrDefaultAsync(c => c.ProductId == comment.ProductId && c.UserId == comment.UserId);
    
        if (existingComment != null)
        {
            return BadRequest("Комментарий уже существует для этого продукта от этого пользователя.");
        }
        
        _context.Comments.Add(comment2);
        await _context.SaveChangesAsync();    
        
        return CreatedAtAction(nameof(GetReviewByProductId), new { productId = comment2.ProductId }, comment2);
    }
    
    //
    // // GET: api/comment
    // [HttpGet]
    // public async Task<IActionResult> GetAllComments()
    // {
    //     var comments = await _commentService.GetAllCommentsAsync();
    //     return Ok(comments);
    // }
    //
    // // GET: api/comment/get_by_user_id
    // [HttpGet("get_by_user_id")]
    // public async Task<IActionResult> GetCommentsByUserId(string userId)
    // {
    //     var comments = await _commentService.GetCommentsByUserId(userId);
    //     return Ok(comments);
    // }
    //
    //
    // // GET: api/comment/{id}
    // [HttpGet("{id}")]
    // public async Task<IActionResult> GetCommentById(int id)
    // {
    //     var comment = await _commentService.GetCommentByIdAsync(id);
    //     if (comment == null)
    //     {
    //         return NotFound();
    //     }
    //     return Ok(comment);
    // }
    //
    // // POST: api/comment
    // [HttpPost]
    // public async Task<IActionResult> AddComment([FromBody] CreateCommentRequest createCommentRequest)
    // {
    //     var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
    //     // Проверяем, может ли пользователь оставить отзыв
    //     if (!_commentService.CanUserLeaveReview(userId, createCommentRequest.ProductId))
    //     {
    //         return BadRequest("Вы можете оставить отзыв только если покупали этот товар.");
    //     }
    //
    //     
    //     var newCommentId = await _commentService.AddCommentAsync(createCommentRequest);
    //     return CreatedAtAction(nameof(GetCommentById), new { id = newCommentId }, createCommentRequest);
    // }
    //
    // // PUT: api/comment/{id}
    // [HttpPut("{id}")]
    // public async Task<IActionResult> UpdateComment(int id, [FromBody] UpdateCommentRequest updateCommentRequest)
    // {
    //     if (id != updateCommentRequest.Id)
    //     {
    //         return BadRequest("comment ID mismatch");
    //     }
    //
    //     var isUpdated = await _commentService.UpdateCommentAsync(updateCommentRequest);
    //     if (!isUpdated)
    //     {
    //         return NotFound();
    //     }
    //     return NoContent();
    // }
    //
    // // DELETE: api/comment/{id}
    // [HttpDelete("{id}")]
    // public async Task<IActionResult> DeleteComment(int id)
    // {
    //     var isDeleted = await _commentService.DeleteCommentAsync(id);
    //     if (!isDeleted)
    //     {
    //         return NotFound();
    //     }
    //     return NoContent();
    // }
}

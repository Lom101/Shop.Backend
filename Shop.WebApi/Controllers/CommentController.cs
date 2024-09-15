using Microsoft.AspNetCore.Mvc;
using Shop.WebAPI.Dtos.Comment.Requests;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    // GET: api/comment
    [HttpGet]
    public async Task<IActionResult> GetAllComments()
    {
        var comments = await _commentService.GetAllCommentsAsync();
        return Ok(comments);
    }

    // GET: api/comment/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCommentById(int id)
    {
        var comment = await _commentService.GetCommentByIdAsync(id);
        if (comment == null)
        {
            return NotFound();
        }
        return Ok(comment);
    }

    // POST: api/comment
    [HttpPost]
    public async Task<IActionResult> AddComment([FromBody] CreateCommentRequest createCommentRequest)
    {
        var newCommentId = await _commentService.AddCommentAsync(createCommentRequest);
        return CreatedAtAction(nameof(GetCommentById), new { id = newCommentId }, createCommentRequest);
    }

    // PUT: api/comment/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment(int id, [FromBody] UpdateCommentRequest updateCommentRequest)
    {
        if (id != updateCommentRequest.Id)
        {
            return BadRequest("comment ID mismatch");
        }

        var isUpdated = await _commentService.UpdateCommentAsync(updateCommentRequest);
        if (!isUpdated)
        {
            return NotFound();
        }
        return NoContent();
    }

    // DELETE: api/comment/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var isDeleted = await _commentService.DeleteCommentAsync(id);
        if (!isDeleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}

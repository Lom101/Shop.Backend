using Shop.WebAPI.Dtos.Comment.Requests;
using Shop.WebAPI.Dtos.Comment.Responses;

namespace Shop.WebAPI.Services.Interfaces;

public interface ICommentService
{
    Task<GetCommentResponse> GetCommentByIdAsync(int id);
    Task<IEnumerable<GetCommentResponse>> GetAllCommentsAsync();
    Task<int> AddCommentAsync(CreateCommentRequest commentDto);
    Task<bool> UpdateCommentAsync(UpdateCommentRequest commentDto);
    Task<bool> DeleteCommentAsync(int id);
}
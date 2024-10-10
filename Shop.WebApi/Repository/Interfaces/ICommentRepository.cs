using Shop.WebAPI.Entities;

namespace Shop.WebAPI.Repository.Interfaces;

public interface ICommentRepository
{
    Task<Comment> GetByIdAsync(int id);
    Task<IEnumerable<Comment>> GetAllAsync();
    Task AddAsync(Comment comment);
    Task UpdateAsync(Comment comment);
    Task DeleteAsync(int id);
    bool CanUserLeaveReview(string userId, int productId);
    Task<IEnumerable<Comment>> GetByUserId(string userId);
}
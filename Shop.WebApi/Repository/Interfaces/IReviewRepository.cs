using Shop.WebAPI.Entities;

namespace Shop.WebAPI.Repository.Interfaces;

public interface IReviewRepository
{
    Task<Review> GetReviewByProductAndUserAsync(int productId, string userId);
    Task AddReviewAsync(Review review);
    Task<IEnumerable<Review>> GetAllReviewsByProductIdAsync(int productId);
    Task SaveChangesAsync();
}
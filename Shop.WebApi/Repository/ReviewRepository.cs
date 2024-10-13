using Microsoft.EntityFrameworkCore;
using Shop.WebAPI.Data;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;

namespace Shop.WebAPI.Repository;

public class ReviewRepository : IReviewRepository
{
    private readonly ShopApplicationContext _context;

    public ReviewRepository(ShopApplicationContext context)
    {
        _context = context;
    }

    public async Task<Review> GetReviewByProductAndUserAsync(int productId, string userId)
    {
        return await _context.Reviews
            .FirstOrDefaultAsync(r => r.ProductId == productId && r.UserId == userId);
    }

    public async Task AddReviewAsync(Review review)
    {
        _context.Reviews.Add(review);
        await SaveChangesAsync();
    }

    public async Task<IEnumerable<Review>> GetAllReviewsAsync()
    {
        return await _context.Reviews.ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
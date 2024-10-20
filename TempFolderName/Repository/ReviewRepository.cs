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

    public async Task<IEnumerable<Review>> GetAllReviewsByProductIdAsync(int productId)
    {
        var reviews = await _context.Reviews
            //.Include(r => r.User) // с этим не работает
            .Where(r => r.ProductId == productId)
            .ToListAsync();

        return reviews;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
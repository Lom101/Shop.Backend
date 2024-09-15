﻿using Microsoft.EntityFrameworkCore;
using Shop.WebAPI.Data;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;

namespace Shop.WebAPI.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly ShopApplicationContext _context;

    public CommentRepository(ShopApplicationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Comment>> GetAllAsync()
    {
        // return await _context.Comments.Include(c => c.Product)
        //     .Include(c => c.User)
        //     .ToListAsync();
        return await _context.Comments.ToListAsync();
    }

    public async Task<Comment> GetByIdAsync(int id)
    {
        // return await _context.Comments.Include(c => c.Product)
        //     .Include(c => c.User)
        //     .FirstOrDefaultAsync(c => c.Id == id);
        return await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Comment comment)
    {
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var comment = await GetByIdAsync(id);
        if (comment != null)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}
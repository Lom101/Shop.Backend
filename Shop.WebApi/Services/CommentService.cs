using AutoMapper;
using Shop.WebAPI.Dtos.Comment.Requests;
using Shop.WebAPI.Dtos.Comment.Responses;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;

    public CommentService(ICommentRepository commentRepository, IMapper mapper)
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
    }

    public async Task<GetCommentResponse> GetCommentByIdAsync(int id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        return _mapper.Map<GetCommentResponse>(comment);
    }

    public async Task<IEnumerable<GetCommentResponse>> GetAllCommentsAsync()
    {
        var comments = await _commentRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<GetCommentResponse>>(comments);
    }

    public async Task<int> AddCommentAsync(CreateCommentRequest commentDto)
    {
        var comment = _mapper.Map<Comment>(commentDto);
        await _commentRepository.AddAsync(comment);
        return comment.Id;
    }

    public async Task<bool> UpdateCommentAsync(UpdateCommentRequest commentDto)
    {
        var comment = _mapper.Map<Comment>(commentDto);
        var existingComment = await _commentRepository.GetByIdAsync(comment.Id);
        if (existingComment == null)
        {
            return false; // Комментарий не найден
        }
        await _commentRepository.UpdateAsync(comment);
        return true;
    }

    public async Task<bool> DeleteCommentAsync(int id)
    {
        var existingComment = await _commentRepository.GetByIdAsync(id);
        if (existingComment == null)
        {
            return false; // Комментарий не найден
        }
        await _commentRepository.DeleteAsync(id);
        return true;
    }
}
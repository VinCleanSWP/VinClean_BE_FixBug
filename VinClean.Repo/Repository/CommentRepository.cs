using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

namespace VinClean.Repo.Repository
{
    public interface ICommentRepository
    {
        Task<ICollection<Comment>> GetComments();
        Task<Comment> GetCommentsById(int id);
        Task<bool> CreateComment(Comment comment);
        Task<bool> UpdateComment(Comment comment);
        Task<bool> DeleteComment(Comment comment);
        Task<ICollection<Comment>> GetCommentsByBlogId(int blogId);

    }
    public class CommentRepository: ICommentRepository
    {
        private readonly ServiceAppContext _context;
        public CommentRepository(ServiceAppContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateComment(Comment comment)
        {
            _context.Comments.Add(comment);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> ICommentRepository.UpdateComment(Comment comment)
        {
            _context.Comments.Update(comment);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<ICollection<Comment>> ICommentRepository.GetComments()
        {
            return await _context.Comments.ToListAsync();
        }

        async Task<Comment> ICommentRepository.GetCommentsById(int id)
        {
            return await _context.Comments.FirstOrDefaultAsync(a => a.CommentId == id);
        }

     

       async Task<bool> ICommentRepository.DeleteComment(Comment comment)
        {
            _context.Comments.Remove(comment);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<ICollection<Comment>> ICommentRepository.GetCommentsByBlogId(int blogId)
        {
          
                return await _context.Comments.Where(c => c.BlogId == blogId).ToListAsync();
            
        }
    }
}

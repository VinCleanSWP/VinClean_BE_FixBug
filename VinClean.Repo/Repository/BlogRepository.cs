using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;


namespace VinClean.Repo.Repository
{
    public interface IBlogRepository
    {
        Task<Blog> GetBlogById(int id);
        Task<bool> CreateBlog(Blog blog);
        Task<ICollection<Blog>> GetBlogs();
        Task<bool> Deleteblog(Blog blog);
        Task<bool> UpdateBlog(Blog blog);
      


    }
    public class BlogRepository:IBlogRepository
    {
        private readonly ServiceAppContext _context;
        public BlogRepository(ServiceAppContext context)
        {
            _context = context;
        }

     

        async Task<ICollection<Blog>> IBlogRepository.GetBlogs()
        {
            return await _context.Blogs.ToListAsync();
        }
        async Task<Blog> IBlogRepository.GetBlogById(int id)
        {
            return await _context.Blogs.FirstOrDefaultAsync(a => a.BlogId == id);
        }
        async Task<bool> IBlogRepository.CreateBlog(Blog blog)
        {
            _context.Blogs.Add(blog);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IBlogRepository.Deleteblog(Blog blog)
        {
            _context.Blogs.Remove(blog);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IBlogRepository.UpdateBlog(Blog blog)
        {
            _context.Blogs.Update(blog);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        
    }
}

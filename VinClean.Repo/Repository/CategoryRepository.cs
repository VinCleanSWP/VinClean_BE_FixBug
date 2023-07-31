using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

namespace VinClean.Repo.Repository
{
    public interface ICategoryRepository
    {
        Task<ICollection<Category>> GetCategoryList();
        Task<Category> GetCategoryById(int id);
        Task<bool> CreateCategory(Category category);
        Task<bool> DeleteCategory(Category category);
        Task<bool> UpdateCategory(Category category);
       
    }
   
   
        public class CategoryRepository : ICategoryRepository
        {
            private readonly ServiceAppContext _context;
            public CategoryRepository(ServiceAppContext context)
            {
                _context = context;
            }

        async Task<bool> ICategoryRepository.CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> ICategoryRepository.DeleteCategory(Category category)
        {
            _context.Categories.Remove(category);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<Category> ICategoryRepository.GetCategoryById(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(a => a.CategoryId == id);
        }

  

        async Task<ICollection<Category>> ICategoryRepository.GetCategoryList()
        {
            return await _context.Categories.ToListAsync();
        }

    

        async Task<bool> ICategoryRepository.UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
    }


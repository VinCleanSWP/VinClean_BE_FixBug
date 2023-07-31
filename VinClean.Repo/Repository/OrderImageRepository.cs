using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

namespace VinClean.Repo.Repository
{
    public interface IOrderImageRepository
    {
        Task<ICollection<OrderImage>> OrderImageList();
        Task<ICollection<OrderImage>> OrderImageListByProcessId(int id);
        Task<OrderImage> OrderImageById(int id);
        Task<bool> UpdateOrderImage(OrderImage customer);
        Task<bool> AddOrderImage(OrderImage slot);
        Task<bool> DeleteOrderImage(OrderImage workingBy);
    }
    public class OrderImageRepository : IOrderImageRepository
    {
        private readonly ServiceAppContext _context;
        public OrderImageRepository(ServiceAppContext context)
        {
            _context = context;
        }
        async Task<ICollection<OrderImage>> IOrderImageRepository.OrderImageList()
        {
            return await _context.OrderImages.ToListAsync();
        }
        async Task<ICollection<OrderImage>> IOrderImageRepository.OrderImageListByProcessId(int id)
        {
            return await _context.OrderImages.Where(e => e.OrderId == id).ToListAsync();
        }

        async Task<OrderImage> IOrderImageRepository.OrderImageById(int id)
        {
            return await _context.OrderImages.FirstOrDefaultAsync(a => a.Id == id);
        }

        async Task<bool> IOrderImageRepository.UpdateOrderImage(OrderImage OrderImage)
        {
            _context.OrderImages.Update(OrderImage);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IOrderImageRepository.AddOrderImage(OrderImage OrderImage)
        {
            _context.OrderImages.Add(OrderImage);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
        async Task<bool> IOrderImageRepository.DeleteOrderImage(OrderImage OrderImage)
        {
            _context.OrderImages.Remove(OrderImage);
            return await _context.SaveChangesAsync() > 0 ? true : false; ;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

namespace VinClean.Repo.Repository
{
    public interface IServiceWorkInRepository
    {
        Task<ICollection<ServiceWorkIn>> GetServiceWorkInList();
        Task<ServiceWorkIn> GetServiceWorkInById(int ServiceId);
        Task<bool> CreateServiceWorkIn(ServiceWorkIn ServiceWorkIn);
        Task<bool> DeleteServiceWorkIn(ServiceWorkIn ServiceWorkIn);
        Task<bool> UpdateServiceWorkIn(ServiceWorkIn ServiceWorkIn);

    }


    public class ServiceWorkInRepository : IServiceWorkInRepository
    {
        private readonly ServiceAppContext _context;
        public ServiceWorkInRepository(ServiceAppContext context)
        {
            _context = context;
        }

        async Task<bool> IServiceWorkInRepository.CreateServiceWorkIn(ServiceWorkIn ServiceWorkIn)
        {
            _context.ServiceWorkIns.Add(ServiceWorkIn);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IServiceWorkInRepository.DeleteServiceWorkIn(ServiceWorkIn ServiceWorkIn)
        {
            _context.ServiceWorkIns.Remove(ServiceWorkIn);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<ServiceWorkIn> IServiceWorkInRepository.GetServiceWorkInById(int ServiceId)
        {
            return await _context.ServiceWorkIns.FirstOrDefaultAsync(a => a.ServiceId == ServiceId);
        }



        async Task<ICollection<ServiceWorkIn>> IServiceWorkInRepository.GetServiceWorkInList()
        {
            return await _context.ServiceWorkIns.ToListAsync();
        }



        async Task<bool> IServiceWorkInRepository.UpdateServiceWorkIn(ServiceWorkIn ServiceWorkIn)
        {
            _context.ServiceWorkIns.Update(ServiceWorkIn);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

namespace VinClean.Repo.Repository
{
    public interface IServiceRepository
    {
        Task<ICollection<VinClean.Repo.Models.Service>> GetServiceList();
        Task<ICollection<VinClean.Repo.Models.Service>> GetServiceListById(int id);
        Task<VinClean.Repo.Models.Service> GetServiceById(int id);
        Task<bool> AddService(VinClean.Repo.Models.Service service);
        Task<bool> DeleteService(VinClean.Repo.Models.Service service);
        Task<bool> UpdateService(VinClean.Repo.Models.Service service);

    }


    public class ServiceRepository : IServiceRepository
    {
        private readonly ServiceAppContext _context;
        public ServiceRepository(ServiceAppContext context)
        {
            _context = context;
        }

        async Task<bool> IServiceRepository.AddService(VinClean.Repo.Models.Service service)
        {
            _context.Services.Add(service);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IServiceRepository.DeleteService(VinClean.Repo.Models.Service service)
        {
            _context.Services.Remove(service);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<VinClean.Repo.Models.Service> IServiceRepository.GetServiceById(int id)
        {
            return await _context.Services.FirstOrDefaultAsync(a => a.ServiceId == id);
        }



        async Task<ICollection<VinClean.Repo.Models.Service>> IServiceRepository.GetServiceList()
        {
            return await _context.Services.ToListAsync();
        }

        async Task<ICollection<VinClean.Repo.Models.Service>> IServiceRepository.GetServiceListById(int id)
        {
            return await _context.Services.Where(e => e.TypeId == id).ToListAsync();
        }



        async Task<bool> IServiceRepository.UpdateService(VinClean.Repo.Models.Service service)
        {
            _context.Services.Update(service);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}

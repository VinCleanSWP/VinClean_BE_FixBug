using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

namespace VinClean.Repo.Repository
{
    public interface ILocationRepository
    {
        Task<ICollection<Location>> GetLocationList();
        Task<IEnumerable<Location>> GetLocationListByEpmId(int id);
        Task<Location> GetLocationById(int id);
        Task<bool> Check(int id);
        Task<Location> GetLocationByOrderId(int id);
        Task<bool> UpdateLocation(Location customer);
        Task<bool> AddLocation(Location slot);
        Task<bool> DeleteLocation(Location Location);
    }
    public class LocationRepository : ILocationRepository
    {
        private readonly ServiceAppContext _context;
        public LocationRepository(ServiceAppContext context)
        {
           _context = context;
        }
        async Task<ICollection<Location>> ILocationRepository.GetLocationList()
        {
            return await _context.Locations.ToListAsync();
        }
        async Task<IEnumerable<Location>> ILocationRepository.GetLocationListByEpmId(int id)
        {
            return await _context.Locations.Where(e=>e.EmployeeId == id).ToListAsync();
        }
        async Task<Location> ILocationRepository.GetLocationById(int id)
        {
            return await _context.Locations.FirstOrDefaultAsync(a => a.EmployeeId == id);
        }
         async Task<bool> ILocationRepository.Check(int id)
        {
            return await _context.Locations.AnyAsync(a => a.EmployeeId == id);
        }
        async Task<Location> ILocationRepository.GetLocationByOrderId(int id)
        {
            return await _context.Locations.FirstOrDefaultAsync(a => a.OrderId == id);
        }

        async Task<bool> ILocationRepository.UpdateLocation(Location slot)
        {
            _context.Locations.Update(slot);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> ILocationRepository.AddLocation(Location slot)
        {
            _context.Locations.Add(slot);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> ILocationRepository.DeleteLocation(Location Location)
        {
             
                _context.Locations.Remove(Location);
                return await _context.SaveChangesAsync() > 0 ? true : false; ;


        }

    }
}

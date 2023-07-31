using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

namespace VinClean.Repo.Repository
{
    public interface IBuildingRepository
    {
        Task<ICollection<Building>> GetBuildingList();
        Task<ICollection<Building>> GetBuildingListByType(int id);
        Task<Building> GetBuildingById(int id);
        Task<bool> AddBuilding(Building role);
        Task<bool> DeleteBuilding(Building role);
        Task<bool> UpdateBuilding(Building role);
    }
    public class BuildingRepository : IBuildingRepository
    {
        private readonly ServiceAppContext _context;
        public BuildingRepository()
        {
            _context = new ServiceAppContext();
        }

        async Task<bool> IBuildingRepository.AddBuilding(Building role)
        {
            _context.Buildings.Add(role);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IBuildingRepository.DeleteBuilding(Building role)
        {
            _context.Buildings.Remove(role);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<Building> IBuildingRepository.GetBuildingById(int id)
        {
            return await _context.Buildings.FirstOrDefaultAsync(a => a.Id == id);
        }


        async Task<ICollection<Building>> IBuildingRepository.GetBuildingList()
        {
            return await _context.Buildings.ToListAsync();
        }

        async Task<ICollection<Building>> IBuildingRepository.GetBuildingListByType(int id)
        {
            return await _context.Buildings.Where(e=>e.TypeId == id).ToListAsync();
        }

        async Task<bool> IBuildingRepository.UpdateBuilding(Building role)
        {
            _context.Buildings.Update(role);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }



}

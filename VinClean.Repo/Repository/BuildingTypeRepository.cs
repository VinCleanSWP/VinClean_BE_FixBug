using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

namespace VinClean.Repo.Repository
{
    public interface IBuildingTypeRepository
    {
        Task<ICollection<BuildingType>> GetBuildingTypeList();
        Task<BuildingType> GetBuildingTypeById(int id);
        Task<bool> AddBuildingType(BuildingType BuildingType);
        Task<bool> DeleteBuildingType(BuildingType BuildingType);
        Task<bool> UpdateBuildingType(BuildingType rBuildingTypeole);
    }
    public class BuildingTypeRepository : IBuildingTypeRepository
    {
        private readonly ServiceAppContext _context;
        public BuildingTypeRepository(ServiceAppContext context)
        {
            _context = context;
        }

        async Task<bool> IBuildingTypeRepository.AddBuildingType(BuildingType role)
        {
            _context.BuildingTypes.Add(role);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IBuildingTypeRepository.DeleteBuildingType(BuildingType role)
        {
            _context.BuildingTypes.Remove(role);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<BuildingType> IBuildingTypeRepository.GetBuildingTypeById(int id)
        {
            return await _context.BuildingTypes.FirstOrDefaultAsync(a => a.Id == id);
        }


        async Task<ICollection<BuildingType>> IBuildingTypeRepository.GetBuildingTypeList()
        {
            return await _context.BuildingTypes.ToListAsync();
        }


        async Task<bool> IBuildingTypeRepository.UpdateBuildingType(BuildingType role)
        {
            _context.BuildingTypes.Update(role);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

    }
}

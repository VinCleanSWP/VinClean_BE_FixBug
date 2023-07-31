using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

namespace VinClean.Repo.Repository
{
    public interface ITypeRepository
    {
        Task<ICollection<VinClean.Repo.Models.Type>> GetTypeList();
        Task<VinClean.Repo.Models.Type> GetTypeById(int id);
        Task<bool> UpdateType(VinClean.Repo.Models.Type customer);
        Task<bool> addType(VinClean.Repo.Models.Type customer);
        Task<bool> DeleteType(int id);
    }
    public class TypeRepository : ITypeRepository
    {
        private readonly ServiceAppContext _context;  
        public TypeRepository(ServiceAppContext context)
        {
            _context = context;
        }

        async Task<ICollection<Models.Type>> ITypeRepository.GetTypeList()
        {
            return await _context.Types.ToListAsync();
        }
        async Task<Models.Type> ITypeRepository.GetTypeById(int id)
        {
            return await _context.Types.FirstOrDefaultAsync(a => a.TypeId == id);
        }

        async Task<bool> ITypeRepository.UpdateType(Models.Type type)
        {
            _context.Types.Update(type);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
        async Task<bool> ITypeRepository.addType(Models.Type type)
        {
            _context.Types.Add(type);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> ITypeRepository.DeleteType(int id)
        {
            var _exisitngType = await _context.Types.FirstOrDefaultAsync(a => a.TypeId == id);
            if (_exisitngType != null)
            {
                _exisitngType.Avaiable = false;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
      
    }
}

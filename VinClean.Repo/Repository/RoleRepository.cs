using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

namespace VinClean.Repo.Repository
{
    public interface IRoleRepository
    {
        Task<ICollection<Role>> GetRoleList();
        Task<Role> GetRoleById(int id);
        Task<bool> AddRole(Role role);
        Task<bool> DeleteRole(Role role);
        Task<bool> UpdateRole(Role role);

    }


    public class RoleRepository : IRoleRepository
    {
        private readonly ServiceAppContext _context;
        public RoleRepository(ServiceAppContext context)
        {
            _context = context;
        }

        async Task<bool> IRoleRepository.AddRole(Role role)
        {
            _context.Roles.Add(role);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IRoleRepository.DeleteRole(Role role)
        {
            _context.Roles.Remove(role);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<Role> IRoleRepository.GetRoleById(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(a => a.RoleId == id);
        }



        async Task<ICollection<Role>> IRoleRepository.GetRoleList()
        {
            return await _context.Roles.ToListAsync();
        }



        async Task<bool> IRoleRepository.UpdateRole(Role role)
        {
            _context.Roles.Update(role);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}

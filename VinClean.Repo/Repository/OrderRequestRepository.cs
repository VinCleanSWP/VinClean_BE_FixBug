using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VinClean.Repo.Models;

// lay table

namespace VinClean.Repo.Repository
{
    public interface IOrderRequestRepository
    {
        Task<ICollection<OrderRequestModel>> GetPSList();
        Task<OrderRequestModel> GetInfoPSById(int id);
        Task<OrderRequest> GetPSById(int id);
        Task<bool> AddPS(OrderRequest OrderRequest);
        Task<bool> UpdatePS(OrderRequest OrderRequest);
        Task<bool> DeletePS(OrderRequest OrderRequest);
    }
    public class OrderRequestRepository : IOrderRequestRepository
    {
        private readonly ServiceAppContext _context;

        public OrderRequestRepository(ServiceAppContext context)
        {
            _context = context;
        }

        async Task<ICollection<OrderRequestModel>> IOrderRequestRepository.GetPSList()
        {
            /*return await _context.OrderRequests.Include(e=>e.OldEmployee)
                .Include(e=>e.Process).ThenInclude(p=>p.Customer)
                .Include(e=>e.NewEmployee)
                .Include(e=>e.CreateByNavigation)
                .ToListAsync();*/
            var query = from ps in _context.OrderRequests
                        join oe in _context.Employees on ps.OldEmployeeId equals oe.EmployeeId into oeGroup
                        from oe in oeGroup.DefaultIfEmpty()
                        join oeac in _context.Accounts on oe.AccountId equals oeac.AccountId into oeacGroup
                        from oeac in oeacGroup.DefaultIfEmpty()
                        join ne in _context.Employees on ps.NewEmployeeId equals ne.EmployeeId into neGroup
                        from ne in neGroup.DefaultIfEmpty()
                        join neac in _context.Accounts on ne.AccountId equals neac.AccountId into neacGroup
                        from neac in neacGroup.DefaultIfEmpty()
                        join p in _context.Orders on ps.OrderId equals p.OrderId into pGroup
                        from p in pGroup.DefaultIfEmpty()
                        join c in _context.Customers on p.CustomerId equals c.CustomerId into cGroup
                        from c in cGroup.DefaultIfEmpty()
                        select new OrderRequestModel
                        {
                            OrderId = p.OrderId,
                            CustomerId = (int)p.CustomerId,
                            CustomerName = c.LastName +" "+ c.FirstName,
                            Address = p.Address,
                            AccountId = ps.CreateBy,
                            OldEmployeeId = (int)ps.OldEmployeeId,
                            OldEmployeeName = oeac.Name,
                            OldEmployePhone = oe.Phone,
                            OldEmployeEmail = oeac.Email,
                            OldEmployeImg = oeac.Img,
                            ServiceId = (int)p.ServiceId,
                            Date = p.Date,
                            StartTime = p.StarTime,
                            EndTime = p.EndTime,
                            NewEmployeeId = ps.NewEmployeeId,
                            NewEmployeeName = neac.Name,
                            NewEmployePhone = ne.Phone,
                            NewEmployeEmail = neac.Email,
                            NewEmployeImg = neac.Img,
                            Status = ps.Satus,
                            Reason = ps.Note
                        };
            return await query.ToListAsync();
                        //join c in _context.Customers on p.CustomerId equals c.CustomerId into cGroup
                        //from c in cGroup.DefaultIfEmpty()
                        //join ac in _context.Accounts on c.AccountId equals ac.AccountId into acGroup
                        //from ac in acGroup.DefaultIfEmpty()

        }

        async Task<OrderRequestModel> IOrderRequestRepository.GetInfoPSById(int id)
        {
            var query = from ps in _context.OrderRequests
                        join oe in _context.Employees on ps.OldEmployeeId equals oe.EmployeeId into oeGroup
                        from oe in oeGroup.DefaultIfEmpty()
                        join oeac in _context.Accounts on oe.AccountId equals oeac.AccountId into oeacGroup
                        from oeac in oeacGroup.DefaultIfEmpty()
                        join ne in _context.Employees on ps.NewEmployeeId equals ne.EmployeeId into neGroup
                        from ne in neGroup.DefaultIfEmpty()
                        join neac in _context.Accounts on ne.AccountId equals neac.AccountId into neacGroup
                        from neac in neacGroup.DefaultIfEmpty()
                        join p in _context.Orders on ps.OrderId equals p.OrderId into pGroup
                        from p in pGroup.DefaultIfEmpty()
                        join c in _context.Customers on p.CustomerId equals c.CustomerId into cGroup
                        from c in cGroup.DefaultIfEmpty()
                        where ps.OrderId == id
                        select new OrderRequestModel
                        {
                            OrderId = p.OrderId,
                            CustomerId = (int)p.CustomerId,
                            CustomerName = c.LastName + c.FirstName,
                            Address = p.Address,
                            AccountId = ps.CreateBy,
                            OldEmployeeId = (int)ps.OldEmployeeId,
                            OldEmployeeName = oeac.Name,
                            OldEmployePhone = oe.Phone,
                            OldEmployeEmail = oeac.Email,
                            OldEmployeImg = oeac.Img,
                            ServiceId = (int)p.ServiceId,
                            Date = p.Date,
                            StartTime = p.StarTime,
                            EndTime = p.EndTime,
                            NewEmployeeId = (int)ps.NewEmployeeId,
                            NewEmployeeName = neac.Name,
                            NewEmployePhone = ne.Phone,
                            NewEmployeEmail = neac.Email,
                            NewEmployeImg = neac.Img,
                            Status = ps.Satus,
                            Reason = ps.Note
                        };
            return await query.FirstOrDefaultAsync();
        }
        async Task<OrderRequest> IOrderRequestRepository.GetPSById(int id)
        {
            return await _context.OrderRequests.FirstOrDefaultAsync(ps => ps.OrderId == id);
        }
        async Task<bool> IOrderRequestRepository.AddPS(OrderRequest OrderRequest)
        {
            _context.Add(OrderRequest);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IOrderRequestRepository.UpdatePS(OrderRequest OrderRequest)
        {
            _context.Update(OrderRequest);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IOrderRequestRepository.DeletePS(OrderRequest OrderRequest)
        {
            _context.Remove(OrderRequest);
            return await _context.SaveChangesAsync() > 0 ? true: false;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Repo.Models.ProcessModel;

// lay table

namespace VinClean.Repo.Repository
{
    public interface IOrderRepository
    {
        Task<ICollection<OrderModeDTO>> GetOrderlist();
        Task<Order> GetOrderById(int id);
        Task<OrderModeDTO> GetAllInfoById(int id);
        Task<bool> AddOrder(Order order);
        Task<bool> UpdateOrder(Order order);
        Task<bool> DeleteOrder(Order order);
        Task<ICollection<OrderModeDTO>> SelectOrder(SelectOrder select);
        Task<ICollection<OrderModeDTO>> SelectAllOrder(SelectOrder select);

    }
    public class OrderRepository : IOrderRepository
    {
        private readonly ServiceAppContext _context;
        public OrderRepository(ServiceAppContext context)
        {
            _context = context;
        }

        async Task<ICollection<OrderModeDTO>> IOrderRepository.GetOrderlist()
        {
            var query = from p in _context.Orders
                        join s in _context.Services on p.ServiceId equals s.ServiceId into sGroup
                        from s in sGroup.DefaultIfEmpty()
                        join ps in _context.OrderRequests on p.OrderId equals ps.OrderId into psGroup
                        from ps in psGroup.DefaultIfEmpty()
                        join c in _context.Customers on p.CustomerId equals c.CustomerId into cGroup
                        from c in cGroup.DefaultIfEmpty()
                        join ac in _context.Accounts on c.AccountId equals ac.AccountId into acGroup
                        from ac in acGroup.DefaultIfEmpty()
                        join wb in _context.Locations on p.OrderId equals wb.OrderId into wbGroup
                        from wb in wbGroup.DefaultIfEmpty()
                        join e in _context.Employees on p.EmployeeId equals e.EmployeeId into eGroup
                        from e in eGroup.DefaultIfEmpty()
                        join ac1 in _context.Accounts on e.AccountId equals ac1.AccountId into ac1Group
                        from ac1 in ac1Group.DefaultIfEmpty()
                        join ac2 in _context.Accounts on p.CancelBy equals ac2.AccountId into ac2Group
                        from ac2 in ac2Group.DefaultIfEmpty()
                        join t in _context.Types on s.TypeId equals t.TypeId into tGroup
                        from t in tGroup.DefaultIfEmpty()
                        where p.IsDeleted == false
                        select new OrderModeDTO
                        {
                            OrderId = p.OrderId,
                            CustomerId = c.CustomerId,
                            AccountId = c.AccountId,
                            Name = ac.Name,
                            Phone = p.Phone,
                            Address = p.Address,
                            Email = ac.Email,
                            BuildingId = p.BuildingId,
                            Dob = ac.Dob,
                            Status = p.Status,
                            Date = (DateTime)p.Date,
                            Note = p.Note,
                            IsDeleted = p.IsDeleted,
                            StartWorking = p.StartWorking,
                            EndWorking = p.EndWorking,
                            CreatedDate = p.CreatedDate,
                            CancelDate = p.CancelDate,
                            CancelBy = p.CancelBy,
                            CancelByName = ac2.Name,
                            CancelByRole = ac2.Role.Name,
                            ReasonCancel = p.ReasonCancel,
                            RatingId = p.RatingId,
                            ServiceId = s.ServiceId,
                            ServiceName = s.Name,
                            CostPerSlot = s.Cost,
                            MinimalSlot = s.MinimalSlot,
                            TypeId = t.TypeId,
                            TypeName = t.Type1,
                            StartTime = p.StarTime,
                            EndTime = p.EndTime,
                            Price = p.Price,
                            SubPrice = p.SubPrice,
                            PointUsed = p.PointUsed,
                            AccountImage = ac.Img,
                            EmployeeImage = ac1.Img,
                            EmployeeId = e.EmployeeId,
                            EmployeeAccountId = e.AccountId,
                            EmployeeName = ac1.Name,
                            EmployeePhone = e.Phone,
                            EmployeeEmail = ac1.Email,
                            Latitude = wb.Latitude,
                            Longtitude = wb.Longtitude,
                        };
            return await query.ToListAsync();
        }
        async Task<Order> IOrderRepository.GetOrderById(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(p => p.OrderId == id);
        }
        async Task<OrderModeDTO> IOrderRepository.GetAllInfoById(int id)
        {
            /*return await _context.Orderes.Include(e=>e.OrderSlots).ThenInclude(p=>p.Slot).FirstOrDefaultAsync(e=>e.OrderId == id);*/
            /*return await _context.Orderes.Where(e=>e.OrderId == id).Include(e=> e.Customer).ThenInclude(c => c.Account)
               .Include(e=>e.OrderSlots).ThenInclude(ps=>ps.Slot)
               .Include(e=>e.OrderDetails).ThenInclude(pd=>pd.Service).ThenInclude(s=>s.Type)
               .Include(e=>e.WorkingBies).ThenInclude(wb=>wb.Employee).FirstOrDefaultAsync();*/

            var query = from p in _context.Orders
                        join s in _context.Services on p.ServiceId equals s.ServiceId into sGroup
                        from s in sGroup.DefaultIfEmpty()
                        join ps in _context.OrderRequests on p.OrderId equals ps.OrderId into psGroup
                        from ps in psGroup.DefaultIfEmpty()
                        join c in _context.Customers on p.CustomerId equals c.CustomerId into cGroup
                        from c in cGroup.DefaultIfEmpty()
                        join ac in _context.Accounts on c.AccountId equals ac.AccountId into acGroup
                        from ac in acGroup.DefaultIfEmpty()
                        join wb in _context.Locations on p.OrderId equals wb.OrderId into wbGroup
                        from wb in wbGroup.DefaultIfEmpty()
                        join e in _context.Employees on wb.EmployeeId equals e.EmployeeId into eGroup
                        from e in eGroup.DefaultIfEmpty()
                        join ac1 in _context.Accounts on e.AccountId equals ac1.AccountId into ac1Group
                        from ac1 in ac1Group.DefaultIfEmpty()
                        join ac2 in _context.Accounts on p.CancelBy equals ac2.AccountId into ac2Group
                        from ac2 in ac2Group.DefaultIfEmpty()
                        join t in _context.Types on s.TypeId equals t.TypeId into tGroup
                        from t in tGroup.DefaultIfEmpty()
                        where p.OrderId == id && p.IsDeleted == false
                        select new OrderModeDTO
                        {
                            OrderId = p.OrderId,
                            CustomerId = c.CustomerId,
                            AccountId = c.AccountId,
                            Name = ac.Name,
                            Phone = p.Phone,
                            Address = p.Address,
                            Email = ac.Email,
                            BuildingId = p.BuildingId,
                            Dob = ac.Dob,
                            Status = p.Status,
                            Date = (DateTime)p.Date,
                            Note = p.Note,
                            IsDeleted = p.IsDeleted,
                            StartWorking = p.StartWorking,
                            EndWorking = p.EndWorking,
                            CreatedDate = p.CreatedDate,
                            CancelDate = p.CancelDate,
                            CancelBy = p.CancelBy,
                            CancelByName = ac2.Name,
                            CancelByRole = ac2.Role.Name,
                            ReasonCancel = p.ReasonCancel,
                            RatingId = p.RatingId,
                            ServiceId = s.ServiceId,
                            ServiceName = s.Name,
                            CostPerSlot = s.Cost,
                            MinimalSlot = s.MinimalSlot,
                            TypeId = t.TypeId,
                            TypeName = t.Type1,
                            StartTime = p.StarTime,
                            EndTime = p.EndTime,
                            Price = p.Price,
                            SubPrice = p.SubPrice,
                            PointUsed = p.PointUsed,
                            AccountImage = ac.Img,
                            EmployeeImage = ac1.Img,
                            EmployeeId = e.EmployeeId,
                            EmployeeAccountId = e.AccountId,
                            EmployeeName = ac1.Name,
                            EmployeePhone = e.Phone,
                            EmployeeEmail = ac1.Email,
                            Latitude = wb.Latitude,
                            Longtitude = wb.Longtitude,
                           
                        };
            return await query.FirstOrDefaultAsync();

            /* var query = _context.Orderes
         .Where(p => p.OrderId == id)
         .Select(p => new OrderModeDTO
         {
             OrderId = p.OrderId,
             CustomerId = p.Customer.CustomerId,
             AccountId = p.Customer.Account.AccountId,
             AccountName = p.Customer.Account.Name,
             CustomerPhone = p.Customer.Phone,
             CustomerAddress = p.Customer.Address,
             OrderStatus = p.Status,
             OrderNote = p.Note,
             IsDeleted = p.IsDeleted,
             CreatedDate = p.CreatedDate,
             ModifiedBy = p.ModifiedBy,
             ServiceId = p.OrderDetails.FirstOrDefault().Service.ServiceId,
             ServiceName = p.OrderDetails.FirstOrDefault().Service.Name,
             CostPerSlot = p.OrderDetails.FirstOrDefault().Service.CostPerSlot,
             MinimalSlot = p.OrderDetails.FirstOrDefault().Service.MinimalSlot,
             TypeId = p.OrderDetails.FirstOrDefault().Service.Type.TypeId,
             TypeName = p.OrderDetails.FirstOrDefault().Service.Type.Type1,
             SlotId = p.OrderSlots.FirstOrDefault().Slot.SlotId,
             SlotName = p.OrderSlots.FirstOrDefault().Slot.SlotName,
             DayOfWeek = p.OrderSlots.FirstOrDefault().Slot.DayOfweek,
             StartTime = p.OrderSlots.FirstOrDefault().Slot.StartTime,
             EndTime = p.OrderSlots.FirstOrDefault().Slot.EndTime,
             TotalMoney = p.Customer.TotalMoney,
             TotalPoint = p.Customer.TotalPoint,
             Employee = new EmployeeInfoDTO
             {
                 EmployeeId = p.WorkingBies.FirstOrDefault().Employee.EmployeeId,
                 AccountId = p.WorkingBies.FirstOrDefault().Employee.Account.AccountId,
                 AccountName = p.WorkingBies.FirstOrDefault().Employee.Account.Name,
                 Phone = p.WorkingBies.FirstOrDefault().Employee.Phone,
                 Email = p.WorkingBies.FirstOrDefault().Employee.Account.Email,
                 Image = p.WorkingBies.FirstOrDefault().Employee.Account.Image
             },
             CustomerImage = p.Customer.Account.Image
         });*/

            /*        }*/
        }

            async Task<bool> IOrderRepository.AddOrder(Order Order)
        {
            _context.Orders.Add(Order);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IOrderRepository.UpdateOrder(Order Order)
        {
            _context.Orders.Update(Order);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IOrderRepository.DeleteOrder(Order Order)
        {
            _context.Orders.Remove(Order);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
        public async Task<ICollection<OrderModeDTO>> SelectOrder(SelectOrder select)
        {
            int employeeId = select.EmployeeId;
            DateTime startDate = DateTime.Parse(select.StartMonth);
            DateTime endDate = DateTime.Parse(select.EndMonth);

            var query = (from o in _context.Orders
                         join c in _context.Customers on o.CustomerId equals c.CustomerId
                         join e in _context.Employees on o.EmployeeId equals e.EmployeeId
                         join s in _context.Services on o.ServiceId equals s.ServiceId
                         join t in _context.Types on s.TypeId equals t.TypeId
                         join ac2 in _context.Accounts on e.AccountId equals ac2.AccountId
                         where (o.Date >= startDate && o.Date <= endDate) && o.EmployeeId == employeeId
                         select new OrderModeDTO
                         {
                             Address = c.Address,
                             Name = c.FirstName + c.LastName,
                             TypeName = t.Type1,
                             SubPrice = o.SubPrice,
                             Phone = c.Phone,
                             PointUsed = (int)o.PointUsed,
                             StartTime = o.StarTime,
                             EndTime = o.EndTime,
                             OrderId = o.OrderId,
                             CreatedDate = o.CreatedDate,
                             StartWorking = o.StartWorking,
                             EndWorking = o.EndWorking,
                             Price = o.Price,
                             ServiceName = s.Name,
                             EmployeeName = e.FirstName + e.LastName,
                             EmployeeId = e.EmployeeId,
                             EmployeeImage = ac2.Img
                         });
            return await query.ToListAsync();
        }

        public async Task<ICollection<OrderModeDTO>> SelectAllOrder(SelectOrder select)
        {
            int employeeId = select.EmployeeId;
            DateTime startDate = DateTime.Parse(select.StartMonth);
            DateTime endDate = DateTime.Parse(select.EndMonth);

            var query = (from o in _context.Orders
                         join c in _context.Customers on o.CustomerId equals c.CustomerId
                         join e in _context.Employees on o.EmployeeId equals e.EmployeeId
                         join s in _context.Services on o.ServiceId equals s.ServiceId
                         join t in _context.Types on s.TypeId equals t.TypeId
                         join ac2 in _context.Accounts on e.AccountId equals ac2.AccountId
                         where (o.Date >= startDate && o.Date <= endDate)
                         select new OrderModeDTO
                         {
                             Address = c.Address,
                             Name = c.FirstName + c.LastName,
                             TypeName = t.Type1,
                             SubPrice = o.SubPrice,
                             Phone = c.Phone,
                             PointUsed = (int)o.PointUsed,
                             StartTime = o.StarTime,
                             EndTime = o.EndTime,
                             OrderId = o.OrderId,
                             CreatedDate = o.CreatedDate,
                             StartWorking = o.StartWorking,
                             EndWorking = o.EndWorking,
                             Price = o.Price,
                             ServiceName = s.Name,
                             EmployeeName = e.FirstName + e.LastName,
                             EmployeeId = e.EmployeeId,
                             EmployeeImage = ac2.Img

                         });
            return await query.ToListAsync();
        }
    }
}

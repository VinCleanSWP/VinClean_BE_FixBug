using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Repo.Models.ProcessModel;

namespace VinClean.Repo.Repository
{
    public interface IEmployeeRepository
    {
        Task<ICollection<Employee>> GetEmployeeList();
        Task<ICollection<Employee>> SearchEmployee(string search);
        Task<Employee> GetEmployeeById(int id);
        Task<bool> CheckEmailEmployeeExist(String email);

        Task<ICollection<Employee>> SelectEmployeeList(String startTime, String endTime, String date);
        Task<bool> AddEmployee(Employee employee);
        Task<bool> DeleteEmployee(Employee employee);
        Task<bool> UpdateEmployee(Employee employee);
        
    }
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ServiceAppContext _context;
        public EmployeeRepository(ServiceAppContext context)
        {
            _context = context;
        }
        async public Task<ICollection<Employee>> SearchEmployee(string search)
        {
            return await _context.Employees.Include(e => e.Account)
                .Where(e => e.Account.Name.Contains(search) || e.EmployeeId.ToString() == search
                    || e.Account.Email.Contains(search) || e.Phone.Contains(search)).ToListAsync();
        }
        async public Task<ICollection<Employee>> GetEmployeeList()
        {
            return await _context.Employees.Include(e=>e.Account).Where(a=>a.Account.IsDeleted == false).ToListAsync();
        }

      

        async public Task<Employee> GetEmployeeById(int id)
        {
            return await _context.Employees.Include(e => e.Account).Where(a => a.Account.IsDeleted == false).FirstOrDefaultAsync(a => a.EmployeeId == id);
        }

        async public Task<bool> CheckEmailEmployeeExist(string email)
        {
            return await _context.Employees.AnyAsync(a => a.Account.Email == email);
        }



        async public Task<bool> AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async public Task<bool> DeleteEmployee(Employee employee)
        {
            _context.Employees.Remove(employee);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async public Task<bool> UpdateEmployee(Employee employee)
        {
            _context.Employees.Update(employee);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<ICollection<Employee>> SelectEmployeeList(String startTime, String endTime, String date)
        {
            TimeSpan startTimeSpan = TimeSpan.Parse(startTime);
            TimeSpan endTimeSpan = TimeSpan.Parse(endTime);
            DateTime dateValue = DateTime.Parse(date);

            //var subquery = (from e in _context.Employees
            //               join wb in _context.WorkingBies on e.EmployeeId equals wb.EmployeeId into workingByJoin
            //               from wb in workingByJoin.DefaultIfEmpty()
            //               join p in _context.Processes on wb.ProcessId equals p.ProcessId into processJoin
            //               from p in processJoin.DefaultIfEmpty()
            //               where (p.StarTime >= startTimeSpan && p.StarTime <= endTimeSpan) ||
            //                     (p.EndTime >= startTimeSpan && p.EndTime <= endTimeSpan) &&
            //                     p.Date == dateValue && e.Status == "Available"
            //               select e.EmployeeId);


            var query = (from e in _context.Employees
                         join a in _context.Accounts on e.AccountId equals a.AccountId into accountJoin
                         from a in accountJoin.DefaultIfEmpty()
                         join o in _context.Orders on e.EmployeeId equals o.EmployeeId into employeeJoin
                         from o in employeeJoin.DefaultIfEmpty()
                         where !(from e in _context.Employees
                                 join p in _context.Orders on e.EmployeeId equals p.EmployeeId into processJoin
                                 from p in processJoin.DefaultIfEmpty()
                                 where ((p.StarTime >= startTimeSpan && p.StarTime <= endTimeSpan) ||
                                       (p.EndTime >= startTimeSpan && p.EndTime <= endTimeSpan)) &&
                                       p.Date == dateValue
                                 select e.EmployeeId).Contains(e.EmployeeId) && a.IsDeleted == false && e.Status == "Available"
                         select new Employee
                         {
                             EmployeeId = e.EmployeeId,
                             FirstName = e.FirstName,
                             LastName = e.LastName,
                             Phone = e.Phone,
                             Status = e.Status,
                             EndDate = e.EndDate,
                             StartDate = e.StartDate,
                             Account = new Account()
                             {
                                 AccountId = a.AccountId,
                                 Email = a.Email,
                                 Name = a.Name,
                                 Img = a.Img,
                                 Gender = a.Gender
                             }
                         }).GroupBy(e => e.EmployeeId).Select(g => g.First());
            return await query.ToListAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

namespace VinClean.Repo.Repository
{
    public interface ICustomerRepository
    {
        Task<ICollection<Customer>> GetCustomerList();
        Task<ICollection<Customer>> SearchNameorId(string search);
        Task<Customer> GetCustomerById(int id);
        Task<Customer> GetCustomerAcById(int id);
        Task<bool> AddCustomer(Customer customer);
        Task<bool> UpdateCustomer(Customer customer);
        bool CheckEmailCustomerExist(String email);

        Task<ICollection<Customer>> GetViewProfileList();
        Task<Customer> GetProfileByID(int id);
        Task<bool> ModifyProfile(Customer customer);
    }
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ServiceAppContext _context;
        public CustomerRepository(ServiceAppContext context)
        {
            _context = context;
        }

        async Task<ICollection<Customer>> ICustomerRepository.GetCustomerList()
        {
            return await _context.Customers.Include(e => e.Account).ToListAsync();
        }
        async Task<ICollection<Customer>> ICustomerRepository.SearchNameorId(string search)
        {
            return await _context.Customers.Include(e => e.Account)
                .Where(e=>e.Account.Name.Contains(search) || e.CustomerId.ToString() == search 
                    || e.Account.Email.Contains(search) || e.Phone.Contains(search)).ToListAsync();
        }
        async Task<Customer> ICustomerRepository.GetCustomerById(int id)
        {
            return await _context.Customers.Include(e => e.Account).FirstOrDefaultAsync(a => a.CustomerId == id);
        }
        async Task<Customer> ICustomerRepository.GetCustomerAcById(int id)
        {
            return await _context.Customers.Include(e => e.Account).FirstOrDefaultAsync(a => a.AccountId == id);
        }

        async Task<bool> ICustomerRepository.AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
        async Task<bool> ICustomerRepository.UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

 
         bool ICustomerRepository.CheckEmailCustomerExist(string email)
        {
            return  _context.Accounts.Any(a => a.Email.Trim() == email.Trim());
        }

        //VIEW PROFILE LIST
        async Task<ICollection<Customer>> ICustomerRepository.GetViewProfileList()
        {
            return await _context.Customers.Include(e => e.Account).ToListAsync();
        }

        //GET PROFILE BY ID
        async Task<Customer> ICustomerRepository.GetProfileByID(int id)
        {
            return await _context.Customers.Include(e => e.Account).FirstOrDefaultAsync(a => a.CustomerId == id);
        }

        //MODIFY PROFILE
        async Task<bool> ICustomerRepository.ModifyProfile(Customer customer)
        {
            _context.Customers.Update(customer);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}

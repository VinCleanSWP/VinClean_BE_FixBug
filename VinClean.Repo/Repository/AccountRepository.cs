using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

namespace VinClean.Repo.Repository
{
        public interface IAccountRepository
        {
            Task<ICollection<Account>> GetAccountList();
            Task<Account> GetAccountById(int id);
            Task<Account> GetToken(string token);
            Task<Account> GetbyEmail(string email);
            Task<Account> GetPasswordResetToken(string token);
            Task<bool> AddAccount(Account account);

            Task<bool> SoftDeleteAccount(int id);
            Task<bool> HardDeleteAccount(Account account);
            Task<bool> UpdateAccount(Account account);
            Task<bool> CheckEmailAccountExist(string email);
            Task<Account> Login(string email, string password);
    }

        public class AccountRepository : IAccountRepository
        {
            private readonly ServiceAppContext _context;
            public AccountRepository(ServiceAppContext context)
            {
                _context = context;
            }

        async Task<ICollection<Account>> IAccountRepository.GetAccountList()
        {
            return await _context.Accounts.ToListAsync();
        }
        async Task<Account> IAccountRepository.GetAccountById(int id)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == id);
        }
        async Task<Account> IAccountRepository.GetbyEmail(string email)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
            return account;
        }

        async Task<Account> IAccountRepository.GetToken(string token)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.VerificationToken == token);
        }

        async Task<Account> IAccountRepository.GetPasswordResetToken(string token)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.PasswordResetToken == token);
        }

        async Task<bool> IAccountRepository.AddAccount(Account account)
        {
             _context.Accounts.Add(account);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IAccountRepository.SoftDeleteAccount(int id)
        {
            var _exisitngAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == id);

            if (_exisitngAccount != null)
            {
                _exisitngAccount.IsDeleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;

        }


        /// <summary>
        /// Đang thử nghiệm. Đừng có làm theo chức năng này
        /// Đang thử nghiệm. Đừng có làm theo chức năng này
        /// Đang thử nghiệm. Đừng có làm theo chức năng này
        /// </summary>
        async Task<bool> IAccountRepository.HardDeleteAccount(Account account)
        {
            var _account = await _context.Accounts.Where(a => a.AccountId == account.AccountId).FirstOrDefaultAsync();
            var customer = await _context.Customers.Where(c => c.AccountId == account.AccountId).FirstOrDefaultAsync();
            var employee = await _context.Employees.Where(c => c.AccountId == account.AccountId).FirstOrDefaultAsync();
            if(account != null && customer != null)
            {
                _context.Customers.Remove(customer);
                _context.Accounts.Remove(_account);
            }
            else if (account != null && employee != null)
            {
                _context.Employees.Remove(employee);
                _context.Accounts.Remove(_account);
            }
            else
            {
                _context.Accounts.Remove(_account);
            }

            return await _context.SaveChangesAsync() > 0 ? true : false;

        }


        async Task<bool> IAccountRepository.UpdateAccount(Account account)
        {
            _context.Accounts.Update(account);
            return await _context.SaveChangesAsync() > 0 ? true : false;

        }
        async Task<bool> IAccountRepository.CheckEmailAccountExist(string email)
        {
            return await _context.Accounts.AnyAsync(a => a.Email == email);
        }

        async Task<Account> IAccountRepository.Login(string email, string password)
        {
            return await _context.Accounts.Include(e => e.Role).FirstOrDefaultAsync(u => u.Email == email&& u.Password == password);
        }

    }

}

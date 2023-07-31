using AutoMapper;
using Azure;
using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Account;


namespace VinClean.Service.Service
{
    public interface IAccountService
    {
        Task<ServiceResponse<List<AccountdDTO>>> GetAccountList();
        Task<ServiceResponse<AccountdDTO>> GetAccountById(int id);
        Task<ServiceResponse<AccountdDTO>> AddAccount(AccountdDTO request);
        Task<ServiceResponse<AccountdDTO>> UpdateAccount(AccountdDTO request);
        Task<ServiceResponse<AccountdDTO>> Verify(string token);
        Task<ServiceResponse<AccountdDTO>> ResetPassword(ResetPasswordReqsuet requset);
        Task<ServiceResponse<AccountdDTO>> ForgotPassword(string email);

        Task<ServiceResponse<AccountdDTO>> SoftDeleteAccount(int id);
        Task<ServiceResponse<AccountdDTO>> HardDeleteAccount(int id);
        Task<ServiceResponse<AccountdDTO>> Login(string email, string password);


    }

    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;
        private readonly IMapper    _mapper;
        public AccountService (IAccountRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<AccountdDTO>>> GetAccountList()
        {
            ServiceResponse<List<AccountdDTO>> _response = new();
            try
            {
                var ListAccount = await _repository.GetAccountList();
                var ListAccountDTO = new List<AccountdDTO>();
                foreach (var account in ListAccount)
                {
                    ListAccountDTO.Add(_mapper.Map<AccountdDTO>(account));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListAccountDTO;
            }catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;

        }

        public async Task<ServiceResponse<AccountdDTO>> GetAccountById(int id)
        {
            ServiceResponse<AccountdDTO> _response = new();
            try
            {
                var account = await _repository.GetAccountById(id);
                if(account == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var accoundto = _mapper.Map<AccountdDTO>(account);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = accoundto;

            }
            catch(Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
                
        }


        public async Task<ServiceResponse<AccountdDTO>> Login(string email, string password)
        {
            ServiceResponse<AccountdDTO> _response = new();
            try
            {
                var account = await _repository.Login(email,password);
                if (account == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var accoundto = _mapper.Map<AccountdDTO>(account);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = accoundto;

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;

        }

        public async Task<ServiceResponse<AccountdDTO>> AddAccount(AccountdDTO request)
        {
            ServiceResponse<AccountdDTO> _response = new();
            try
            {
                if (await _repository.CheckEmailAccountExist(request.Email))
                {
                    _response.Message = "Exist";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }
                Account _newAccount = new Account()
                {
                    Email = request.Email,
                    Password = request.Password,
                    Name = request.Name,
                    RoleId = request.RoleId,
                    Status = request.Status,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                    Dob = request.Dob,
                    Gender = request.Gender,
                    Img = request.Img

                };

                if (!await _repository.AddAccount(_newAccount))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<AccountdDTO>(_newAccount);
                _response.Message = "Created";

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }

            return _response;
        }
        public async Task<ServiceResponse<AccountdDTO>> UpdateAccount(AccountdDTO request)
        {
            ServiceResponse<AccountdDTO> _response = new();
            try
            {
                var existingAccount = await _repository.GetAccountById(request.AccountId);
                if(existingAccount == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                existingAccount.Email = request.Email;
                existingAccount.Password = request.Password;
                existingAccount.Name = request.Name;
                existingAccount.Status = request.Status;
                existingAccount.IsDeleted = request.IsDeleted;
                existingAccount.Dob = request.Dob;
                existingAccount.Gender = request.Gender;
                existingAccount.Img = request.Img;

                if (!await _repository.UpdateAccount(existingAccount))
                {
                    _response.Success = false;
                    _response.Message= "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _accountDTO = _mapper.Map<AccountdDTO>(existingAccount);
                _response.Success = true;
                _response.Data = _accountDTO;
                _response.Message = "Updated";

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }
        public async Task<ServiceResponse<AccountdDTO>> Verify(string token)
        {
            ServiceResponse<AccountdDTO> _response = new();
            try
            {
                var account = await _repository.GetToken(token);
                if (account == null)
                {
                    _response.Success = false;
                    _response.Message = "InvalidToken";
                    _response.Data = null;
                    return _response;
                }

                account.VerifiedAt = DateTime.Now;

                if (!await _repository.UpdateAccount(account))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _accountDTO = _mapper.Map<AccountdDTO>(account);
                _response.Success = true;
                _response.Data = _accountDTO;
                _response.Message = "User verified";

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }

        public async Task<ServiceResponse<AccountdDTO>> ForgotPassword(string email)
        {
            ServiceResponse<AccountdDTO> _response = new();
            try
            {
                var account = await _repository.GetbyEmail(email);
                if (account == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                account.PasswordResetToken = CreateRandomToken();
                account.ResetTokenExpires = DateTime.Now.AddDays(1);

                if (!await _repository.UpdateAccount(account))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _accountDTO = _mapper.Map<AccountdDTO>(account);
                _response.Success = true;
                _response.Data = _accountDTO;
                _response.Message = "User verified";

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }

        public async Task<ServiceResponse<AccountdDTO>> ResetPassword(ResetPasswordReqsuet requset)
        {
            ServiceResponse<AccountdDTO> _response = new();
            try
            {
                var account = await _repository.GetPasswordResetToken(requset.Token);
                if (account == null || account.ResetTokenExpires < DateTime.Now)
                {
                    _response.Success = false;
                    _response.Message = "InvalidToken";
                    _response.Data = null;
                    return _response;
                }

                account.Password = requset.Password;

                if (!await _repository.UpdateAccount(account))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _accountDTO = _mapper.Map<AccountdDTO>(account);
                _response.Success = true;
                _response.Data = _accountDTO;
                _response.Message = "Reset Password Successfully";

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }


        public async Task<ServiceResponse<AccountdDTO>> SoftDeleteAccount(int id)

        {
            ServiceResponse<AccountdDTO> _response = new();
            try
            {
                var existingAccount = await _repository.GetAccountById(id);
                if (existingAccount == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }


                if(!await _repository.SoftDeleteAccount(id))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    return _response;
                }

                _response.Success = true;
                _response.Message = "SoftDeleted";

        }
            catch(Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message)
    };
}
            return _response;
        }


        /// <summary>
        /// Đang thử nghiệm. Đừng có làm theo chức năng này
        /// Đang thử nghiệm. Đừng có làm theo chức năng này
        /// Đang thử nghiệm. Đừng có làm theo chức năng này
        /// </summary>
        public async Task<ServiceResponse<AccountdDTO>> HardDeleteAccount(int id)
        {
            ServiceResponse<AccountdDTO> _response = new();
            //try
            //{
                var existingAccount = await _repository.GetAccountById(id);
                if (existingAccount == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                
                if (!await _repository.HardDeleteAccount(existingAccount))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    return _response;
                }

                _response.Success = true;
                _response.Message = "HardDeleted";

            //}
            //catch (Exception ex)
            //{
            //    _response.Success = false;
            //    _response.Message = "error";
            //    _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message)};

            //}
            return _response;
        }
        


        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }



    }
}

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using VinClean.Service.DTO.CustomerResponse;

namespace VinClean.Service.Service
{
    public interface ICustomerService
    {
        Task<ServiceResponse<List<CustomerDTO>>> GetCustomerList();
        Task<ServiceResponse<List<CustomerDTO>>> SearchNameorId(string search);
        Task<ServiceResponse<CustomerDTO>> GetCustomerById(int id);
        Task<ServiceResponse<CustomerDTO>> GetCustomerAcById(int id);
        Task<ServiceResponse<CustomerDTO>> Register(RegisterDTO request);
        Task<ServiceResponse<CustomerDTO>> UpdateCustomer(UpdateDTO request);

        /*        Task<ServiceResponse<RegisterDTO>> DeleteCustomer(int id);*/

        Task<ServiceResponse<List<CustomerProfileDTO>>> GetViewProfileList();
        Task<ServiceResponse<CustomerProfileDTO>> GetProfileByID(int id);
        Task<ServiceResponse<CustomerProfileDTO>> ModifyProfile(ModifyCustomerProfileDTO request);
    }
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        public CustomerService(ICustomerRepository customertRepository, IAccountRepository accountRepository, IMapper mapper ) 
        { 
            _accountRepository = accountRepository;
            _customerRepository = customertRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<CustomerDTO>>> GetCustomerList()
        {
            ServiceResponse<List<CustomerDTO>> _response = new();
            try
            {
                var ListCustomer = await _customerRepository.GetCustomerList();
                var ListCustomerDTO = new List<CustomerDTO>();
                foreach (var customer in ListCustomer)
                {
/*                    var account = await _accountRepository.GetAccountById((int)customer.AccountId);*/ // Get the account for the customer
                    var customerDTO = _mapper.Map<CustomerDTO>(customer);
/*                    customerDTO.Account = _mapper.Map<AccountdDTO>(account);*/ // Map the account information to the DTO
                    ListCustomerDTO.Add(customerDTO);

                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListCustomerDTO;
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


        public async Task<ServiceResponse<List<CustomerDTO>>> SearchNameorId(string search)
        {
            ServiceResponse<List<CustomerDTO>> _response = new();
            /*try
            {*/
                var ListCustomer = await _customerRepository.SearchNameorId(search);
                var ListCustomerDTO = new List<CustomerDTO>();
                foreach (var customer in ListCustomer)
                {
                    /*                    var account = await _accountRepository.GetAccountById((int)customer.AccountId);*/ // Get the account for the customer
                    var customerDTO = _mapper.Map<CustomerDTO>(customer);
                    /*                    customerDTO.Account = _mapper.Map<AccountdDTO>(account);*/ // Map the account information to the DTO
                    ListCustomerDTO.Add(customerDTO);

                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListCustomerDTO;
            /*}
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }*/
            return _response;

        }
        public async Task<ServiceResponse<CustomerDTO>> GetCustomerById(int id)
        {
            ServiceResponse<CustomerDTO> _response = new();
            try
            {
                var customer = await _customerRepository.GetCustomerById(id);
                if (customer == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = _mapper.Map<CustomerDTO>(customer);


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

        public async Task<ServiceResponse<CustomerDTO>> GetCustomerAcById(int id)
        {
            ServiceResponse<CustomerDTO> _response = new();
            try
            {
                var customer = await _customerRepository.GetCustomerAcById(id);
                if (customer == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = _mapper.Map<CustomerDTO>(customer);


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

        public async Task<ServiceResponse<CustomerDTO>> Register(RegisterDTO request)
        {
            ServiceResponse<CustomerDTO> _response = new();
            try
            {
                if ( _customerRepository.CheckEmailCustomerExist(request.Email))
                {
                    _response.Message = "Exist";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }
                var _newAccount = new Account
                {
                    Name = request.FirstName + " " + request.LastName,
                    Password = request.Password,
                    Email = request.Email,
                    RoleId = 1, // assign a default role for new accounts
                    Status = "Active", // set the status to active by default
                    IsDeleted =  false, // set the isDeleted flag to false by default
                    CreatedDate = DateTime.Now, // set the created date to the current date/time
                    VerificationToken =  CreateRandomToken()
                };
                await _accountRepository.AddAccount(_newAccount);

                var _newcustomer = new Customer
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Phone = request.Phone,
                    Address = request.Address,
                    AccountId = _newAccount.AccountId,
                    Status = "Active" // set the status to active by default
                };
                if (!await _customerRepository.AddCustomer(_newcustomer))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }   
                _response.Success = true;
                _response.Data = _mapper.Map<CustomerDTO>(await _customerRepository.GetCustomerById(_newcustomer.CustomerId));
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

        public async Task<ServiceResponse<CustomerDTO>> UpdateCustomer(UpdateDTO request)
        {
            ServiceResponse<CustomerDTO> _response = new();
            try
            {
                var existingCustomer = await _customerRepository.GetCustomerById(request.CustomerId);
                if (existingCustomer == null)
                {
                    _response.Message = "NotFound";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }
                var _newAccount = await _accountRepository.GetAccountById(existingCustomer.Account.AccountId);
                _newAccount.Name = request.FirstName + " " + request.LastName;
                _newAccount.Password = request.Password;
                _newAccount.Email = request.Email;
                _newAccount.Gender = request.Gender;
                _newAccount.Img = request.Img;
                _newAccount.Dob = request.Dob;
                await _accountRepository.UpdateAccount(_newAccount);

                existingCustomer.FirstName = request.FirstName;
                existingCustomer.LastName = request.LastName;
                existingCustomer.Phone = request.Phone;
                existingCustomer.Address = request.Address;


                if (!await _customerRepository.UpdateCustomer(existingCustomer))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<CustomerDTO>(await _customerRepository.GetCustomerById(existingCustomer.CustomerId));
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

        //VIEW PROFILE LIST
        public async Task<ServiceResponse<List<CustomerProfileDTO>>> GetViewProfileList()
        {
            ServiceResponse<List<CustomerProfileDTO>> response = new();
            try
            {
                var profileList = await _customerRepository.GetViewProfileList();
                var viewProfileList = new List<CustomerProfileDTO>();

                foreach (var profile in profileList)
                {
                    var viewProfileDTO = _mapper.Map<CustomerProfileDTO>(profile);
                    // Additional mapping or processing specific to view profiles

                    viewProfileList.Add(viewProfileDTO);
                }

                response.Success = true;
                response.Message = "OK";
                response.Data = viewProfileList;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error";
                response.Data = null;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        //GET PROFILE BY ID
        public async Task<ServiceResponse<CustomerProfileDTO>> GetProfileByID(int id)
        {
            ServiceResponse<CustomerProfileDTO> _response = new();
            try
            {
                var profile = await _customerRepository.GetProfileByID(id);
                if (profile == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = _mapper.Map<CustomerProfileDTO>(profile);


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


        //MODIFY PROFILE
        public async Task<ServiceResponse<CustomerProfileDTO>> ModifyProfile(ModifyCustomerProfileDTO request)
        {
            ServiceResponse<CustomerProfileDTO> _response = new();
            try
            {
                var modifypCustomer = await _customerRepository.GetProfileByID(request.CustomerId);

                if (modifypCustomer == null)
                {
                    _response.Message = "NotFound";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                var _editAccount = await _accountRepository.GetAccountById(modifypCustomer.Account.AccountId);
                _editAccount.Email = request.Email;
                _editAccount.Password = request.Password;
                await _accountRepository.UpdateAccount(_editAccount);

                modifypCustomer.FirstName = request.FirstName;
                modifypCustomer.LastName = request.LastName;
                modifypCustomer.Phone = request.Phone;
                modifypCustomer.Address = request.Address;
                modifypCustomer.TotalPoint = request.TotalPoint;


                if (!await _customerRepository.ModifyProfile(modifypCustomer))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }
                _response.Success = true;
                _response.Data = _mapper.Map<CustomerProfileDTO>(await _customerRepository.GetProfileByID(modifypCustomer.CustomerId));
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



        //private string CreateRandomToken()
        //{
        //    return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        //}
        private string CreateRandomToken()
        {
            var random = new Random();
            var token = random.Next(10000000, 99999999).ToString();
            return token;
        }



    }
}

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Service;

namespace VinClean.Service.Service
{
    public interface IServiceManageService
    {
        Task<ServiceResponse<List<ServiceManageDTO>>> GetServiceManageList();
        Task<ServiceResponse<ServiceManageDTO>> GetServiceManageById(int id);
        Task<ServiceResponse<ServiceManageDTO>> CreateServiceManage(ServiceManageDTO request);
        Task<ServiceResponse<ServiceManageDTO>> UpdateServiceManage(ServiceManageDTO request);
        Task<ServiceResponse<ServiceManageDTO>> DeleteServiceManage(int id);
    }
    public class ServiceManageService : IServiceManageService
    {
        private readonly IServiceWorkInRepository _repository;
        private readonly IMapper _mapper;

        public ServiceManageService(IServiceWorkInRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        async Task<ServiceResponse<ServiceManageDTO>> IServiceManageService.CreateServiceManage(ServiceManageDTO request)
        {
            ServiceResponse<ServiceManageDTO> _response = new();
            try
            {

                ServiceWorkIn _newServiceManage = new ServiceWorkIn()
                {
                    StartDate = request.StartDate

                };

                if (!await _repository.CreateServiceWorkIn(_newServiceManage))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<ServiceManageDTO>(_newServiceManage);
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

        async Task<ServiceResponse<ServiceManageDTO>> IServiceManageService.DeleteServiceManage(int id)
        {
            ServiceResponse<ServiceManageDTO> _response = new();
            try
            {
                var existingServiceManage = await _repository.GetServiceWorkInById(id);
                if (existingServiceManage == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                if (!await _repository.DeleteServiceWorkIn(existingServiceManage))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _ServiceManageDTO = _mapper.Map<ServiceManageDTO>(existingServiceManage);
                _response.Success = true;
                _response.Data = _ServiceManageDTO;
                _response.Message = "Deleted";

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

        async Task<ServiceResponse<ServiceManageDTO>> IServiceManageService.GetServiceManageById(int id)
        {
            ServiceResponse<ServiceManageDTO> _response = new();
            try
            {
                var ServiceManage = await _repository.GetServiceWorkInById(id);
                if (ServiceManage == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var ServiceManagedto = _mapper.Map<ServiceManageDTO>(ServiceManage);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ServiceManagedto;

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

        async Task<ServiceResponse<List<ServiceManageDTO>>> IServiceManageService.GetServiceManageList()
        {
            ServiceResponse<List<ServiceManageDTO>> _response = new();
            try
            {
                var ListServiceManage = await _repository.GetServiceWorkInList();
                var ListServiceManageDTO = new List<ServiceManageDTO>();
                foreach (var ServiceManage in ListServiceManage)
                {
                    ListServiceManageDTO.Add(_mapper.Map<ServiceManageDTO>(ServiceManage));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListServiceManageDTO;
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



        async Task<ServiceResponse<ServiceManageDTO>> IServiceManageService.UpdateServiceManage(ServiceManageDTO request)
        {
            ServiceResponse<ServiceManageDTO> _response = new();
            try
            {
                var existingServiceManage = await _repository.GetServiceWorkInById(request.ServiceId);
                if (existingServiceManage == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                existingServiceManage.EmployeeId = request.EmployeeId;
                existingServiceManage.ServiceId = request.ServiceId;
                existingServiceManage.StartDate = request.StartDate;


                if (!await _repository.UpdateServiceWorkIn(existingServiceManage))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _ServiceManageDTO = _mapper.Map<ServiceManageDTO>(existingServiceManage);
                _response.Success = true;
                _response.Data = _ServiceManageDTO;
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
    }
}

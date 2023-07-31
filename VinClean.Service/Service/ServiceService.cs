using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Repo.Repository;
using VinClean.Service.DTO.Service;
using VinClean.Service.DTO;

namespace VinClean.Service.Service
{
    public interface IServiceService
    {
        Task<ServiceResponse<List<ServiceDTO>>> GetServiceList();
        Task<ServiceResponse<List<ServiceDTO>>> GetServiceListById(int id);
        Task<ServiceResponse<ServiceDTO>> GetServiceById(int id);
        Task<ServiceResponse<ServiceDTO>> AddService(newServiceDTO request);
        Task<ServiceResponse<ServiceDTO>> UpdateService(ServiceDTO request);
        Task<ServiceResponse<ServiceDTO>> DeleteService(int id);

    }
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _repository;
        private readonly IMapper _mapper;

        public ServiceService(IServiceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<ServiceDTO>> AddService(newServiceDTO request)
        {
            ServiceResponse<ServiceDTO> _response = new();
            try
            {
                Repo.Models.Service _newService = new Repo.Models.Service()
                {
                    Name = request.Name,
                    Cost = request.Cost,
                    TypeId = request.TypeId,
                    MinimalSlot = 1,
                    Description = request.Description,
                    Status = "Available",
                    Avaiable = true,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,

                };

                if (!await _repository.AddService(_newService))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<ServiceDTO>(_newService);
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

        public async Task<ServiceResponse<ServiceDTO>> DeleteService(int id)
        {
            ServiceResponse<ServiceDTO> _response = new();
            try
            {
                var existingService = await _repository.GetServiceById(id);
                if (existingService == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                if (!await _repository.DeleteService(existingService))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _ServiceDTO = _mapper.Map<ServiceDTO>(existingService);
                _response.Success = true;
                _response.Data = _ServiceDTO;
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

        public async Task<ServiceResponse<ServiceDTO>> GetServiceById(int id)
        {
            ServiceResponse<ServiceDTO> _response = new();
            try
            {
                var Service = await _repository.GetServiceById(id);
                if (Service == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var Servicedto = _mapper.Map<ServiceDTO>(Service);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = Servicedto;

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

        public async Task<ServiceResponse<List<ServiceDTO>>> GetServiceList()
        {
            ServiceResponse<List<ServiceDTO>> _response = new();
            try
            {
                var ListService = await _repository.GetServiceList();
                var ListServiceDTO = new List<ServiceDTO>();
                foreach (var Service in ListService)
                {
                    ListServiceDTO.Add(_mapper.Map<ServiceDTO>(Service));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListServiceDTO;
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


        public async Task<ServiceResponse<List<ServiceDTO>>> GetServiceListById(int id)
        {
            ServiceResponse<List<ServiceDTO>> _response = new();
            try
            {
                var ListService = await _repository.GetServiceListById(id);
                var ListServiceDTO = new List<ServiceDTO>();
                foreach (var Service in ListService)
                {
                    ListServiceDTO.Add(_mapper.Map<ServiceDTO>(Service));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListServiceDTO;
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
        public async Task<ServiceResponse<ServiceDTO>> UpdateService(ServiceDTO request)
        {
            ServiceResponse<ServiceDTO> _response = new();
            try
            {
                var existingService = await _repository.GetServiceById(request.ServiceId);
                if (existingService == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }
                // cac gia trị cho sua
                existingService.Name = request.Name;
                existingService.Cost = request.Cost;
                existingService.MinimalSlot = request.MinimalSlot;
                existingService.Description = request.Description;
                existingService.Status = request.Status;                
                ///Name 
                ///CostPerSlot 
                ///MinimalSlot 
                ///Description 
                ///Status 
                ///Avaiable 
                ///IsDeleted 
                ///CreatedDate 


                if (!await _repository.UpdateService(existingService))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _ServiceDTO = _mapper.Map<ServiceDTO>(existingService);
                _response.Success = true;
                _response.Data = _ServiceDTO;
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

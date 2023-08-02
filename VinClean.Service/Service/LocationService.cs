using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Order;
using VinClean.Service.DTO.WorkingBy;
using VinClean.Service.DTO.WorkingSlot;

namespace VinClean.Service.Service
{
    public interface IWorkingByService
    {
        Task<ServiceResponse<List<LocationDTO>>> GetWBList();
        Task<ServiceResponse<LocationDTO>> GetWBById(int id);
        Task<ServiceResponse<LocationDTO>> GetWBByProcessId(int id);
        Task<ServiceResponse<LocationDTO>> DeleteWB(int id);
        Task<ServiceResponse<LocationDTO>> AddWB(LocationDTO request);
        Task<ServiceResponse<LocationDTO>> UpdateWB(LocationDTO request);
        Task<ServiceResponse<DTO.WorkingBy.UpdateLocation>> UpdateLocation(UpdateLocation request);
        Task<ServiceResponse<LocationDTO>> AcceptRequest(LocationDTO request);
    }
    public class LocationService : IWorkingByService
    {
        private readonly ILocationRepository _repository;
        private readonly IOrderRequestRepository _PRrepository;
        private readonly IOrderRepository _Orderrepository;
        private readonly IMapper _mapper;
        public LocationService(ILocationRepository repository, IMapper mapper, IOrderRequestRepository pRrepository, IOrderRepository orderrepository)
        {
            _repository = repository;
            _mapper = mapper;
            _PRrepository = pRrepository;
            _Orderrepository = orderrepository;
        }

        public async Task<ServiceResponse<List<LocationDTO>>> GetWBList()
        {
            ServiceResponse<List<LocationDTO>> _response = new();
            try
            {
                var ListWslot = await _repository.GetLocationList();
                var ListWslotDTO = new List<LocationDTO>();
                foreach (var WSlot in ListWslot)
                {
                    ListWslotDTO.Add(_mapper.Map<LocationDTO>(WSlot));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListWslotDTO;
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

        public async Task<ServiceResponse<LocationDTO>> GetWBById(int id)
        {
            ServiceResponse<LocationDTO> _response = new();
            try
            {
                var WSlot = await _repository.GetLocationById(id);
                if (WSlot == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var WSlotDTO = _mapper.Map<LocationDTO>(WSlot);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = WSlotDTO;

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
        public async Task<ServiceResponse<LocationDTO>> GetWBByProcessId(int id)
        {
            ServiceResponse<LocationDTO> _response = new();
            try
            {
                var WSlot = await _repository.GetLocationByOrderId(id);
                if (WSlot == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var WSlotDTO = _mapper.Map<LocationDTO>(WSlot);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = WSlotDTO;

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

        public async Task<ServiceResponse<LocationDTO>> UpdateWB(LocationDTO request)
        {
            ServiceResponse<LocationDTO> _response = new();
            try
            {
                var existingWSlot = await _repository.GetLocationById(request.EmployeeId);
                if (existingWSlot == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }
                existingWSlot.EmployeeId = request.EmployeeId;

                if (!await _repository.UpdateLocation(existingWSlot))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _WSlotDTO = _mapper.Map<LocationDTO>(existingWSlot);
                _response.Success = true;
                _response.Data = _WSlotDTO;
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


        public async Task<ServiceResponse<DTO.WorkingBy.UpdateLocation>> UpdateLocation(DTO.WorkingBy.UpdateLocation request)
        {
            ServiceResponse<DTO.WorkingBy.UpdateLocation> _response = new();
            //try
            //{
                var existingWSlot = await _repository.GetLocationByOrderId(request.ProcessId);
                if (existingWSlot == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }
                existingWSlot.Latitude = request.Latitude;
                existingWSlot.Longtitude = request.Longtitude;
                if (!await _repository.UpdateLocation(existingWSlot))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _WSlotDTO = _mapper.Map<DTO.WorkingBy.UpdateLocation>(existingWSlot);
                _response.Success = true;
                _response.Data = _WSlotDTO;
                _response.Message = "Updated";

            //}
            //catch (Exception ex)
            //{
            //    _response.Success = false;
            //    _response.Data = null;
            //    _response.Message = "Error";
            //    _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            //}
            return _response;
        }


        public async Task<ServiceResponse<LocationDTO>> AcceptRequest(LocationDTO request)
        {
            ServiceResponse<LocationDTO> _response = new();
            try
            {
                var existingWSlot = await _repository.GetLocationByOrderId(request.OrderId);
                var existingOrder = await _Orderrepository.GetOrderById(request.OrderId);
                if (existingWSlot == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }
                existingWSlot.EmployeeId = request.EmployeeId;
                existingOrder.EmployeeId = request.EmployeeId;

                var check1 = await _repository.UpdateLocation(existingWSlot);
                var check3 = await _Orderrepository.UpdateOrder(existingOrder);


                //Update ProcessRequest
                var existingProcess = await _PRrepository.GetPSById(request.OrderId);
                if (existingProcess == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }
                existingProcess.Satus = "Accepted";
                existingProcess.NewEmployeeId = request.EmployeeId;
                var check2 = await _PRrepository.UpdatePS(existingProcess);
                //Update ProcessRequest

                if (!check1 && !check2 && !check3)
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _WSlotDTO = _mapper.Map<LocationDTO>(existingWSlot);
                _response.Success = true;
                _response.Data = _WSlotDTO;
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

        public async Task<ServiceResponse<LocationDTO>> AddWB(LocationDTO request)
        {
            ServiceResponse<LocationDTO> _response = new();
            try
            {
                if (request != null)
                {
                    Location _newWB = new Location()
                    {
                        OrderId = request.OrderId,
                        EmployeeId = request.EmployeeId,

                    };
                    //AssignEmployeeDTO assign = new AssignEmployeeDTO()
                    //{
                    //    OrderId = request.OrderId,
                    //    EmployeeId = request.EmployeeId
                    //};
                    if (!await _repository.AddLocation(_newWB))
                    {
                        _response.Error = "RepoError";
                        _response.Success = false;
                        _response.Data = null;
                        return _response;
                    }

                    _response.Success = true;
                    _response.Data = _mapper.Map<LocationDTO>(_newWB);
                    _response.Message = "Created";
                } 
         
        }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message)
    };
}

return _response;
        }

        public async Task<ServiceResponse<LocationDTO>> DeleteWB(int id)

        {
            ServiceResponse<LocationDTO> _response = new();
            try
            {
                var existingWB = await _repository.GetLocationByOrderId(id);
                if (existingWB == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }


                if (!await _repository.DeleteLocation(existingWB))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    return _response;
                }

                _response.Success = true;
                _response.Message = "SoftDeleted";

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message)
    };
            }
            return _response;
        }
    }
}

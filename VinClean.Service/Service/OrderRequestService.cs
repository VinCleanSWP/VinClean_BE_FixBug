using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using VinClean.Service.DTO.CustomerResponse;
using VinClean.Service.DTO.Employee;
using VinClean.Service.DTO.Process;
using VinClean.Service.DTO.Slot;

namespace VinClean.Service.Service
{
    public interface IProcessSlotService
    {
        Task<ServiceResponse<List<OrderRequestModel>>> GetPS();
        Task<ServiceResponse<OrderRequestModel>> GetPSById(int id);
        Task<ServiceResponse<OrderRequestDTO>> CreatePS(AddOrderRequest processSlotDTO);
        Task<ServiceResponse<OrderRequestDTO>> UpdatePS(OrderRequestDTO processSlotDTO);
        Task<ServiceResponse<OrderRequestDTO>> CancelRequest(int i);
        Task<ServiceResponse<OrderRequestDTO>> DeletePS(int id);
    }
    public class OrderRequestService : IProcessSlotService
    {
        private readonly IOrderRequestRepository _repository;
        public readonly IMapper _mapper;

        public OrderRequestService(IOrderRequestRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<OrderRequestModel>>> GetPS()
        {
            ServiceResponse<List<OrderRequestModel>> _response = new();
            try
            {
                var processSlots = await _repository.GetPSList();
                var processSlotDTOs = new List<OrderRequestModel>();

                foreach (var processSlot in processSlots)
                {
                    var processSlotDTO = _mapper.Map<OrderRequestModel>(processSlot);


                    processSlotDTOs.Add(processSlotDTO);
                }

                _response.Success = true;
                _response.Message = "OK";
                _response.Data = processSlotDTOs;
        }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message)
    };
}
return _response;
        }

        public async Task<ServiceResponse<OrderRequestModel>> GetPSById(int id)
        {
            ServiceResponse<OrderRequestModel> _response = new();
            try
            {
                var process = await _repository.GetInfoPSById(id);
                if (process == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var processSlotDTO = _mapper.Map<OrderRequestModel>(process);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = processSlotDTO;

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

        public async Task<ServiceResponse<OrderRequestDTO>> CreatePS(AddOrderRequest request)
        {
            ServiceResponse<OrderRequestDTO> _response = new();
            /*try
            {*/
            OrderRequest _newProcess = new OrderRequest()
            {
                OrderId = request.ProcessId,
                CreateBy = request.CreateBy,
                OldEmployeeId = request.OldEmployeeId,
                Note = request.Note,
                CreateAt = request.CreateAt,
                Satus = "Waiting"
            };
            if (!await _repository.AddPS(_newProcess))
            {
                _response.Error = "RepoError";
                _response.Success = false;
                _response.Data = null;
                return _response;
            }


            _response.Success = true;
            _response.Data = _mapper.Map<OrderRequestDTO>(_newProcess);
            _response.Message = "Created";

            /*}
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }*/

            return _response;
        }

        public async Task<ServiceResponse<OrderRequestDTO>> UpdatePS(OrderRequestDTO request)
        {
            ServiceResponse<OrderRequestDTO> _response = new();
            try
            {
                var existingProcess = await _repository.GetPSById(request.ProcessId);
                if (existingProcess == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                existingProcess.OrderId = request.ProcessId;

                if (!await _repository.UpdatePS(existingProcess))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _processSlotDTO = _mapper.Map<OrderRequestDTO>(existingProcess);
                _response.Success = true;
                _response.Data = _processSlotDTO;
                _response.Message = "Process Updated";

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

        public async Task<ServiceResponse<OrderRequestDTO>> CancelRequest(int id)
        {
            ServiceResponse<OrderRequestDTO> _response = new();
            try
            {
                var existingProcess = await _repository.GetPSById(id);
                if (existingProcess == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                existingProcess.Satus = "Denied";

                if (!await _repository.UpdatePS(existingProcess))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _processSlotDTO = _mapper.Map<OrderRequestDTO>(existingProcess);
                _response.Success = true;
                _response.Data = _processSlotDTO;
                _response.Message = "Process Updated";

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



        public async Task<ServiceResponse<OrderRequestDTO>> DeletePS(int id)
        {
            ServiceResponse<OrderRequestDTO> _response = new();
            try
            {
                var existingProcess = await _repository.GetPSById(id);
                if (existingProcess == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                if (!await _repository.DeletePS(existingProcess))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _processSlotDTO = _mapper.Map<OrderRequestDTO>(existingProcess);
                _response.Success = true;
                _response.Data = _processSlotDTO;
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
    }
}

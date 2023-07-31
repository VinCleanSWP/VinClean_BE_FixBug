using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Service.DTO.WorkingSlot;
using VinClean.Service.DTO;
using VinClean.Service.DTO.ProcessImage;
using AutoMapper;
using VinClean.Repo.Repository;
using VinClean.Repo.Models;
using VinClean.Service.DTO.CustomerResponse;

namespace VinClean.Service.Service
{
    public interface IProcessImageService
    {
        Task<ServiceResponse<List<ProcessImageDTO>>> ProcessImageList();
        Task<ServiceResponse<ProcessImageDTO>> ProcessImageById(int id);
        Task<ServiceResponse<List<ProcessImageDTO>>> ProcessImageByProcessId(int id);
        Task<ServiceResponse<ProcessImageDTO>> DeleteProcessImage(int id);
        Task<ServiceResponse<ProcessImageDTO>> AddProcessImage(ProcessImageDTO request);
        Task<ServiceResponse<ProcessImageDTO>> UpdateProcessImage(ProcessImageDTO request);
        Task<ServiceResponse<ProcessImageDTO>> UpdateImage(UpdateImage request);
    }
    public class OrderImageService : IProcessImageService
    {
        private readonly IOrderImageRepository _repository;
        private readonly IMapper _mapper;
        public OrderImageService(IOrderImageRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<ProcessImageDTO>>> ProcessImageList()
        {
            ServiceResponse<List<ProcessImageDTO>> _response = new();
            try
            {
                var ListProcessImage = await _repository.OrderImageList();
                var ListProcessImageDTO = new List<ProcessImageDTO>();
                foreach (var ProcessImage in ListProcessImage)
                {
                    var ProcessImageDTO = _mapper.Map<ProcessImageDTO>(ProcessImage);
                    ListProcessImageDTO.Add(ProcessImageDTO);
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListProcessImageDTO;
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

        public async Task<ServiceResponse<ProcessImageDTO>> ProcessImageById(int id)
        {
            ServiceResponse<ProcessImageDTO> _response = new();
            try
            {
                var ProcessImage = await _repository.OrderImageById(id);
                if (ProcessImage == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var ProcessImageDTO = _mapper.Map<ProcessImageDTO>(ProcessImage);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ProcessImageDTO;
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

        public async Task<ServiceResponse<List<ProcessImageDTO>>> ProcessImageByProcessId(int id)
        {
            ServiceResponse<List<ProcessImageDTO>> _response = new();
            try
            {
                var ListProcessImage = await _repository.OrderImageListByProcessId(id);
                var ListProcessImageDTO = new List<ProcessImageDTO>();
                foreach (var ProcessImage in ListProcessImage)
                {
                    var ProcessImageDTO = _mapper.Map<ProcessImageDTO>(ProcessImage);
                    ListProcessImageDTO.Add(ProcessImageDTO);
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListProcessImageDTO;
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

        public async Task<ServiceResponse<ProcessImageDTO>> AddProcessImage(ProcessImageDTO request)
        {
            ServiceResponse<ProcessImageDTO> _response = new();
            try
            {
                OrderImage _newPImage = new OrderImage()
                {
                    OrderId = request.OrderId,
                    Name = request.Name,
                    Type = request.Type,
                    Image = request.Image
                };

                if (!await _repository.AddOrderImage(_newPImage))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _processImage = _mapper.Map<ProcessImageDTO>(_newPImage);
                _response.Success = true;
                _response.Data = _processImage;
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

        public async Task<ServiceResponse<ProcessImageDTO>> UpdateProcessImage(ProcessImageDTO request)
        {
            ServiceResponse<ProcessImageDTO> _response = new();
            try
            {
                var ProcessImage = await _repository.OrderImageById(request.Id);
                if (ProcessImage == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }
                ProcessImage.OrderId = request.OrderId;
                ProcessImage.Name = request.Name;
                ProcessImage.Type = request.Type;
                ProcessImage.Image = request.Image;

                if (!await _repository.UpdateOrderImage(ProcessImage))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _processImage = _mapper.Map<ProcessImageDTO>(ProcessImage);
                _response.Success = true;
                _response.Data = _processImage;
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
        public async Task<ServiceResponse<ProcessImageDTO>> UpdateImage(UpdateImage request)
        {
            ServiceResponse<ProcessImageDTO> _response = new();
            try
            {
                var ProcessImage = await _repository.OrderImageById(request.Id);
                if (ProcessImage == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }
                ProcessImage.Image = request.Image;

                if (!await _repository.UpdateOrderImage(ProcessImage))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _processImage = _mapper.Map<ProcessImageDTO>(ProcessImage);
                _response.Success = true;
                _response.Data = _processImage;
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

        public async Task<ServiceResponse<ProcessImageDTO>> DeleteProcessImage(int id)

        {
            ServiceResponse<ProcessImageDTO> _response = new();
            try
            {
                var ProcessImage = await _repository.OrderImageById(id);
                if (ProcessImage == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }


                if (!await _repository.DeleteOrderImage(ProcessImage))
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
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message)};
            }
            return _response;
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Service.DTO.WorkingSlot;
using VinClean.Service.DTO;
using AutoMapper;
using VinClean.Repo.Repository;
using VinClean.Repo.Models;
using VinClean.Service.DTO.CustomerResponse;
using VinClean.Service.DTO.ProcessImage;

namespace VinClean.Service.Service
{
    public interface IOrderImageService
    {
        Task<ServiceResponse<List<OrderImageDTO>>> OrderImageList();
        Task<ServiceResponse<OrderImageDTO>> OrderImageById(int id);
        Task<ServiceResponse<List<OrderImageDTO>>> OrderImageByOrderId(int id);
        Task<ServiceResponse<OrderImageDTO>> DeleteOrderImage(int id);
        Task<ServiceResponse<OrderImageDTO>> AddOrderImage(OrderImageDTO request);
        Task<ServiceResponse<OrderImageDTO>> UpdateOrderImage(OrderImageDTO request);
        Task<ServiceResponse<OrderImageDTO>> UpdateImage(UpdateImage request);
    }
    public class OrderImageService : IOrderImageService
    {
        private readonly IOrderImageRepository _repository;
        private readonly IMapper _mapper;
        public OrderImageService(IOrderImageRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<OrderImageDTO>>> OrderImageList()
        {
            ServiceResponse<List<OrderImageDTO>> _response = new();
            try
            {
                var ListOrderImage = await _repository.OrderImageList();
                var ListOrderImageDTO = new List<OrderImageDTO>();
                foreach (var OrderImage in ListOrderImage)
                {
                    var OrderImageDTO = _mapper.Map<OrderImageDTO>(OrderImage);
                    ListOrderImageDTO.Add(OrderImageDTO);
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListOrderImageDTO;
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

        public async Task<ServiceResponse<OrderImageDTO>> OrderImageById(int id)
        {
            ServiceResponse<OrderImageDTO> _response = new();
            try
            {
                var OrderImage = await _repository.OrderImageById(id);
                if (OrderImage == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var OrderImageDTO = _mapper.Map<OrderImageDTO>(OrderImage);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = OrderImageDTO;
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

        public async Task<ServiceResponse<List<OrderImageDTO>>> OrderImageByOrderId(int id)
        {
            ServiceResponse<List<OrderImageDTO>> _response = new();
            try
            {
                var ListOrderImage = await _repository.OrderImageListByProcessId(id);
                var ListOrderImageDTO = new List<OrderImageDTO>();
                foreach (var OrderImage in ListOrderImage)
                {
                    var OrderImageDTO = _mapper.Map<OrderImageDTO>(OrderImage);
                    ListOrderImageDTO.Add(OrderImageDTO);
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListOrderImageDTO;
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

        public async Task<ServiceResponse<OrderImageDTO>> AddOrderImage(OrderImageDTO request)
        {
            ServiceResponse<OrderImageDTO> _response = new();
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

                var _OrderImage = _mapper.Map<OrderImageDTO>(_newPImage);
                _response.Success = true;
                _response.Data = _OrderImage;
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

        public async Task<ServiceResponse<OrderImageDTO>> UpdateOrderImage(OrderImageDTO request)
        {
            ServiceResponse<OrderImageDTO> _response = new();
            try
            {
                var OrderImage = await _repository.OrderImageById(request.Id);
                if (OrderImage == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }
                OrderImage.OrderId = request.OrderId;
                OrderImage.Name = request.Name;
                OrderImage.Type = request.Type;
                OrderImage.Image = request.Image;

                if (!await _repository.UpdateOrderImage(OrderImage))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _OrderImage = _mapper.Map<OrderImageDTO>(OrderImage);
                _response.Success = true;
                _response.Data = _OrderImage;
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
        public async Task<ServiceResponse<OrderImageDTO>> UpdateImage(UpdateImage request)
        {
            ServiceResponse<OrderImageDTO> _response = new();
            try
            {
                var OrderImage = await _repository.OrderImageById(request.Id);
                if (OrderImage == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }
                OrderImage.Image = request.Image;

                if (!await _repository.UpdateOrderImage(OrderImage))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _OrderImage = _mapper.Map<OrderImageDTO>(OrderImage);
                _response.Success = true;
                _response.Data = _OrderImage;
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

        public async Task<ServiceResponse<OrderImageDTO>> DeleteOrderImage(int id)

        {
            ServiceResponse<OrderImageDTO> _response = new();
            try
            {
                var OrderImage = await _repository.OrderImageById(id);
                if (OrderImage == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }


                if (!await _repository.DeleteOrderImage(OrderImage))
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

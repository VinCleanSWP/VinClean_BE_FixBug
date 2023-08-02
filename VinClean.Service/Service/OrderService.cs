using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Repository;
using VinClean.Repo.Models;
using VinClean.Service.DTO;
using System.ComponentModel;
using VinClean.Service.DTO.Order;
using VinClean.Service.DTO.WorkingSlot;
using VinClean.Service.DTO.WorkingBy;
using VinClean.Service.DTO.Employee;
using VinClean.Service.DTO.Process;
using VinClean.Repo.Models.ProcessModel;

// Pass data from Repo to Controller

namespace VinClean.Service.Service
{
    public interface IOrderService
    {
        Task<ServiceResponse<List<OrderModeDTO>>> GetOrderList();
        Task<ServiceResponse<OrderDTO>> GetOrderById(int id);
        Task<ServiceResponse<OrderModeDTO>> GetAllInfoById(int id);
        Task<ServiceResponse<OrderDTO>> AddOrder(NewBooking Order);
        Task<ServiceResponse<OrderDTO>> UpdateOrder(OrderDTO Order);
        Task<ServiceResponse<OrderDTO>> UpdateSubPrice(UpdateSubPirce Order);
        Task<ServiceResponse<OrderDTO>> UpdateStartWorking(ProcessStartWorking Order);
        Task<ServiceResponse<OrderDTO>> UpdateEndWorking(ProcessEndWorking Order);
        Task<ServiceResponse<OrderDTO>> UpdateStatusCompleted(int id);
        Task<ServiceResponse<OrderDTO>> CancelOrder(CancelOrderDTO cancelOrder);
        Task<ServiceResponse<OrderDTO>> DeleteOrder(int id);
        Task<ServiceResponse<OrderDTO>> AssignEmployee(AssignEmployeeDTO request);
        Task<ServiceResponse<List<OrderModeDTO>>> GetOrderRange(SelectOrder select);
        Task<ServiceResponse<List<OrderModeDTO>>> GetAllOrderbyRange(SelectOrder select);
    }

    public class OrderService : IOrderService
    {
        private readonly IServiceRepository _serviceRepo;
        private readonly IOrderRepository _repository;
        private readonly ICustomerRepository _Curepository;
        private readonly ILocationRepository _Lrepository;
        private readonly IOrderImageRepository _PImgrepository;
        public readonly IMapper _mapper;
        public OrderService(IOrderRepository repository, IMapper mapper, 
            IServiceRepository serviceRepo, ICustomerRepository Curepository, ILocationRepository WBrepository, IOrderImageRepository pImgrepository)
        {
            _repository = repository;
            _mapper = mapper;
            _serviceRepo = serviceRepo;
            _Curepository = Curepository;
            _Lrepository = WBrepository;
            _PImgrepository = pImgrepository;
        }

        public async Task<ServiceResponse<List<OrderModeDTO>>> GetOrderList()
        {
            ServiceResponse<List<OrderModeDTO>> _response = new();
            /*try
            {*/
                var listOrder = await _repository.GetOrderlist();
                var listOrderDTO = new List<OrderModeDTO>();
                foreach (var Order in listOrder)
                {
                    listOrderDTO.Add(_mapper.Map<OrderModeDTO>(Order));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = listOrderDTO;
           /* }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }*/
            return _response;
        }

        public async Task<ServiceResponse<OrderDTO>> GetOrderById(int id)
        {
            ServiceResponse<OrderDTO> _response = new();
            try
            {
                var Order = await _repository.GetOrderById(id);
                if (Order == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var OrderDTO = _mapper.Map<OrderDTO>(Order);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = OrderDTO;

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

        public async Task<ServiceResponse<OrderModeDTO>> GetAllInfoById(int id)
        {
            ServiceResponse<OrderModeDTO> _response = new();
            try
            {
                var Order = await _repository.GetAllInfoById(id);
                /* var Order_dto = _mapper.Map<OrderInfo>(Order);*/
                if (Order == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                /*var OrderDTO = _mapper.Map<OrderModeDTO>(Order);*/
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = Order;

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

        public async Task<ServiceResponse<OrderDTO>> AddOrder(NewBooking request)
        {
            ServiceResponse<OrderDTO> _response = new();
            /*            try
                        {*/
            var service = await _serviceRepo.GetServiceById(request.ServiceId);
            var customer = await _Curepository.GetCustomerById(request.CustomerId);

            Order _newOrder = new Order()
            {
                CustomerId = request.CustomerId,
                Note = request.Note,
                Status = "Incoming",
                StarTime = request.StarTime,
                EndTime = request.StarTime + TimeSpan.FromHours((int)service.MinimalSlot),
                CreatedDate = DateTime.Now,
                Date = request.Date,
                Phone = request.Phone,
                Address = request.Address,
                Price = request.Price,
                PointUsed = request.PointUsed,
                IsDeleted = false,
            };
            var check1 = await _repository.AddOrder(_newOrder);

                //Update TotalPoint in Cutomer
                customer.TotalPoint = customer.TotalPoint - request.PointUsed;
                var check3 = await _Curepository.UpdateCustomer(customer);

            OrderImage _OrderImage1 = new OrderImage()
            {
                 OrderId = _newOrder.OrderId,
                 Type = "Verify",
                 Name = "Start Working"
            };
            await _PImgrepository.AddOrderImage(_OrderImage1);
            OrderImage _OrderImage2 = new OrderImage()
            {
                 OrderId = _newOrder.OrderId,
                 Type = "Ordering",
                 Name = "Ordering"
            };
            await _PImgrepository.AddOrderImage(_OrderImage2);
            OrderImage _OrderImage3 = new OrderImage()
            {
                OrderId = _newOrder.OrderId,
                Type = "Completed",
                Name = "End Working"
            };
            await _PImgrepository.AddOrderImage(_OrderImage3);

            Location location = new Location()
            {
                OrderId = _newOrder.OrderId,
            };
            await _Lrepository.AddLocation(location);

            if (!check1&&!check3)
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<OrderDTO>(_newOrder);
                _response.Message = "Created";

/*            }
            catch (Exception ex)
            {
                OrderId = _newOrder.OrderId,
                ServiceId = request.ServiceId,

            };
            var check2 = await _PDrepository.AddPD(_OrderDetail);


            if (!check1 && !check2)
            {
                _response.Error = "RepoError";
                _response.Success = false;
                _response.Data = null;
                return _response;
            }

            _response.Success = true;
            _response.Data = _mapper.Map<OrderDTO>(_newOrder);
            _response.Message = "Created";

            /*            }
                        catch (Exception ex)
                        {
                            _response.Success = false;
                            _response.Data = null;
                            _response.Message = "Error";
                            _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
                        }*/

            return _response;
        }

        public async Task<ServiceResponse<OrderDTO>> UpdateOrder(OrderDTO request)
        {
            ServiceResponse<OrderDTO> _response = new();
            try
            {
                var existingOrder = await _repository.GetOrderById(request.OrderId);
                if (existingOrder == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                existingOrder.Note = request.Note;
                existingOrder.Status = request.Status;
                existingOrder.IsDeleted = request.isDelete;
                existingOrder.Date = request.Date;
                //existingOrder.ModifiedDate = DateTime.Now;
                //existingOrder.ModifiedBy = request.ModifiedBy;

                if (!await _repository.UpdateOrder(existingOrder))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _OrderDTO = _mapper.Map<OrderDTO>(existingOrder);
                _response.Success = true;
                _response.Data = _OrderDTO;
                _response.Message = "Order Updated";

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

        public async Task<ServiceResponse<OrderDTO>> UpdateStartWorking(ProcessStartWorking request)
        {
            ServiceResponse<OrderDTO> _response = new();
            try
            {
                var existingOrder = await _repository.GetOrderById(request.ProcessId);
                if (existingOrder == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                existingOrder.StartWorking = request.StartWorking;
                existingOrder.Status = "Ordering";

                if (!await _repository.UpdateOrder(existingOrder))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _OrderDTO = _mapper.Map<OrderDTO>(existingOrder);
                _response.Success = true;
                _response.Data = _OrderDTO;
                _response.Message = "Order Updated";

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

        public async Task<ServiceResponse<OrderDTO>> AssignEmployee(AssignEmployeeDTO request)
        {
            ServiceResponse<OrderDTO> _response = new();
            try
            {
                var existingOrder = await _repository.GetOrderById(request.OrderId);
                if (existingOrder == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                existingOrder.EmployeeId = request.EmployeeId;

                if (!await _repository.UpdateOrder(existingOrder))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _OrderDTO = _mapper.Map<OrderDTO>(existingOrder);
                _response.Success = true;
                _response.Data = _OrderDTO;
                _response.Message = "Order Updated";

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

        public async Task<ServiceResponse<OrderDTO>> UpdateSubPrice(UpdateSubPirce request)
        {
            ServiceResponse<OrderDTO> _response = new();
            try
            {
                var existingOrder = await _repository.GetOrderById(request.ProcessId);
                if (existingOrder == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                existingOrder.SubPrice = request.SubPrice;
                existingOrder.Price = request.SubPrice + existingOrder.Price;

                if (!await _repository.UpdateOrder(existingOrder))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _OrderDTO = _mapper.Map<OrderDTO>(existingOrder);
                _response.Success = true;
                _response.Data = _OrderDTO;
                _response.Message = "Order Updated";

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

        public async Task<ServiceResponse<OrderDTO>> UpdateEndWorking(ProcessEndWorking request)
        {
            ServiceResponse<OrderDTO> _response = new();
            try
            {
                var existingOrder = await _repository.GetOrderById(request.ProcessId);
                if (existingOrder == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                existingOrder.EndWorking = request.EndWorking;

                if (!await _repository.UpdateOrder(existingOrder))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _OrderDTO = _mapper.Map<OrderDTO>(existingOrder);
                _response.Success = true;
                _response.Data = _OrderDTO;
                _response.Message = "Order Updated";

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


        public async Task<ServiceResponse<OrderDTO>> UpdateStatusCompleted(int id)
        {
            ServiceResponse<OrderDTO> _response = new();
            try
            {
                var existingOrder = await _repository.GetOrderById(id);
                if (existingOrder == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                existingOrder.Status = "Completed";

                if (!await _repository.UpdateOrder(existingOrder))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _OrderDTO = _mapper.Map<OrderDTO>(existingOrder);
                _response.Success = true;
                _response.Data = _OrderDTO;
                _response.Message = "Order Updated";

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
        public async Task<ServiceResponse<OrderDTO>> CancelOrder(CancelOrderDTO cancelOrder)
        {
            ServiceResponse<OrderDTO> _response = new();
            try
            {
                var existingOrder = await _repository.GetOrderById(cancelOrder.OrderId);
                if (existingOrder == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                existingOrder.Status = "Cancel";
                existingOrder.CancelBy = cancelOrder.CancelBy;
                existingOrder.CancelDate = DateTime.UtcNow;
                existingOrder.ReasonCancel = cancelOrder.ReasonCancel;

                if (!await _repository.UpdateOrder(existingOrder))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _OrderDTO = _mapper.Map<OrderDTO>(existingOrder);
                _response.Success = true;
                _response.Data = _OrderDTO;
                _response.Message = "Order Updated";

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
        public async Task<ServiceResponse<OrderDTO>> DeleteOrder(int id)
        {
            ServiceResponse<OrderDTO> _response = new();
            /*try
            {*/
                var existingOrder = await _repository.GetOrderById(id);
                var existingWorkingBy = await _Lrepository.GetLocationByOrderId(id);
                var existingOrderImg = await _PImgrepository.OrderImageListByProcessId(id);
                if (existingOrder == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                foreach ( var img in existingOrderImg)
                {
                    await _PImgrepository.DeleteOrderImage(img);
                }

                if (!await _Lrepository.DeleteLocation(existingWorkingBy)
                && (!await _repository.DeleteOrder(existingOrder)))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _OrderDTO = _mapper.Map<OrderDTO>(existingOrder);
                _response.Success = true;
                _response.Data = _OrderDTO;
                _response.Message = "Deleted";

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
        public async Task<ServiceResponse<List<OrderModeDTO>>> GetOrderRange(SelectOrder select)
        {
            ServiceResponse<List<OrderModeDTO>> _response = new();
            try
            {
                var ListOrder = await _repository.SelectOrder(select);
                var ListOrderDTO = new List<OrderModeDTO>();
                foreach (var order in ListOrder)
                {
                    ListOrderDTO.Add(_mapper.Map<OrderModeDTO>(order));
                }

                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListOrderDTO;
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

        public async Task<ServiceResponse<List<OrderModeDTO>>> GetAllOrderbyRange(SelectOrder select)
        {
            ServiceResponse<List<OrderModeDTO>> _response = new();
            try
            {
                var ListOrder = await _repository.SelectAllOrder(select);
                var ListOrderDTO = new List<OrderModeDTO>();
                foreach (var order in ListOrder)
                {
                    ListOrderDTO.Add(_mapper.Map<OrderModeDTO>(order));
                }

                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListOrderDTO;
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
    }
}

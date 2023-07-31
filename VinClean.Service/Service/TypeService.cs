using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Type;

namespace VinClean.Service.Service
{
    public interface ITypeService
    {
        Task<ServiceResponse<List<TypeDTO>>> GetTypeList();
        Task<ServiceResponse<TypeDTO>> GetTypeById(int id);
        Task<ServiceResponse<TypeDTO>> DeleteType(int id);
        Task<ServiceResponse<TypeDTO>> UpdateType(TypeDTO request);
        Task<ServiceResponse<TypeDTO>> AddType(TypeDTO request);
    }
    public class TypeService : ITypeService
    {
        private readonly ITypeRepository _repository;
        private readonly IMapper _mapper;
        public TypeService(ITypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<TypeDTO>>> GetTypeList()
        {
            ServiceResponse<List<TypeDTO>> _response = new();
            try
            {
                var ListType = await _repository.GetTypeList();
                var ListTypeDTO = new List<TypeDTO>();
                foreach (var type in ListType)
                {
                    ListTypeDTO.Add(_mapper.Map<TypeDTO>(type));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListTypeDTO;
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

        public async Task<ServiceResponse<TypeDTO>> GetTypeById(int id)
        {
            ServiceResponse<TypeDTO> _response = new();
            try
            {
                var account = await _repository.GetTypeById(id);
                if (account == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var accoundto = _mapper.Map<TypeDTO>(account);
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

        public async Task<ServiceResponse<TypeDTO>> UpdateType(TypeDTO request)
        {
            ServiceResponse<TypeDTO> _response = new();
            try
            {
                var existingType = await _repository.GetTypeById(request.TypeId);
                if (existingType == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }
                existingType.Type1 = request.Type1;
                existingType.Avaiable = request.Avaiable;
                existingType.Img = request.Img;


                if (!await _repository.UpdateType(existingType))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _TypetDTO = _mapper.Map<TypeDTO>(existingType);
                _response.Success = true;
                _response.Data = _TypetDTO;
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

        public async Task<ServiceResponse<TypeDTO>> AddType(TypeDTO request)
        {
            ServiceResponse<TypeDTO> _response = new();
            try
            {
                Repo.Models.Type type = new Repo.Models.Type()
                {
                    Type1 = request.Type1,
                    Img = request.Img,
                    Avaiable = request.Avaiable
                };


                if (!await _repository.addType(type))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _TypetDTO = _mapper.Map<TypeDTO>(type);
                _response.Success = true;
                _response.Data = _TypetDTO;
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
        public async Task<ServiceResponse<TypeDTO>> DeleteType(int id)

        {
            ServiceResponse<TypeDTO> _response = new();
            try
            {
                var existingAccount = await _repository.GetTypeById(id);
                if (existingAccount == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }


                if (!await _repository.DeleteType(id))
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

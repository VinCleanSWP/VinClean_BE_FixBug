using AutoMapper;
using VinClean.Repo.Models;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Role;

namespace VinClean.Service.Service
{
    public interface IRoleService
    {
        Task<ServiceResponse<List<RoleDTO>>> GetRoleList();
        Task<ServiceResponse<RoleDTO>> GetRoleById(int id);
        Task<ServiceResponse<RoleDTO>> AddRole(RoleDTO request);
        Task<ServiceResponse<RoleDTO>> UpdateRole(RoleDTO request);
        Task<ServiceResponse<RoleDTO>> DeleteRole(int id);

    }
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<RoleDTO>> AddRole(RoleDTO request)
        {
            ServiceResponse<RoleDTO> _response = new();
            try
            {
                Role _newRole = new Role()
                {
                    Name = request.Name,

                };

                if (!await _repository.AddRole(_newRole))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<RoleDTO>(_newRole);
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

        public async Task<ServiceResponse<RoleDTO>> DeleteRole(int id)
        {
            ServiceResponse<RoleDTO> _response = new();
            try
            {
                var existingRole = await _repository.GetRoleById(id);
                if (existingRole == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                if (!await _repository.DeleteRole(existingRole))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _OrderDTO = _mapper.Map<RoleDTO>(existingRole);
                _response.Success = true;
                _response.Data = _OrderDTO;
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

        public async Task<ServiceResponse<RoleDTO>> GetRoleById(int id)
        {
            ServiceResponse<RoleDTO> _response = new();
            try
            {
                var Role = await _repository.GetRoleById(id);
                if (Role == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var Roledto = _mapper.Map<RoleDTO>(Role);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = Roledto;

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

        public async Task<ServiceResponse<List<RoleDTO>>> GetRoleList()
        {
            ServiceResponse<List<RoleDTO>> _response = new();
            try
            {
                var ListRole = await _repository.GetRoleList();
                var ListRoleDTO = new List<RoleDTO>();
                foreach (var Role in ListRole)
                {
                    ListRoleDTO.Add(_mapper.Map<RoleDTO>(Role));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListRoleDTO;
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

        public async Task<ServiceResponse<RoleDTO>> UpdateRole(RoleDTO request)
        {
            ServiceResponse<RoleDTO> _response = new();
            try
            {
                var existingRole = await _repository.GetRoleById(request.RoleId);
                if (existingRole == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }
                // cac gia trị cho sua
                existingRole.Name = request.Name;               

                if (!await _repository.UpdateRole(existingRole))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _RoleDTO = _mapper.Map<RoleDTO>(existingRole);
                _response.Success = true;
                _response.Data = _RoleDTO;
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

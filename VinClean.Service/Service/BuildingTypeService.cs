using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using VinClean.Repo.Models;
using VinClean.Service.DTO.Building;
using VinClean.Service.DTO.Role;

namespace VinClean.Service.Service
{
    public interface IBuildingTypeService
    {
        Task<ServiceResponse<List<BuildingTypeDTO>>> GetBuildingTypeList();
        Task<ServiceResponse<BuildingTypeDTO>> GetBuildingTypeById(int id);
        Task<ServiceResponse<BuildingTypeDTO>> AddBuildingType(BuildingTypeDTO request);
        Task<ServiceResponse<BuildingTypeDTO>> UpdateBuildingType(BuildingTypeDTO request);
        Task<ServiceResponse<BuildingTypeDTO>> DeleteBuildingType(int id);
    }
    public class BuildingTypeService : IBuildingTypeService
    {
        private readonly IBuildingTypeRepository _buildingTypeRepositor;
        private readonly IMapper _mapper;
        public BuildingTypeService(IBuildingTypeRepository buildingTypeRepository, IMapper mapper)
        {
            _buildingTypeRepositor = buildingTypeRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<BuildingTypeDTO>> AddBuildingType(BuildingTypeDTO request)
        {
            ServiceResponse<BuildingTypeDTO> _response = new();
            try
            {
                BuildingType _newBuildingType = new BuildingType()
                {
                    Id = request.Id,
                };

                if (!await _buildingTypeRepositor.AddBuildingType(_newBuildingType))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<BuildingTypeDTO>(_newBuildingType);
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

        public async Task<ServiceResponse<BuildingTypeDTO>> DeleteBuildingType(int id)
        {
            ServiceResponse<BuildingTypeDTO> _response = new();
            try
            {
                var existingBuildingType = await _buildingTypeRepositor.GetBuildingTypeById(id);
                if (existingBuildingType == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                if (!await _buildingTypeRepositor.DeleteBuildingType(existingBuildingType))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _OrderDTO = _mapper.Map<BuildingTypeDTO>(existingBuildingType);
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

        public async Task<ServiceResponse<BuildingTypeDTO>> GetBuildingTypeById(int id)
        {
            ServiceResponse<BuildingTypeDTO> _response = new();
            try
            {
                var BuildingType = await _buildingTypeRepositor.GetBuildingTypeById(id);
                if (BuildingType == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var BuildingTypeDTO = _mapper.Map<BuildingTypeDTO>(BuildingType);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = BuildingTypeDTO;

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

        public async Task<ServiceResponse<List<BuildingTypeDTO>>> GetBuildingTypeList()
        {
            ServiceResponse<List<BuildingTypeDTO>> _response = new();
            try
            {
                var ListBuildingType = await _buildingTypeRepositor.GetBuildingTypeList();
                var ListBuildingTypeDTO = new List<BuildingTypeDTO>();
                foreach (var BuildingType in ListBuildingType)
                {
                    ListBuildingTypeDTO.Add(_mapper.Map<BuildingTypeDTO>(BuildingType));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListBuildingTypeDTO;
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

        public async Task<ServiceResponse<BuildingTypeDTO>> UpdateBuildingType(BuildingTypeDTO request)
        {
            ServiceResponse<BuildingTypeDTO> _response = new();
            try
            {
                var existingBuildingType = await _buildingTypeRepositor.GetBuildingTypeById(request.Id);
                if (existingBuildingType == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }
                // cac gia trị cho sua
                existingBuildingType.Type = request.Type;

                if (!await _buildingTypeRepositor.UpdateBuildingType(existingBuildingType))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _BuildingType = _mapper.Map<BuildingTypeDTO>(existingBuildingType);
                _response.Success = true;
                _response.Data = _BuildingType;
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

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Repository;
using VinClean.Service.DTO.Building;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Building;
using VinClean.Repo.Models;

namespace VinClean.Service.Service
{
    public interface IBuildingService
    {
        Task<ServiceResponse<List<BuildingDTO>>> GetBuildingList();
        Task<ServiceResponse<List<BuildingDTO>>> GetBuildingListByType(int id);
        Task<ServiceResponse<BuildingDTO>> GetBuildingById(int id);
        Task<ServiceResponse<BuildingDTO>> AddBuilding(BuildingDTO request);
        Task<ServiceResponse<BuildingDTO>> UpdateBuilding(BuildingDTO request);
        Task<ServiceResponse<BuildingDTO>> DeleteBuilding(int id);
    }

    public class BuildingService : IBuildingService
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly IMapper _mapper;
        public BuildingService(IBuildingRepository buildingRepository, IMapper mapper)
        {
            _buildingRepository = buildingRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<BuildingDTO>> AddBuilding(BuildingDTO request)
        {
            ServiceResponse<BuildingDTO> _response = new();
            try
            {
                Building _newBuilding = new Building()
                {
                    Name = request.Name, 
                    TypeId = request.TypeId

                };

                if (!await _buildingRepository.AddBuilding(_newBuilding))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<BuildingDTO>(_newBuilding);
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

        public async Task<ServiceResponse<BuildingDTO>> DeleteBuilding(int id)
        {
            ServiceResponse<BuildingDTO> _response = new();
            try
            {
                var existingBuilding = await _buildingRepository.GetBuildingById(id);
                if (existingBuilding == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                if (!await _buildingRepository.DeleteBuilding(existingBuilding))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _OrderDTO = _mapper.Map<BuildingDTO>(existingBuilding);
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

        public async Task<ServiceResponse<BuildingDTO>> GetBuildingById(int id)
        {
            ServiceResponse<BuildingDTO> _response = new();
            try
            {
                var Building = await _buildingRepository.GetBuildingById(id);
                if (Building == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var BuildingDTO = _mapper.Map<BuildingDTO>(Building);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = BuildingDTO;

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

        public async Task<ServiceResponse<List<BuildingDTO>>> GetBuildingList()
        {
            ServiceResponse<List<BuildingDTO>> _response = new();
            try
            {
                var ListBuilding = await _buildingRepository.GetBuildingList();
                var ListBuildingDTO = new List<BuildingDTO>();
                foreach (var Building in ListBuilding)
                {
                    ListBuildingDTO.Add(_mapper.Map<BuildingDTO>(Building));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListBuildingDTO;
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

        public async Task<ServiceResponse<List<BuildingDTO>>> GetBuildingListByType(int id)
        {
            ServiceResponse<List<BuildingDTO>> _response = new();
            try
            {
                var ListBuilding = await _buildingRepository.GetBuildingListByType(id);
                var ListBuildingDTO = new List<BuildingDTO>();
                foreach (var Building in ListBuilding)
                {
                    ListBuildingDTO.Add(_mapper.Map<BuildingDTO>(Building));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListBuildingDTO;
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

        public async Task<ServiceResponse<BuildingDTO>> UpdateBuilding(BuildingDTO request)
        {
            ServiceResponse<BuildingDTO> _response = new();
            try
            {
                var existingBuilding = await _buildingRepository.GetBuildingById(request.Id);
                if (existingBuilding == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }
                // cac gia trị cho sua
                existingBuilding.Name = request.Name;
                existingBuilding.TypeId = request.TypeId;

                if (!await _buildingRepository.UpdateBuilding(existingBuilding))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _BuildingDTO = _mapper.Map<BuildingDTO>(existingBuilding);
                _response.Success = true;
                _response.Data = _BuildingDTO;
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

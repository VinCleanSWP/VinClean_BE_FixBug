using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Category;

namespace VinClean.Service.Service
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<CategoryDTO>>> GetCategoryList();
        Task<ServiceResponse<CategoryDTO>> GetCategoryById(int id);
        Task<ServiceResponse<CategoryDTO>> CreateCategory(CategoryDTO request);
        Task<ServiceResponse<CategoryDTO>> UpdateCategory(CategoryDTO request);
        Task<ServiceResponse<CategoryDTO>> DeleteCategory(int id);
    }
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        async Task<ServiceResponse<CategoryDTO>> ICategoryService.CreateCategory(CategoryDTO request)
        {
            ServiceResponse<CategoryDTO> _response = new();
            try
            {

                Category _newCategory = new Category()
                {
                    Category1 = request.Category1
                   

                };

                if (!await _repository.CreateCategory(_newCategory))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<CategoryDTO>(_newCategory);
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

        async Task<ServiceResponse<CategoryDTO>> ICategoryService.DeleteCategory(int id)
        {
            ServiceResponse<CategoryDTO> _response = new();
            try
            {
                var existingCategory = await _repository.GetCategoryById(id);
                if (existingCategory == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                if (!await _repository.DeleteCategory(existingCategory))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _categoryDTO = _mapper.Map<CategoryDTO>(existingCategory);
                _response.Success = true;
                _response.Data = _categoryDTO;
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

        async Task<ServiceResponse<CategoryDTO>> ICategoryService.GetCategoryById(int id)
        {
            ServiceResponse<CategoryDTO> _response = new();
            try
            {
                var category = await _repository.GetCategoryById(id);
                if (category == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var categorydto = _mapper.Map<CategoryDTO>(category);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = categorydto;

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

        async Task<ServiceResponse<List<CategoryDTO>>> ICategoryService.GetCategoryList()
        {
            ServiceResponse<List<CategoryDTO>> _response = new();
            try
            {
                var ListCategory = await _repository.GetCategoryList();
                var ListCategoryDTO = new List<CategoryDTO>();
                foreach (var category in ListCategory)
                {
                   ListCategoryDTO.Add(_mapper.Map<CategoryDTO>(category));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListCategoryDTO;
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

       

        async Task<ServiceResponse<CategoryDTO>> ICategoryService.UpdateCategory(CategoryDTO request)
        {
            ServiceResponse<CategoryDTO> _response = new();
            try
            {
                var existingCategory = await _repository.GetCategoryById(request.CategoryId);
                if (existingCategory == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                existingCategory.CategoryId = request.CategoryId;
                existingCategory.Category1 = request.Category1;


                if (!await _repository.UpdateCategory(existingCategory))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _categoryDTO = _mapper.Map<CategoryDTO>(existingCategory);
                _response.Success = true;
                _response.Data = _categoryDTO;
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

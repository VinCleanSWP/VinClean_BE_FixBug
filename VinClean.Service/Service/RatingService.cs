using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Rating;

// Pass data from Repo to Controller

namespace VinClean.Service.Service
{
    public interface IRatingService
    {
        Task<ServiceResponse<List<RatingModelDTO>>> GetRatingList();
        Task<ServiceResponse<List<RatingModelDTO>>> GetRatingByService(int id);
        Task<ServiceResponse<RateServiceDTO>> GetRatingById(int id);
        Task<ServiceResponse<RatingDTO>> AddRating(RatingDTO Rating);
        Task<ServiceResponse<RatingDTO>> UpdateRating(RatingDTO Rating);
        Task<ServiceResponse<RatingDTO>> DeleteRating(int id);
        //Task<ServiceResponse<AverageRatingDTO>> GetAverageTypeRating(int id);
    }
    public class RatingService : IRatingService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IServiceRepository _serviceRepository;
        public readonly IMapper _mapper;
        public RatingService(ICustomerRepository customerRepository, IRatingRepository ratingRepository, IServiceRepository serviceRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _ratingRepository = ratingRepository;
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        // Get Rating List
        public async Task<ServiceResponse<List<RatingModelDTO>>> GetRatingList()
        {
            ServiceResponse<List<RatingModelDTO>> _response = new();
            try
            {
                var listRating = await _ratingRepository.GetRatinglist();
                var listRatingDTO = new List<RatingModelDTO>();
                foreach (var rating in listRating)
                {
                    listRatingDTO.Add(_mapper.Map<RatingModelDTO>(rating));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = listRatingDTO;
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

        // Get Rating List By TypeID
        async Task<ServiceResponse<List<RatingModelDTO>>> IRatingService.GetRatingByService(int id)
        {
            ServiceResponse<List<RatingModelDTO>> _response = new();
            try
            {
                var ratingList = await _ratingRepository.GetRatingByService(id);
                if (ratingList == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var ratingDTO = new List<RatingModelDTO>();
                foreach (var rating in ratingList)
                {
                    ratingDTO.Add(_mapper.Map<RatingModelDTO>(rating));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ratingDTO;

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

        // Get Rating By ID
        public async Task<ServiceResponse<RateServiceDTO>> GetRatingById(int id)
        {
            ServiceResponse<RateServiceDTO> _response = new();
            try
            {
                var rating = await _ratingRepository.GetRatingById(id);
                if (rating == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var ratingDTO = _mapper.Map<RateServiceDTO>(rating);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ratingDTO;

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

        // Add Rating To Ordered Service
        public async Task<ServiceResponse<RatingDTO>> AddRating(RatingDTO request)
        {
            ServiceResponse<RatingDTO> _response = new();
            try
            {
                //var existingRating = await _ratingRepository.GetRatingById(request.RateId);
                if (!await _ratingRepository.CheckServiceRating(request.ServiceId, request.CustomerId))
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                Rating _newRating = new Rating()
                {
                    ServiceId = request.ServiceId,
                    Rate = request.Rate,
                    Comment = request.Comment,
                    CreatedDate = DateTime.Now,
                    CustomerId = request.CustomerId,
                    IsDeleted = false,
                };

                if (!await _ratingRepository.AddRating(_newRating))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<RatingDTO>(_newRating);
                _response.Message = "Rating Created";

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

        // Update Existing Rating
        public async Task<ServiceResponse<RatingDTO>> UpdateRating(RatingDTO request)
        {
            ServiceResponse<RatingDTO> _response = new();
            try
            {
                var existingRating = await _ratingRepository.GetRatingById(request.RateId);
                if (existingRating == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                existingRating.Rate = request.Rate;
                existingRating.Comment = request.Comment;
                existingRating.ModifiedDate = DateTime.Now;

                if (!await _ratingRepository.UpdateRating(existingRating))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _ratingDTO = _mapper.Map<RatingDTO>(existingRating);
                _response.Success = true;
                _response.Data = _ratingDTO;
                _response.Message = "Rating Updated";

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

        // Delete Existing Rating
        public async Task<ServiceResponse<RatingDTO>> DeleteRating(int id)
        {
            ServiceResponse<RatingDTO> _response = new();
            try
            {
                var existingRating = await _ratingRepository.GetRatingById(id);
                if (existingRating == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                if (!await _ratingRepository.DeleteRating(existingRating))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _ratingDTO = _mapper.Map<RatingDTO>(existingRating);
                _response.Success = true;
                _response.Data = _ratingDTO;
                _response.Message = "Rating Deleted";

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

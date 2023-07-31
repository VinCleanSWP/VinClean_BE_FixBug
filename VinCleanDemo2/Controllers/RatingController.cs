using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VinClean.Repo.Models;
using VinClean.Service.DTO.Rating;
using VinClean.Service.DTO.Service;
using VinClean.Service.Service;

namespace VinClean.Controllers
{
    // API Deploy

    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _service;
        public RatingController(IRatingService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<RatingModelDTO>>> GetRating()
        {
            return Ok(await _service.GetRatingList());
        }

        [HttpGet("Service/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RatingModelDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<RatingModelDTO>>> GetByServiceId(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var ratingFound = await _service.GetRatingByService(id);
            if (ratingFound == null)
            {
                return NotFound();
            }
            return Ok(ratingFound);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RateServiceDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Rating>> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var ratingFound = await _service.GetRatingById(id);
            if (ratingFound == null)
            {
                return NotFound();
            }
            return Ok(ratingFound);
        }
        [HttpPost]
        public async Task<ActionResult<Rating>> AddRating(RatingDTO request)
        {


            var newRating = await _service.AddRating(request);
            if (newRating.Success == false && newRating.Message == "Exist")
            {
                return Ok(newRating);
            }

            if (newRating.Success == false && newRating.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when adding Rating {request}");
                return StatusCode(500, ModelState);
            }

            if (newRating.Success == false && newRating.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when adding Rating {request}");
                return StatusCode(500, ModelState);
            }
            return Ok(newRating.Data);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateRating(RatingDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateRating = await _service.UpdateRating(request);

            if (updateRating.Success == false && updateRating.Message == "NotFound")
            {
                return Ok(updateRating);
            }

            if (updateRating.Success == false && updateRating.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating Rating {request}");
                return StatusCode(500, ModelState);
            }

            if (updateRating.Success == false && updateRating.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating Rating {request}");
                return StatusCode(500, ModelState);
            }

            return Ok(updateRating);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRating(int id)
        {
            var deleteRating = await _service.DeleteRating(id);


            if (deleteRating.Success == false && deleteRating.Message == "NotFound")
            {
                ModelState.AddModelError("", "Rating Not found");
                return StatusCode(404, ModelState);
            }

            if (deleteRating.Success == false && deleteRating.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in Repository when deleting Rating");
                return StatusCode(500, ModelState);
            }

            if (deleteRating.Success == false && deleteRating.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when deleting Rating");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
    }
}
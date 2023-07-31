using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VinClean.Repo.Models;
using VinClean.Service.DTO.Building;
using VinClean.Service.Service;

namespace VinClean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingTypeController : ControllerBase
    {
        private readonly IBuildingTypeService _service;
        public BuildingTypeController(IBuildingTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<BuildingType>>> GetBuildingTypeList()
        {
            return Ok(await _service.GetBuildingTypeList());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BuildingType))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<BuildingTypeDTO>> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var BuildingTypeFound = await _service.GetBuildingTypeById(id);
            if (BuildingTypeFound == null)
            {
                return NotFound();
            }
            return Ok(BuildingTypeFound);
        }


        [HttpPost]
        public async Task<ActionResult<BuildingTypeDTO>> CreateBuildingType(BuildingTypeDTO request)
        {
            /*            if(request == null)
                        {
                            return BadRequest(ModelState);
                        }
                        if(ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }*/

            var newBuildingType = await _service.AddBuildingType(request);
            if (newBuildingType.Success == false && newBuildingType.Message == "Exist")
            {
                return Ok(newBuildingType);
            }

            if (newBuildingType.Success == false && newBuildingType.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when adding BuildingType {request}");
                return StatusCode(500, ModelState);
            }

            if (newBuildingType.Success == false && newBuildingType.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when adding BuildingType {request}");
                return StatusCode(500, ModelState);
            }
            return Ok(newBuildingType.Data);
        }


        [HttpPut]
        public async Task<ActionResult> UpdateBuildingType(BuildingTypeDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateBuildingType = await _service.UpdateBuildingType(request);

            if (updateBuildingType.Success == false && updateBuildingType.Message == "NotFound")
            {
                return Ok(updateBuildingType);
            }

            if (updateBuildingType.Success == false && updateBuildingType.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating BuildingType {request}");
                return StatusCode(500, ModelState);
            }

            if (updateBuildingType.Success == false && updateBuildingType.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating BuildingType {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateBuildingType);

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBuildingType(int id)
        {
            var deleteBuildingType = await _service.DeleteBuildingType(id);


            if (deleteBuildingType.Success == false && deleteBuildingType.Message == "NotFound")
            {
                ModelState.AddModelError("", "BuildingType Not found");
                return StatusCode(404, ModelState);
            }

            if (deleteBuildingType.Success == false && deleteBuildingType.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in Repository when deleting BuildingType");
                return StatusCode(500, ModelState);
            }

            if (deleteBuildingType.Success == false && deleteBuildingType.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when deleting BuildingType");
                return StatusCode(500, ModelState);
            }

            return NoContent();


        }
    }
}

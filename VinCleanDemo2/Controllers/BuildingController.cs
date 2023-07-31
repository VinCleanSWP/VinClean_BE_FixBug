using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VinClean.Repo.Models;
using VinClean.Service.DTO.Process;
using VinClean.Service.DTO;
using VinClean.Service.Service;
using VinClean.Service.DTO.Building;

namespace VinClean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private readonly IBuildingService _buildingService;
        public BuildingController(IBuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BuildingDTO>>> Order()
        {
            return Ok(await _buildingService.GetBuildingList());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BuildingDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Order>> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var processFound = await _buildingService.GetBuildingById(id);
            if (processFound == null)
            {
                return NotFound();
            }
            return Ok(processFound);
        }

        [HttpGet("Type/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BuildingDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Order>> GetBuildingListByType(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var processFound = await _buildingService.GetBuildingListByType(id);
            if (processFound == null)
            {
                return NotFound();
            }
            return Ok(processFound);
        }

        [HttpPost]
        public async Task<ActionResult<Building>> CreateOrder(BuildingDTO request)
        {


            var newBuilding = await _buildingService.AddBuilding(request);
            if (newBuilding.Success == false && newBuilding.Message == "Exist")
            {
                return Ok(newBuilding);
            }

            if (newBuilding.Success == false && newBuilding.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when adding Process {request}");
                return StatusCode(500, ModelState);
            }

            if (newBuilding.Success == false && newBuilding.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when adding Process {request}");
                return StatusCode(500, ModelState);
            }
            return Ok(newBuilding.Data);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateProcess(BuildingDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateBuilding = await _buildingService.UpdateBuilding(request);

            if (updateBuilding.Success == false && updateBuilding.Message == "NotFound")
            {
                return Ok(updateBuilding);
            }

            if (updateBuilding.Success == false && updateBuilding.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating Process {request}");
                return StatusCode(500, ModelState);
            }

            if (updateBuilding.Success == false && updateBuilding.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating Process {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateBuilding);

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProcess(int id)
        {
            var deleteBuilding = await _buildingService.DeleteBuilding(id);


            if (deleteBuilding.Success == false && deleteBuilding.Message == "NotFound")
            {
                ModelState.AddModelError("", "Process Not found");
                return StatusCode(404, ModelState);
            }

            if (deleteBuilding.Success == false && deleteBuilding.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in Repository when deleting Process");
                return StatusCode(500, ModelState);
            }

            if (deleteBuilding.Success == false && deleteBuilding.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when deleting Process");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
    }
}

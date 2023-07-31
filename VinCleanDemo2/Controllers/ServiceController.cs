using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VinClean.Repo.Models;
using VinClean.Service.DTO.Service;
using VinClean.Service.Service;

namespace VinClean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {

        private readonly IServiceService _service;
        public ServiceController(IServiceService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ServiceDTO>>> GetServiceList()
        {
            return Ok(await _service.GetServiceList());
        }

        [HttpGet("Type/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Repo.Models.Service>> GetListServerByTypeId(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var ServiceFound = await _service.GetServiceListById(id);
            if (ServiceFound == null)
            {
                return NotFound();
            }
            return Ok(ServiceFound);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Repo.Models.Service>> GetServerById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var ServiceFound = await _service.GetServiceById(id);
            if (ServiceFound == null)
            {
                return NotFound();
            }
            return Ok(ServiceFound);
        }
        [HttpPost]
        public async Task<ActionResult<Repo.Models.Service>> AddService(newServiceDTO request)
        {


            var newService = await _service.AddService(request);
            if (newService.Success == false && newService.Message == "Exist")
            {
                return Ok(newService);
            }

            if (newService.Success == false && newService.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when adding Account {request}");
                return StatusCode(500, ModelState);
            }

            if (newService.Success == false && newService.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when adding service {request}");
                return StatusCode(500, ModelState);
            }
            return Ok(newService.Data);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateService(ServiceDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateService = await _service.UpdateService(request);

            if (updateService.Success == false && updateService.Message == "NotFound")
            {
                return Ok(updateService);
            }

            if (updateService.Success == false && updateService.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating account {request}");
                return StatusCode(500, ModelState);
            }

            if (updateService.Success == false && updateService.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating account {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateService);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteService(int id)
        {
            var deleteService = await _service.DeleteService(id);


            if (deleteService.Success == false && deleteService.Message == "NotFound")
            {
                ModelState.AddModelError("", "Service Not found");
                return StatusCode(404, ModelState);
            }

            if (deleteService.Success == false && deleteService.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in Repository when deleting Service");
                return StatusCode(500, ModelState);
            }

            if (deleteService.Success == false && deleteService.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when deleting Service");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

    }
}

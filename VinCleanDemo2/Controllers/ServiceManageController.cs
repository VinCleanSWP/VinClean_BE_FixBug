using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VinClean.Service.DTO.Service;
using VinClean.Service.Service;

namespace VinClean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceManageController : ControllerBase
    {

        private readonly IServiceManageService _ServiceManage;
        public ServiceManageController(IServiceManageService ServiceManage)
        {
            _ServiceManage = ServiceManage;
        }

        [HttpGet]
        public async Task<ActionResult<List<ServiceManageDTO>>> GetServiceManageList()
        {
            return Ok(await _ServiceManage.GetServiceManageList());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceManageDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Repo.Models.ServiceManage>> GetServerById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var ServiceManageFound = await _ServiceManage.GetServiceManageById(id);
            if (ServiceManageFound == null)
            {
                return NotFound();
            }
            return Ok(ServiceManageFound);
        }
        
        
        [HttpPost]
        public async Task<ActionResult<Repo.Models.ServiceManage>> CreateServiceManage(ServiceManageDTO request)
        {


            var newServiceManage = await _ServiceManage.CreateServiceManage(request);
            if (newServiceManage.Success == false && newServiceManage.Message == "Exist")
            {
                return Ok(newServiceManage);
            }

            if (newServiceManage.Success == false && newServiceManage.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when adding Account {request}");
                return StatusCode(500, ModelState);
            }

            if (newServiceManage.Success == false && newServiceManage.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in ServiceManage layer when adding Account {request}");
                return StatusCode(500, ModelState);
            }
            return Ok(newServiceManage.Data);
        }
        
        [HttpPut]
        public async Task<ActionResult> UpdateServiceManage(ServiceManageDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateServiceManage = await _ServiceManage.UpdateServiceManage(request);

            if (updateServiceManage.Success == false && updateServiceManage.Message == "NotFound")
            {
                return Ok(updateServiceManage);
            }

            if (updateServiceManage.Success == false && updateServiceManage.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating account {request}");
                return StatusCode(500, ModelState);
            }

            if (updateServiceManage.Success == false && updateServiceManage.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in ServiceManage layer when updating account {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateServiceManage);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteServiceManage(int id)
        {
            var deleteAccount = await _ServiceManage.DeleteServiceManage(id);


            if (deleteAccount.Success == false && deleteAccount.Message == "NotFound")
            {
                ModelState.AddModelError("", "ServiceManage Not found");
                return StatusCode(404, ModelState);
            }

            if (deleteAccount.Success == false && deleteAccount.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in Repository when deleting ServiceManage");
                return StatusCode(500, ModelState);
            }

            if (deleteAccount.Success == false && deleteAccount.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in ServiceManage layer when deleting ServiceManage");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

    }
}

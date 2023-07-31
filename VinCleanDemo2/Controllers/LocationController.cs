using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VinClean.Repo.Models;
using VinClean.Service.DTO.WorkingBy;
using VinClean.Service.DTO.WorkingSlot;
using VinClean.Service.Service;

namespace VinClean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IWorkingByService _service;
        public LocationController(IWorkingByService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<LocationDTO>>> Get()
        {
            return Ok(await _service.GetWBList());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LocationDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<LocationDTO>> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var Wb = await _service.GetWBById(id);
            if (Wb == null)
            {
                return NotFound();
            }
            return Ok(Wb);
        }
        [HttpGet("Order/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LocationDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<LocationDTO>> GetByOrderId(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var Wb = await _service.GetWBByProcessId(id);
            if (Wb == null)
            {
                return NotFound();
            }
            return Ok(Wb);
        }

        [HttpPost]
        public async Task<ActionResult<LocationDTO>> AddWB(LocationDTO request)
        {
            /*            if(request == null)
                        {
                            return BadRequest(ModelState);
                        }
                        if(ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }*/

            var newSlot = await _service.AddWB(request);
            if (newSlot.Success == false && newSlot.Message == "Exist")
            {
                return Ok(newSlot);
            }

            if (newSlot.Success == false && newSlot.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when adding Account {request}");
                return StatusCode(500, ModelState);
            }

            if (newSlot.Success == false && newSlot.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when adding Account {request}");
                return StatusCode(500, ModelState);
            }
            return Ok(newSlot.Data);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateWB(LocationDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateAccount = await _service.UpdateWB(request);

            if (updateAccount.Success == false && updateAccount.Message == "NotFound")
            {
                return Ok(updateAccount);
            }

            if (updateAccount.Success == false && updateAccount.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating account {request}");
                return StatusCode(500, ModelState);
            }

            if (updateAccount.Success == false && updateAccount.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating account {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateAccount);

        }
        [HttpPut("Location")]
        public async Task<ActionResult> UpdateLocation(Service.DTO.WorkingBy.UpdateLocation request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateAccount = await _service.UpdateLocation(request);

            if (updateAccount.Success == false && updateAccount.Message == "NotFound")
            {
                return Ok(updateAccount);
            }

            if (updateAccount.Success == false && updateAccount.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating account {request}");
                return StatusCode(500, ModelState);
            }

            if (updateAccount.Success == false && updateAccount.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating account {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateAccount);

        }
        [HttpPut("AcceptedRequest")]
        public async Task<ActionResult> AcceptedRequest(LocationDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateAccount = await _service.AcceptRequest(request);

            if (updateAccount.Success == false && updateAccount.Message == "NotFound")
            {
                return Ok(updateAccount);
            }

            if (updateAccount.Success == false && updateAccount.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating account {request}");
                return StatusCode(500, ModelState);
            }

            if (updateAccount.Success == false && updateAccount.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating account {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateAccount);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWB(int id)
        {
            var deleteAccount = await _service.DeleteWB(id);


            if (deleteAccount.Success == false && deleteAccount.Message == "NotFound")
            {
                ModelState.AddModelError("", "Account Not found");
                return StatusCode(404, ModelState);
            }

            if (deleteAccount.Success == false && deleteAccount.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in Repository when deleting account");
                return StatusCode(500, ModelState);
            }

            if (deleteAccount.Success == false && deleteAccount.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when deleting account");
                return StatusCode(500, ModelState);
            }

            return Ok(deleteAccount);

        }
    }
}

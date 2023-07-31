using Microsoft.AspNetCore.Mvc;
using VinClean.Repo.Models;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Process;
using VinClean.Service.Service;

namespace VinCleanDemo2.Controllers
{
    // API Deploy

    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;
        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderModeDTO>>> Order()
        {
            return Ok(await _service.GetOrderList());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Order>> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var processFound = await _service.GetOrderById(id);
            if (processFound == null)
            {
                return NotFound();
            }
            return Ok(processFound);
        }

        [HttpGet("GetALL/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderModeDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<OrderModeDTO>> GetAllInfo(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var processFound = await _service.GetAllInfoById(id);
            if (processFound == null)
            {
                return NotFound();
            }
            return Ok(processFound);
        }
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(NewBooking request)
        {


            var newProcess = await _service.AddOrder(request);
            if (newProcess.Success == false && newProcess.Message == "Exist")
            {
                return Ok(newProcess);
            }

            if (newProcess.Success == false && newProcess.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when adding Process {request}");
                return StatusCode(500, ModelState);
            }

            if (newProcess.Success == false && newProcess.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when adding Process {request}");
                return StatusCode(500, ModelState);
            }
            return Ok(newProcess.Data);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateProcess(OrderDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateProcess = await _service.UpdateOrder(request);

            if (updateProcess.Success == false && updateProcess.Message == "NotFound")
            {
                return Ok(updateProcess);
            }

            if (updateProcess.Success == false && updateProcess.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating Process {request}");
                return StatusCode(500, ModelState);
            }

            if (updateProcess.Success == false && updateProcess.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating Process {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateProcess);

        }

        [HttpPut("StartWorking")]
        public async Task<ActionResult> UpdateStartWorking(ProcessStartWorking request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateProcess = await _service.UpdateStartWorking(request);

            if (updateProcess.Success == false && updateProcess.Message == "NotFound")
            {
                return Ok(updateProcess);
            }

            if (updateProcess.Success == false && updateProcess.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating Process {request}");
                return StatusCode(500, ModelState);
            }

            if (updateProcess.Success == false && updateProcess.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating Process {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateProcess);

        }

        [HttpPut("EndWorking")]
        public async Task<ActionResult> UpdateEndWorking(ProcessEndWorking request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateProcess = await _service.UpdateEndWorking(request);

            if (updateProcess.Success == false && updateProcess.Message == "NotFound")
            {
                return Ok(updateProcess);
            }

            if (updateProcess.Success == false && updateProcess.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating Process {request}");
                return StatusCode(500, ModelState);
            }

            if (updateProcess.Success == false && updateProcess.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating Process {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateProcess);

        }

        [HttpPut("StatusCompleted")]
        public async Task<ActionResult> UpdateStatus(int processid)
        {
            //if (request == null)
            //{
            //    return BadRequest(ModelState);
            //}


            var updateProcess = await _service.UpdateStatusCompleted(processid);

            if (updateProcess.Success == false && updateProcess.Message == "NotFound")
            {
                return Ok(updateProcess);
            }

            if (updateProcess.Success == false && updateProcess.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating Process ");
                return StatusCode(500, ModelState);
            }

            if (updateProcess.Success == false && updateProcess.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating Process ");
                return StatusCode(500, ModelState);
            }


            return Ok(updateProcess);

        }

        [HttpPut("Denied")]
        public async Task<ActionResult> DeniedProcess(int processid)
        {
            //if (request == null)
            //{
            //    return BadRequest(ModelState);
            //}


            var updateProcess = await _service.DeniedOrder(processid);

            if (updateProcess.Success == false && updateProcess.Message == "NotFound")
            {
                return Ok(updateProcess);
            }

            if (updateProcess.Success == false && updateProcess.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating Process ");
                return StatusCode(500, ModelState);
            }

            if (updateProcess.Success == false && updateProcess.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating Process ");
                return StatusCode(500, ModelState);
            }


            return Ok(updateProcess);

        }

        [HttpPut("SubPrice")]
        public async Task<ActionResult> UpdateSubPrice(UpdateSubPirce request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateProcess = await _service.UpdateSubPrice(request);

            if (updateProcess.Success == false && updateProcess.Message == "NotFound")
            {
                return Ok(updateProcess);
            }

            if (updateProcess.Success == false && updateProcess.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating Process {request}");
                return StatusCode(500, ModelState);
            }

            if (updateProcess.Success == false && updateProcess.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating Process {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateProcess);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProcess(int id)
        {
            var deleteProcess = await _service.DeleteOrder(id);


            if (deleteProcess.Success == false && deleteProcess.Message == "NotFound")
            {
                ModelState.AddModelError("", "Process Not found");
                return StatusCode(404, ModelState);
            }

            if (deleteProcess.Success == false && deleteProcess.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in Repository when deleting Process");
                return StatusCode(500, ModelState);
            }

            if (deleteProcess.Success == false && deleteProcess.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when deleting Process");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
    }
}

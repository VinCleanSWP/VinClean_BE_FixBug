using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VinClean.Service.DTO.ProcessImage;
using VinClean.Service.DTO.WorkingSlot;
using VinClean.Service.Service;

namespace VinClean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderImageController : ControllerBase
    {
        private readonly IOrderImageService _service;
        public OrderImageController(IOrderImageService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<OrderImageDTO>>> Get()
        {
            return Ok(await _service.OrderImageList());
        }
        [HttpGet("Order/{id}")]
        public async Task<ActionResult<List<OrderImageDTO>>> GetbyOrderId(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var OrderImage = await _service.OrderImageByOrderId(id);
            if (OrderImage == null)
            {
                return NotFound();
            }
            return Ok(OrderImage);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderImageDTO>> GetbyId(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var OrderImage = await _service.OrderImageById(id);
            if (OrderImage == null)
            {
                return NotFound();
            }
            return Ok(OrderImage);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateOrderImage(OrderImageDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateOrderImg = await _service.UpdateOrderImage(request);

            if (updateOrderImg.Success == false && updateOrderImg.Message == "NotFound")
            {
                return Ok(updateOrderImg);
            }

            if (updateOrderImg.Success == false && updateOrderImg.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating account {request}");
                return StatusCode(500, ModelState);
            }

            if (updateOrderImg.Success == false && updateOrderImg.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating account {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateOrderImg);

        }

        [HttpPut("UpdateImage")]
        public async Task<ActionResult> UpdateImage(UpdateImage request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateOrderImg = await _service.UpdateImage(request);

            if (updateOrderImg.Success == false && updateOrderImg.Message == "NotFound")
            {
                return Ok(updateOrderImg);
            }

            if (updateOrderImg.Success == false && updateOrderImg.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating account {request}");
                return StatusCode(500, ModelState);
            }

            if (updateOrderImg.Success == false && updateOrderImg.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating account {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateOrderImg);

        }

        [HttpPost]
        public async Task<ActionResult<OrderImageDTO>> AddWB(OrderImageDTO request)
        {
            /*            if(request == null)
                        {
                            return BadRequest(ModelState);
                        }
                        if(ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }*/

            var newPImg = await _service.AddOrderImage(request);
            if (newPImg.Success == false && newPImg.Message == "Exist")
            {
                return Ok(newPImg);
            }

            if (newPImg.Success == false && newPImg.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when adding Account {request}");
                return StatusCode(500, ModelState);
            }

            if (newPImg.Success == false && newPImg.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when adding Account {request}");
                return StatusCode(500, ModelState);
            }
            return Ok(newPImg.Data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrderImage(int id)
        {
            var deletePImg = await _service.DeleteOrderImage(id);


            if (deletePImg.Success == false && deletePImg.Message == "NotFound")
            {
                ModelState.AddModelError("", "Account Not found");
                return StatusCode(404, ModelState);
            }

            if (deletePImg.Success == false && deletePImg.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in Repository when deleting account");
                return StatusCode(500, ModelState);
            }

            if (deletePImg.Success == false && deletePImg.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when deleting account");
                return StatusCode(500, ModelState);
            }

            return Ok(deletePImg);

        }
    }
}

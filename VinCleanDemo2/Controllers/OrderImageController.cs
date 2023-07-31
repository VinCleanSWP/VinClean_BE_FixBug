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
        private readonly IProcessImageService _service;
        public OrderImageController(IProcessImageService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<ProcessImageDTO>>> Get()
        {
            return Ok(await _service.ProcessImageList());
        }
        [HttpGet("Process/{id}")]
        public async Task<ActionResult<List<ProcessImageDTO>>> GetbyProcessId(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var ProcessImage = await _service.ProcessImageByProcessId(id);
            if (ProcessImage == null)
            {
                return NotFound();
            }
            return Ok(ProcessImage);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProcessImageDTO>> GetbyId(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var ProcessImage = await _service.ProcessImageById(id);
            if (ProcessImage == null)
            {
                return NotFound();
            }
            return Ok(ProcessImage);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateProcessImage(ProcessImageDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateProcessImg = await _service.UpdateProcessImage(request);

            if (updateProcessImg.Success == false && updateProcessImg.Message == "NotFound")
            {
                return Ok(updateProcessImg);
            }

            if (updateProcessImg.Success == false && updateProcessImg.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating account {request}");
                return StatusCode(500, ModelState);
            }

            if (updateProcessImg.Success == false && updateProcessImg.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating account {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateProcessImg);

        }

        [HttpPut("UpdateImage")]
        public async Task<ActionResult> UpdateImage(UpdateImage request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateProcessImg = await _service.UpdateImage(request);

            if (updateProcessImg.Success == false && updateProcessImg.Message == "NotFound")
            {
                return Ok(updateProcessImg);
            }

            if (updateProcessImg.Success == false && updateProcessImg.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating account {request}");
                return StatusCode(500, ModelState);
            }

            if (updateProcessImg.Success == false && updateProcessImg.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating account {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateProcessImg);

        }

        [HttpPost]
        public async Task<ActionResult<ProcessImageDTO>> AddWB(ProcessImageDTO request)
        {
            /*            if(request == null)
                        {
                            return BadRequest(ModelState);
                        }
                        if(ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }*/

            var newPImg = await _service.AddProcessImage(request);
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
        public async Task<ActionResult> DeleteProcessImage(int id)
        {
            var deletePImg = await _service.DeleteProcessImage(id);


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

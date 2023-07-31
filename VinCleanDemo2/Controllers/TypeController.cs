using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VinClean.Repo.Models;
using VinClean.Service.DTO;
using VinClean.Service.DTO.CustomerResponse;
using VinClean.Service.DTO.Type;
using VinClean.Service.Service;

namespace VinClean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        public readonly ITypeService _service;
        public TypeController(ITypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<TypeDTO>>> Get()
        {
            return Ok(await _service.GetTypeList());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypeDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TypeDTO>> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var typeFound = await _service.GetTypeById(id);
            if (typeFound == null)
            {
                return NotFound();
            }
            return Ok(typeFound);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateAccount(TypeDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateType = await _service.UpdateType(request);

            if (updateType.Success == false && updateType.Message == "NotFound")
            {
                return Ok(updateType);
            }

            if (updateType.Success == false && updateType.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating account {request}");
                return StatusCode(500, ModelState);
            }

            if (updateType.Success == false && updateType.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating account {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateType);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> SoftDeleteAccount(int id)
        {
            var deleteType = await _service.DeleteType(id);


            if (deleteType.Success == false && deleteType.Message == "NotFound")
            {
                ModelState.AddModelError("", "Account Not found");
                return StatusCode(404, ModelState);
            }

            if (deleteType.Success == false && deleteType.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in Repository when deleting account");
                return StatusCode(500, ModelState);
            }

            if (deleteType.Success == false && deleteType.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when deleting account");
                return StatusCode(500, ModelState);
            }

            return Ok(deleteType);
        }



            [HttpPost]
            public async Task<ActionResult> AddService(TypeDTO request)
            {
                var deleteType = await _service.AddType(request);


                if (deleteType.Success == false && deleteType.Message == "NotFound")
                {
                    ModelState.AddModelError("", "Account Not found");
                    return StatusCode(404, ModelState);
                }

                if (deleteType.Success == false && deleteType.Message == "RepoError")
                {
                    ModelState.AddModelError("", $"Some thing went wrong in Repository when deleting account");
                    return StatusCode(500, ModelState);
                }

                if (deleteType.Success == false && deleteType.Message == "Error")
                {
                    ModelState.AddModelError("", $"Some thing went wrong in service layer when deleting account");
                    return StatusCode(500, ModelState);
                }

                return Ok(deleteType);

            }
        }
}

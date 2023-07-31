using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VinClean.Repo.Models;
using VinClean.Service.DTO.Role;
using VinClean.Service.DTO;
using VinClean.Service.Service;

namespace VinClean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _service;
        public RoleController(IRoleService service)
        {
            _service = service;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<RoleDTO>>> GetRoleList()
        {
            return Ok(await _service.GetRoleList());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Role>> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var RoleFound = await _service.GetRoleById(id);
            if (RoleFound == null)
            {
                return NotFound();
            }
            return Ok(RoleFound);
        }


        [HttpPost]
        public async Task<ActionResult<Role>> CreateRole(RoleDTO request)
        {
            /*            if(request == null)
                        {
                            return BadRequest(ModelState);
                        }
                        if(ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }*/

            var newRole = await _service.AddRole(request);
            if (newRole.Success == false && newRole.Message == "Exist")
            {
                return Ok(newRole);
            }

            if (newRole.Success == false && newRole.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when adding Role {request}");
                return StatusCode(500, ModelState);
            }

            if (newRole.Success == false && newRole.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when adding Role {request}");
                return StatusCode(500, ModelState);
            }
            return Ok(newRole.Data);
        }


        [HttpPut]
        public async Task<ActionResult> UpdateRole(RoleDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateRole = await _service.UpdateRole(request);

            if (updateRole.Success == false && updateRole.Message == "NotFound")
            {
                return Ok(updateRole);
            }

            if (updateRole.Success == false && updateRole.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating Role {request}");
                return StatusCode(500, ModelState);
            }

            if (updateRole.Success == false && updateRole.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating Role {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateRole);

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRole(int id)
        {
            var deleteRole = await _service.DeleteRole(id);


            if (deleteRole.Success == false && deleteRole.Message == "NotFound")
            {
                ModelState.AddModelError("", "Role Not found");
                return StatusCode(404, ModelState);
            }

            if (deleteRole.Success == false && deleteRole.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in Repository when deleting Role");
                return StatusCode(500, ModelState);
            }

            if (deleteRole.Success == false && deleteRole.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when deleting Role");
                return StatusCode(500, ModelState);
            }

            return NoContent();


        }
    }
}

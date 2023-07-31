using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VinClean.Repo.Models;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using VinClean.Service.DTO.CustomerResponse;
using VinClean.Service.Service;

namespace VinClean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;
        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Customer>>> Get()
        {
            return Ok(await _service.GetCustomerList());
        }
        [HttpGet("Search")]
        public async Task<ActionResult<List<Customer>>> SearchNameorId(string search)
        {
            return Ok(await _service.SearchNameorId(search));
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountdDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Account>> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var accountFound = await _service.GetCustomerById(id);
            if (accountFound == null)
            {
                return NotFound();
            }
            return Ok(accountFound);
        }
        [HttpGet("Account/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountdDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Account>> GetCustomerAcById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var accountFound = await _service.GetCustomerAcById(id);
            if (accountFound == null)
            {
                return NotFound();
            }
            return Ok(accountFound);
        }

        [HttpPost("registration")]
        public async Task<ActionResult<Customer>> Registration (RegisterDTO request)
        {
/*            if (request == null)
            {
                return BadRequest(ModelState);
            }*/
/*            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/

            var newAccount = await _service.Register(request);
            if (newAccount.Success == false && newAccount.Message == "Exist")
            {
                return StatusCode(500, ModelState);
            }

            if (newAccount.Success == false && newAccount.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when adding Account {request}");
                return StatusCode(500, ModelState);
            }

            if (newAccount.Success == false && newAccount.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when adding Account {request}");
                return StatusCode(500, ModelState);
            }
            return Ok(newAccount.Data);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAccount(UpdateDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateAccount = await _service.UpdateCustomer(request);

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

        [HttpGet("ListViewProfile")]
        public async Task<ActionResult<List<Customer>>> GetProfileList()
        {
            return Ok(await _service.GetViewProfileList());
        }

        [HttpGet("GetProfileBy {id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountdDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Account>> GetPId(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var profileFound = await _service.GetProfileByID(id);
            if (profileFound == null)
            {
                return NotFound();
            }
            return Ok(profileFound);
        }

        [HttpPut("ModifyProfile")]
        public async Task<ActionResult> ModifyProfile(ModifyCustomerProfileDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var editProfile = await _service.ModifyProfile(request);

            if (editProfile.Success == false && editProfile.Message == "NotFound")
            {
                return Ok(editProfile);
            }

            if (editProfile.Success == false && editProfile.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating account {request}");
                return StatusCode(500, ModelState);
            }

            if (editProfile.Success == false && editProfile.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating account {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(editProfile);

        }



    }
}

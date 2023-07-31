using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using VinClean.Repo.Models;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Account;

using VinClean.Service.Service;

namespace VinCleanDemo2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;
        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<AccountdDTO>>> Get()
        {
            return Ok(await _service.GetAccountList());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountdDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Account>> GetById(int id)
        {
            if(id <= 0)
            {
                return BadRequest(id);
            }
            var accountFound = await _service.GetAccountById(id);
            if(accountFound == null)
            {
                return NotFound();
            }
            return Ok(accountFound);
        }

        [HttpPost]
        public async Task<ActionResult<Account>> AddAccount(AccountdDTO request)
        {
/*            if(request == null)
            {
                return BadRequest(ModelState);
            }
            if(ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/

            var newAccount = await _service.AddAccount(request);
            if(newAccount.Success == false && newAccount.Message == "Exist")
            {
                return Ok(newAccount);
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

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<Account>> Login(LoginDTO request)
        {
          

            var newAccount = await _service.Login(request.Email, request.Password);
            if (newAccount.Success == true && newAccount.Message == "OK")
            {
                return Ok(newAccount);
            }

            if (newAccount.Success == false && newAccount.Message == "NotFound")
            {
                ModelState.AddModelError("", $"Some thing went wrong with Email or Password");
                return StatusCode(500, ModelState);
            }

            if (newAccount.Success == false && newAccount.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when adding Account {request}");
                return StatusCode(500, ModelState);
            }
            return Ok(newAccount);
        }

        [HttpPost]
        [Route("Verify")]
        public async Task<ActionResult<Account>> Verify(string token)
        {


            var account = await _service.Verify(token);
            if (account.Success == true && account.Message == "OK")
            {
                return Ok(account);
            }

            if (account.Success == false && account.Message == "InvalidToken")
            {
                ModelState.AddModelError("", $"Invalid Token");
                return StatusCode(500, ModelState);
            }

            if (account.Success == false && account.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when adding Account {token}");
                return StatusCode(500, ModelState);
            }
            return Ok(account);
        }

        [HttpPost]
        [Route("forgot-password")]
        public async Task<ActionResult<Account>> ForgotPassword(string email)
        {


            var account = await _service.ForgotPassword(email);
            if (account.Success == true && account.Message == "OK")
            {
                return Ok(account);
            }

            if (account.Success == false && account.Message == "NotFound")
            {
                ModelState.AddModelError("", $"Email {email} is not existed");
                return StatusCode(500, ModelState);
            }

            if (account.Success == false && account.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when adding Account {email}");
                return StatusCode(500, ModelState);
            }
            return Ok(account);
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<ActionResult<Account>> ResetPassword(ResetPasswordReqsuet request)
        {


            var account = await _service.ResetPassword(request);
            if (account.Success == true && account.Message == "OK")
            {
                return Ok(account);
            }

            if (account.Success == false && account.Message == "InvalidToken")
            {
                ModelState.AddModelError("", $"Token is not correct! Please Check again");
                return StatusCode(500, ModelState);
            }

            if (account.Success == false && account.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer ");
                return StatusCode(500, ModelState);
            }
            return Ok(account);
        }


        [HttpPut]
        public async Task<ActionResult> UpdateAccount(AccountdDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateAccount = await _service.UpdateAccount(request);

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
        public async Task<ActionResult> SoftDeleteAccount(int id)
        {
            var deleteAccount = await _service.SoftDeleteAccount(id);


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



        /// <summary>
        /// Đang thử nghiệm. Đừng có làm theo chức năng này
        /// Đang thử nghiệm. Đừng có làm theo chức năng này
        /// Đang thử nghiệm. Đừng có làm theo chức năng này
        /// </summary>
        [HttpDelete("HardDelete/{id}")]
        public async Task<ActionResult> HardDeleteAccount(int id)
        {
            var deleteAccount = await _service.HardDeleteAccount(id);


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

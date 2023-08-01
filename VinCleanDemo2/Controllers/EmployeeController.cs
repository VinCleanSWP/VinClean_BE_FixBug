using Microsoft.AspNetCore.Mvc;
using VinClean.Repo.Models;
using VinClean.Repo.Models.ProcessModel;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Employee;
using VinClean.Service.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VinClean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;
        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }
        // GET: api/<EmployeeController>
        [HttpGet]
        public async Task<ActionResult<List<EmployeeDTO>>> GetAllEmployee()
        {
            return Ok(await _service.GetEmployeeList());
        }

        [HttpGet("Search")]
        public async Task<ActionResult<List<EmployeeDTO>>> SearchEmployee(string search)
        {
            return Ok(await _service.SearchEmployee(search));
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var EmployeeFound = await _service.GetEmployeeById(id);
            if (EmployeeFound == null)
            {
                return NotFound();
            }
            return Ok(EmployeeFound);
        }
        //POST api/<EmployeeController>/
        [HttpPost("selectemployee")]
        public async Task<ActionResult<List<Employee>>> SelectEmployee(SelectEmpDTO request)
        {

            var response = await _service.SelectEmployeeList(request);

            if (!response.Success)
            {
                return NotFound(response.Message);
            }

            return Ok(response.Data);
        }
        //    // POST api/<EmployeeController>
        //[HttpPost]
        //public async Task<ActionResult<Employee>> AddEmployee(RegisterEmployeeDTO request)
        //{
        //    var newEmployee = await _service.AddEmployee(request);
        //    if (newEmployee.Success == false && newEmployee.Message == "Exist")
        //    {
        //        ModelState.AddModelError("", $"Email {request} is Existed");
        //        return StatusCode(500, ModelState);
        //    }

        //    if (newEmployee.Success == false && newEmployee.Message == "RepoError")
        //    {
        //        ModelState.AddModelError("", $"Some thing went wrong in respository layer when adding Employee {request}");
        //        return StatusCode(500, ModelState);
        //    }

        //    if (newEmployee.Success == false && newEmployee.Message == "Error")
        //    {
        //        ModelState.AddModelError("", $"Some thing went wrong in service layer when adding Employee {request}");
        //        return StatusCode(500, ModelState);
        //    }
        //    return Ok(newEmployee.Data);
        //}

        // PUT api/<EmployeeController>/5
        [HttpPut]
        public async Task<ActionResult> UpdateEmployee(UpdateEmployeeDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateEmployee = await _service.UpdateEmployee(request);

            if (updateEmployee.Success == false && updateEmployee.Message == "NotFound")
            {
                return Ok(updateEmployee);
            }

            if (updateEmployee.Success == false && updateEmployee.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating Employee {request}");
                return StatusCode(500, ModelState);
            }

            if (updateEmployee.Success == false && updateEmployee.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating Employee {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateEmployee);

        }
        // DELETE api/<EmployeeController>/5
        [HttpDelete("SoftDelete/{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            var deleteEmployee = await _service.DeleteEmployee(id);


            if (deleteEmployee.Success == false && deleteEmployee.Message == "NotFound")
            {
                ModelState.AddModelError("", "Employee Not found");
                return StatusCode(404, ModelState);
            }

            if (deleteEmployee.Success == false && deleteEmployee.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in Repository when deleting Employee");
                return StatusCode(500, ModelState);
            }

            if (deleteEmployee.Success == false && deleteEmployee.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when deleting Employee");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> SoftDeleteEmployee(int id)
        {
            var deleteEmployee = await _service.SoftDeleteEmployee(id);


            if (deleteEmployee.Success == false && deleteEmployee.Message == "NotFound")
            {
                ModelState.AddModelError("", "Employee Not found");
                return StatusCode(404, ModelState);
            }

            if (deleteEmployee.Success == false && deleteEmployee.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in Repository when deleting Employee");
                return StatusCode(500, ModelState);
            }

            if (deleteEmployee.Success == false && deleteEmployee.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when deleting Employee");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
    }
}

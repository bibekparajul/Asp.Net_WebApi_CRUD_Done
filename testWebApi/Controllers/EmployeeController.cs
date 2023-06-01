using DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using testWebApi.Repository;

namespace testWebApi.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]

        public async Task<ActionResult> GetEmployees()
        {
            try
            {
                return Ok(await _employeeRepository.GetEmployees());

            }
            catch (Exception)
            {
                return StatusCode
                    (StatusCodes.Status500InternalServerError,
                    "Error in Retrieving Data from Database");
            }
        }

        [HttpGet("{id:int}")]

        public async Task<ActionResult<Employee>> GetEmployees(int id)
        {
            try
            {
                var result = await _employeeRepository.GetEmployees(id);
                if (result == null)
                {
                    return NotFound();
                }
                return result;

            }
            catch (Exception)
            {
                return StatusCode
                    (StatusCodes.Status500InternalServerError,
                    "Error in Retrieving Data from Database");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest();
                }
                var CreatedEmployee = await _employeeRepository.AddEmployee(employee);
                return CreatedAtAction(nameof(GetEmployees), new { id = CreatedEmployee.Id }, CreatedEmployee);


            }
            catch (Exception)
            {
                return StatusCode
                    (StatusCodes.Status500InternalServerError,
                    "Error in Retrieving Data from Database");
            }
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id,Employee employee)
        {
            try
            {
                if(id != employee.Id)
                {
                    return BadRequest("Id Mismatch");
                }

                var employeeUpdate = await _employeeRepository.GetEmployees(id);
                if (employeeUpdate == null)
                {
                    return NotFound($"Employee Id={id} not found");
                }

                return await _employeeRepository.UpdateEmployee(employee);


            }
            catch (Exception)
            {
                return StatusCode
                    (StatusCodes.Status500InternalServerError,
                    "Error in Retrieving Data from Database");
            }
        }



        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            try
            {
           

                var employeeDelete = await _employeeRepository.GetEmployees(id);
                if (employeeDelete == null)
                {
                    return NotFound($"Employee Id={id} not found");
                }

                return await _employeeRepository.DeleteEmployee(id);


            }
            catch (Exception)
            {
                return StatusCode
                    (StatusCodes.Status500InternalServerError,
                    "Error in Retrieving Data from Database");
            }
        }


        [HttpGet("{search}")]
        public async Task<ActionResult<IEnumerable<Employee>>> Search(string name)
        {

            try
            {
                var result = await _employeeRepository.Search(name);

                if (result.Any())
                {
                    return Ok(result);

                }
                return NotFound();

            }
            catch (Exception)
            {
                return StatusCode
                    (StatusCodes.Status500InternalServerError,
                    "Error in Retrieving Data from Database");
            }

        }




    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using talabat.core.Entites;
using talabat.core.Repositories;
using talabat.core.Specifications;
using talabat.core.Specifications.Employee_specs;

namespace talabat.Apis.Controllers
{

    public class EmployeeController : ApiBaseController
    {
        private readonly iGenericRepository<Employee> _employeesRepo;

        public EmployeeController(iGenericRepository<Employee> EmployeesRepo)
        {
            _employeesRepo = EmployeesRepo;
        }



        [HttpGet] // Get :Api/Employees
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var spec = new EmployeeWithDepartmentSpecification();
            var employee = await _employeesRepo.Getallwithspecasync(spec);
            return Ok(employee);
        }

        [HttpGet ("{id}")] // Get :Api/Employees/1
        public async Task<ActionResult<Employee>> GetEmployees(int id)
        {
            var spec = new EmployeeWithDepartmentSpecification(id);
            var employee = await _employeesRepo.Getallwithspecasync(spec);
            return Ok(employee);

        }


    }
}

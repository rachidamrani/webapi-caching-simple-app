using Microsoft.AspNetCore.Mvc;


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
    public async Task<IActionResult> GetAllEmployees() => Ok(await _employeeRepository.GetAllEmployees());

    [HttpPost]
    public async Task<IActionResult> AddNewEmployee(Employee employee)
    {
        await _employeeRepository.AddEmployee(employee);
        return NoContent();
    }
}
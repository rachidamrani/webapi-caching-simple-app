using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;


[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly AppDBContext _appDBContext;
    private readonly IMemoryCache _memoryCache;

    public EmployeeController(AppDBContext appDBContext, IMemoryCache memoryCache)
    {
        _appDBContext = appDBContext;
        _memoryCache = memoryCache;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var cachkey = "employeesCache";
        IEnumerable<Employee>? employees;

        if (!_memoryCache.TryGetValue(cachkey, out employees))
        {
            // Cach miss : fetch data from databse if the cach is empty
            employees = await _appDBContext.Employees.ToListAsync();

            // Set cache entry with absolute expiration or sliding expiration

            MemoryCacheEntryOptions cachEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            _memoryCache.Set(cachkey, employees, cachEntryOptions);

            return Ok(employees);
        }


        return Ok(employees);

        // List<Employee> employees = await _appDBContext.Employees.ToListAsync();
        // return Ok(employees);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee(Employee employee)
    {
        employee.Id = Guid.NewGuid();
        await _appDBContext.Employees.AddAsync(employee);
        await _appDBContext.SaveChangesAsync();

        return Ok("Employee created successfully.");
    }
}
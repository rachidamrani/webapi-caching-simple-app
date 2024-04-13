using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

public class EmployeeRepository : IEmployeeRepository
{

    private readonly AppDBContext _appDBContext;
    private readonly IMemoryCache _memoryCache;

    public EmployeeRepository(AppDBContext appDBContext, IMemoryCache memoryCache)
    {
        _appDBContext = appDBContext;
        _memoryCache = memoryCache;
    }

    // Add new employee
    public async Task AddEmployee(Employee employee)
    {
        employee.Id = Guid.NewGuid();
        await _appDBContext.Employees.AddAsync(employee);
        await _appDBContext.SaveChangesAsync();
    }

    // Get all employees
    public async Task<IEnumerable<Employee>> GetAllEmployees()
    {
        IEnumerable<Employee> employees;

        employees = _memoryCache.Get<IEnumerable<Employee>>("employeesCache")!;

        if (employees is not null)
        {
            return employees;
        }

        employees = await _appDBContext.Employees.ToListAsync();
        _memoryCache.Set("employeesCache", employees, TimeSpan.FromMinutes(1));
        return employees;
    }
}
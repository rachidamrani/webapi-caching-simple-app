using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        string cachkey = "employeesCache";
        IEnumerable<Employee>? employees;

        bool getValueFromCache = _memoryCache.TryGetValue(cachkey, out employees);

        if (!getValueFromCache)
        {
            // Cach miss : fetch data from databse if the cach is empty
            employees = await _appDBContext.Employees.ToListAsync();

            // Set cache entry with absolute expiration or sliding expiration

            MemoryCacheEntryOptions cachEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
            _memoryCache.Set(cachkey, employees, cachEntryOptions);

            return employees;
        }

        return employees;
    }
}
using Microsoft.AspNetCore.Mvc;

public interface IEmployeeRepository
{
    Task AddEmployee(Employee employee);
    Task<IEnumerable<Employee>> GetAllEmployees();
}
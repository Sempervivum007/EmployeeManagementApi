using EmployeeManagement.Models;

namespace EmployeeManagement.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(string id);
        Task CreateAsync(/*IClientSessionHandle session,*/ Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(string id);
        Task<List<Employee>> GetFilteredAsync(string? departmentId, string? positionId,
            DateTime? hireDateFrom, DateTime? hireDateTo, List<string>? personalInfoIds = null);
    }
}
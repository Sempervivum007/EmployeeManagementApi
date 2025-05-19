using EmployeeManagement.Models;

namespace EmployeeManagement.Repositories.Interfaces
{
    public interface IPositionRepository
    {
        Task<IEnumerable<Position>> GetAllAsync();
        Task<Position> GetByIdAsync(string id);
        Task CreateAsync(Position position);
        Task UpdateAsync(Position position);
        Task DeleteAsync(string id);
    }
}
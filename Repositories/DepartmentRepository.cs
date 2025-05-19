using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interfaces;
using MongoDB.Driver;

namespace EmployeeManagement.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly IMongoCollection<Department> _departments;

        public DepartmentRepository(IMongoDatabase database)
        {
            _departments = database.GetCollection<Department>("Departments");
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _departments.Find(_ => true).ToListAsync();
        }

        public async Task<Department> GetByIdAsync(string id)
        {
            return await _departments.Find(d => d.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Department department)
        {
            await _departments.InsertOneAsync(department);
        }

        public async Task UpdateAsync(Department department)
        {
            await _departments.ReplaceOneAsync(d => d.Id == department.Id, department);
        }

        public async Task DeleteAsync(string id)
        {
            await _departments.DeleteOneAsync(d => d.Id == id);
        }
    }
}
using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EmployeeManagement.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IMongoCollection<Employee> _employee;

        public EmployeeRepository(IMongoDatabase database)
        {
            _employee = database.GetCollection<Employee>("Employees");
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _employee.Find(_ => true).ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(string id)
        {
            return await _employee.Find(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(/*IClientSessionHandle session,*/ Employee employee)
        {
            employee.HireDate = employee.HireDate.ToLocalTime();
            await _employee.InsertOneAsync(/*session,*/ employee);
        }

        public async Task UpdateAsync(/*IClientSessionHandle session,*/ Employee employee)
        {
            employee.HireDate = employee.HireDate.ToLocalTime();
            await _employee.ReplaceOneAsync(e => e.Id == employee.Id, employee);
        }

        public async Task DeleteAsync(/*IClientSessionHandle session,*/ string id)
        {
            await _employee.DeleteOneAsync(e => e.Id == id);
        }

        public async Task<List<Employee>> GetFilteredAsync(string? departmentId, string? positionId,
            DateTime? hireDateFrom, DateTime? hireDateTo, List<string>? personalInfoIds = null)
        {
            var filterBuilder = Builders<Employee>.Filter;
            var filter = filterBuilder.Empty;

            if (!string.IsNullOrEmpty(departmentId))
                filter &= filterBuilder.Eq(e => e.DepartmentId, departmentId);

            if (!string.IsNullOrEmpty(positionId))
                filter &= filterBuilder.Eq(e => e.PositionId, positionId);

            if (hireDateFrom.HasValue)
                filter &= filterBuilder.Gte(e => e.HireDate, hireDateFrom.Value.Date);

            if (hireDateTo.HasValue)
                filter &= filterBuilder.Lte(e => e.HireDate, hireDateTo.Value.Date);

            if (personalInfoIds != null && personalInfoIds.Any())
                filter &= filterBuilder.In(x => x.PersonalInfoId, personalInfoIds);

            return await _employee.Find(filter).ToListAsync();
        }
    }
}
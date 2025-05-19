using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interfaces;
using MongoDB.Driver;

namespace EmployeeManagement.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        private readonly IMongoCollection<Position> _positions;

        public PositionRepository(IMongoDatabase database)
        {
            _positions = database.GetCollection<Position>("Positions");
        }

        public async Task<IEnumerable<Position>> GetAllAsync()
        {
            return await _positions.Find(_ => true).ToListAsync();
        }

        public async Task<Position> GetByIdAsync(string id)
        {
            return await _positions.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Position position)
        {
            await _positions.InsertOneAsync(position);
        }

        public async Task UpdateAsync(Position position)
        {
            await _positions.ReplaceOneAsync(p => p.Id == position.Id, position);
        }

        public async Task DeleteAsync(string id)
        {
            await _positions.DeleteOneAsync(p => p.Id == id);
        }
    }
}
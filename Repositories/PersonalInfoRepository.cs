using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EmployeeManagement.Repositories
{
    public class PersonalInfoRepository : IPersonalInfoRepository
    {
        private readonly IMongoCollection<PersonalInfo> _personalInfo;

        public PersonalInfoRepository(IMongoDatabase database)
        {
            _personalInfo = database.GetCollection<PersonalInfo>("PersonalInfo");
        }

        public async Task<IEnumerable<PersonalInfo>> GetAllAsync()
        {
            return await _personalInfo.Find(_ => true).ToListAsync();
        }

        public async Task<PersonalInfo> GetByIdAsync(string id)
        {
            return await _personalInfo.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(PersonalInfo personalInfo/*,IClientSessionHandle session*/)
        {
            await _personalInfo.InsertOneAsync(/*session,*/ personalInfo);
        }

        public async Task UpdateAsync(PersonalInfo personalInfo)
        {
            await _personalInfo.ReplaceOneAsync(p => p.Id == personalInfo.Id, personalInfo);
        }

        public async Task DeleteAsync(string id)
        {
            await _personalInfo.DeleteOneAsync(p => p.Id == id);
        }

        public async Task<List<string>> SearchIdsByFullNameAsync(string fullName)
        {
            var filter = Builders<PersonalInfo>.Filter.Regex(x => x.FullName, new BsonRegularExpression(fullName, "i"));
            var projection = Builders<PersonalInfo>.Projection.Include(x => x.Id);
            
            var result = await _personalInfo.Find(filter).Project<PersonalInfo>(projection).ToListAsync();
            return result.Select(x => x.Id).ToList();
        }
    }
}
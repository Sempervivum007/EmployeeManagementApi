using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interfaces;
using MongoDB.Driver;

namespace EmployeeManagement.Repositories
{
    public class CompanyInfoRepository : ICompanyInfoRepository
    {
        private readonly IMongoCollection<CompanyInfo> _companyInfo;

        public CompanyInfoRepository(IMongoDatabase database)
        {
            _companyInfo = database.GetCollection<CompanyInfo>("CompanyInfo");
        }

        public async Task<CompanyInfo?> GetAsync()
        {
            return await _companyInfo.Find(_ => true).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(CompanyInfo companyInfo)
        {
            await _companyInfo.ReplaceOneAsync(c => c.Id == companyInfo.Id, companyInfo);
        }

        public async Task CreateAsync(CompanyInfo companyInfo)
        {
            await _companyInfo.InsertOneAsync(companyInfo);
        }
    }
}
using EmployeeManagement.Models;

namespace EmployeeManagement.Repositories.Interfaces
{
    public interface ICompanyInfoRepository
    {
        Task<CompanyInfo?> GetAsync();
        Task UpdateAsync(CompanyInfo companyInfo);
        Task CreateAsync(CompanyInfo companyInfo);
    }
}
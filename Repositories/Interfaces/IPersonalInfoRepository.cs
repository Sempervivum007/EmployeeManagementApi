using EmployeeManagement.Models;
using MongoDB.Driver;

namespace EmployeeManagement.Repositories.Interfaces
{
    public interface IPersonalInfoRepository
    {
        Task<IEnumerable<PersonalInfo>> GetAllAsync();
        Task<PersonalInfo> GetByIdAsync(string id);
        Task/*<string?>*/ CreateAsync(PersonalInfo personalInfo/*,IClientSessionHandle session*/);
        Task UpdateAsync(PersonalInfo personalInfo);
        Task DeleteAsync(string id);
        Task<List<string>> SearchIdsByFullNameAsync(string fullName);
    }
}
using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Pages.Departments
{
    public class IndexModel : PageModel
    {
        private readonly IDepartmentRepository _departmentRepository;

        public IndexModel(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public List<Department> Departments { get; set; } = [];

        public async Task OnGetAsync()
        {
            var departments = await _departmentRepository.GetAllAsync();
            Departments = departments.ToList();
        }
    }
}
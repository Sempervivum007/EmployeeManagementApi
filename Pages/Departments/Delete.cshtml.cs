using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Pages.Departments
{
    public class DeleteModel : PageModel
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DeleteModel(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [BindProperty]
        public Department Department { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            Department = department;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Department.Id == null)
                return NotFound();

            await _departmentRepository.DeleteAsync(Department.Id);
            return RedirectToPage("./Index");
        }
    }
}
using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Pages.Departments
{
    public class EditModel : PageModel
    {
        private readonly IDepartmentRepository _departmentRepository;

        public EditModel(IDepartmentRepository departmentRepository)
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
            if (!ModelState.IsValid)
            {
#if DEBUG
                foreach (var modelState in ModelState)
                {
                    foreach (var error in modelState.Value.Errors)
                    {
                        Console.WriteLine($"Key: {modelState.Key}, Error: {error.ErrorMessage}");
                    }
                }
#endif

                return Page();
            }

            await _departmentRepository.UpdateAsync(Department);
            return RedirectToPage("Index");
        }
    }
}
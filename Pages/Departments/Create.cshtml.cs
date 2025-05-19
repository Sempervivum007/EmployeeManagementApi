using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Pages.Departments
{
    public class CreateModel : PageModel
    {
        private readonly IDepartmentRepository _departmentRepository;

        public CreateModel(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [BindProperty]
        public Department Department { get; set; } = new();

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

            await _departmentRepository.CreateAsync(Department);
            return RedirectToPage("Index");
        }
    }
}
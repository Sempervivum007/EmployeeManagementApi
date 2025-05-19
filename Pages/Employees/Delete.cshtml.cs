using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Pages.Employees
{
    public class DeleteModel : PageModel
    {
        private readonly IPersonalInfoRepository _personalInfoRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public DeleteModel(IDepartmentRepository departmentRepository,
                            IPersonalInfoRepository personalInfoRepository,
                            IEmployeeRepository employeeRepository,
                            IPositionRepository positionRepository)
        {
            _departmentRepository = departmentRepository;
            _personalInfoRepository = personalInfoRepository;
            _employeeRepository = employeeRepository;
            _positionRepository = positionRepository;
        }

        [BindProperty]
        public PersonalInfo PersonalInfo { get; set; } = new();

        [BindProperty]
        public Employee Employee { get; set; } = new();

        public string DepartmentName { get; set; } = string.Empty;
        public string PositionName { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Employee = await _employeeRepository.GetByIdAsync(id);
            if (Employee == null)
                return NotFound();

            PersonalInfo = await _personalInfoRepository.GetByIdAsync(Employee.PersonalInfoId) ?? new();

            var department = await _departmentRepository.GetByIdAsync(Employee.DepartmentId);
            DepartmentName = department?.Name ?? "";

            var position = await _positionRepository.GetByIdAsync(Employee.PositionId);
            PositionName = position?.Name ?? "";

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Employee.Id == null)
                return NotFound();

            await _employeeRepository.DeleteAsync(Employee.Id);
            await _personalInfoRepository.DeleteAsync(PersonalInfo.Id);

            return RedirectToPage("./Index");
        }
    }
}
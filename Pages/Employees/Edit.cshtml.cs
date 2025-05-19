using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManagement.Pages.Employees
{
    public class EditModel : PageModel
    {
        private readonly IPersonalInfoRepository _personalInfoRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IDepartmentRepository _departmentRepository;

        [BindProperty]
        public PersonalInfo PersonalInfo { get; set; } = new();

        [BindProperty]
        public Employee Employee { get; set; } = new();

        public SelectList Departments { get; set; } = null!;
        public SelectList Positions { get; set; } = null!;

        public EditModel(IDepartmentRepository departmentRepository,
                           IPersonalInfoRepository personalInfoRepository,
                           IEmployeeRepository employeeRepository,
                           IPositionRepository positionRepository)
        {
            _departmentRepository = departmentRepository;
            _personalInfoRepository = personalInfoRepository;
            _employeeRepository = employeeRepository;
            _positionRepository = positionRepository;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return NotFound();

            employee.HireDate = employee.HireDate.ToLocalTime();

            var personalInfo = await _personalInfoRepository.GetByIdAsync(employee.PersonalInfoId);
            if (personalInfo == null)
                return NotFound();

            personalInfo.BirthDate = personalInfo.BirthDate.ToLocalTime();

            Employee = employee;
            PersonalInfo = personalInfo;

            Departments = new SelectList(await _departmentRepository.GetAllAsync(), "Id", "Name");
            Positions = new SelectList(await _positionRepository.GetAllAsync(), "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Departments = new SelectList(await _departmentRepository.GetAllAsync(), "Id", "Name");
                Positions = new SelectList(await _positionRepository.GetAllAsync(), "Id", "Name");
                return Page();
            }

            Employee.PersonalInfoId = PersonalInfo.Id;

            await _personalInfoRepository.UpdateAsync(PersonalInfo);
            await _employeeRepository.UpdateAsync(Employee);
            return RedirectToPage("Index");
        }
    }
}
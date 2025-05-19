using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EmployeeManagement.Pages.Reports
{
    public class SalaryReportModel : PageModel
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPersonalInfoRepository _personalInfoRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IDepartmentRepository _departmentRepository;

        [BindProperty]
        public List<EmployeeViewModel> ViewEmployees { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? DepartmentId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? PositionId { get; set; }

        public SelectList Departments { get; set; } = null!;
        public SelectList Positions { get; set; } = null!;

        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        public DateTime? HireDateFrom { get; set; }

        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        public DateTime? HireDateTo { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal TotalSalary { get; set; } = 0;

        public SalaryReportModel(IDepartmentRepository departmentRepository,
                           IPersonalInfoRepository personalInfoRepository,
                           IEmployeeRepository employeeRepository,
                           IPositionRepository positionRepository)
        {
            _departmentRepository = departmentRepository;
            _personalInfoRepository = personalInfoRepository;
            _employeeRepository = employeeRepository;
            _positionRepository = positionRepository;
        }

        public async Task OnGetAsync()
        {
            var employees = await _employeeRepository.GetFilteredAsync(DepartmentId, PositionId, HireDateFrom, HireDateTo, null);

            Departments = new SelectList(await _departmentRepository.GetAllAsync(), "Id", "Name");
            Positions = new SelectList(await _positionRepository.GetAllAsync(), "Id", "Name");

            foreach (var employee in employees)
            {
                var viewModel = new EmployeeViewModel
                {
                    Id = employee.Id!,
                    Salary = employee.Salary,
                    HireDate = employee.HireDate,
                    PersonalInfo = await _personalInfoRepository.GetByIdAsync(employee.PersonalInfoId),
                    Department = await _departmentRepository.GetByIdAsync(employee.DepartmentId),
                    Position = await _positionRepository.GetByIdAsync(employee.PositionId)
                };
                ViewEmployees.Add(viewModel);
            }

            TotalSalary = ViewEmployees.Sum(r => r.Salary);
        }

        public async Task<IActionResult> OnPostExportAsync()
        {
            await OnGetAsync();

            var sb = new StringBuilder();
            sb.AppendLine("ПІБ\t\tВідділ\t\tПосада\t\tОклад");

            foreach (var row in ViewEmployees)
            {
                sb.AppendLine($"{row.PersonalInfo.FullName}\t{row.Department.Name}\t{row.Position.Name}\t{row.Salary:F2}");
            }
            sb.AppendLine($"\nЗагальна сума окладів: {TotalSalary:F2}");

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/plain", $"salary_report_{DateTime.Now:G}.txt");
        }
    }
}
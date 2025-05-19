using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Pages.Employees
{
    public class IndexModel : PageModel
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
        public string? SearchName { get; set; }

        public IndexModel(IDepartmentRepository departmentRepository,
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
            List<string>? personalInfoIds = null;
            if (!string.IsNullOrWhiteSpace(SearchName))
            {
                personalInfoIds = await _personalInfoRepository.SearchIdsByFullNameAsync(SearchName);
            }

            var employees = await _employeeRepository.GetFilteredAsync(DepartmentId, PositionId, 
                HireDateFrom, HireDateTo, personalInfoIds);

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
        }
    }
}
using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Driver;

namespace EmployeeManagement.Pages.Employees
{
    public class CreateModel : PageModel
    {
        private readonly IPersonalInfoRepository _personalInfoRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMongoClient _client;

        [BindProperty]
        public PersonalInfo PersonalInfo { get; set; } = new();

        [BindProperty]
        public Employee Employee { get; set; } = new();

        public SelectList Departments { get; set; } = null!;
        public SelectList Positions { get; set; } = null!;

        public CreateModel(IDepartmentRepository departmentRepository,
                           IPersonalInfoRepository personalInfoRepository,
                           IEmployeeRepository employeeRepository,
                           IPositionRepository positionRepository,
                           IMongoClient client)
        {
            _departmentRepository = departmentRepository;
            _personalInfoRepository = personalInfoRepository;
            _employeeRepository = employeeRepository;
            _positionRepository = positionRepository;
            _client = client;
        }

        public async Task OnGetAsync()
        {
            Departments = new SelectList(await _departmentRepository.GetAllAsync(), "Id", "Name");
            Positions = new SelectList(await _positionRepository.GetAllAsync(), "Id", "Name");
            Employee = new Employee { HireDate = DateTime.Now };
            PersonalInfo = new PersonalInfo { BirthDate = DateTime.Now };
        }

        //public async Task<IActionResult> OnPostAsyncTransaction()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        await OnGetAsync();
        //        return Page();
        //    }

        //    PersonalInfo.BirthDate = PersonalInfo.BirthDate.ToLocalTime();

        //    using var session = await _client.StartSessionAsync();
        //    session.StartTransaction();

        //    try
        //    {
        //        await _personalInfoRepository.CreateAsync(session, PersonalInfo);

        //        Employee.PersonalInfoId = PersonalInfo.Id;
        //        Employee.HireDate = Employee.HireDate.ToLocalTime();

        //        await _employeeRepository.CreateAsync(session, Employee);

        //        await session.CommitTransactionAsync();
        //    }
        //    catch
        //    {
        //        await session.AbortTransactionAsync();
        //    }

        //    return RedirectToPage("Index");
        //}

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

                await OnGetAsync();
                return Page();
            }

            PersonalInfo.BirthDate = PersonalInfo.BirthDate.ToLocalTime();

            await _personalInfoRepository.CreateAsync(PersonalInfo);
            Employee.PersonalInfoId = PersonalInfo.Id;

            Employee.HireDate = Employee.HireDate.ToLocalTime();

            try
            {
                await _employeeRepository.CreateAsync(Employee);
            }
            catch
            {
                await _personalInfoRepository.DeleteAsync(PersonalInfo.Id);
            }

            return RedirectToPage("Index");
        }
    }
}
using EmployeeManagement.Models;
using EmployeeManagement.Repositories;
using EmployeeManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace EmployeeManagement.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ICompanyInfoRepository _companyRepository;

        public IndexModel(ILogger<IndexModel> logger, ICompanyInfoRepository companyRepository)
        {
            _companyRepository = companyRepository;
            _logger = logger;
        }

        [BindProperty]
        public CompanyInfo Company { get; set; }

        public async Task OnGetAsync()
        {
            Company = await _companyRepository.GetAsync();
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

            await _companyRepository.CreateAsync(Company);
            return RedirectToPage("Index");
        }
    }
}
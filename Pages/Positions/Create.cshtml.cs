using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Pages.Positions
{
    public class CreateModel : PageModel
    {
        private readonly IPositionRepository _positionRepository;

        public CreateModel(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        [BindProperty]
        public Position Position { get; set; } = new();

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

            await _positionRepository.CreateAsync(Position);
            return RedirectToPage("Index");
        }
    }
}
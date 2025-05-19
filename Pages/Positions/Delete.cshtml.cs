using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Pages.Positions
{
    public class DeleteModel : PageModel
    {
        private readonly IPositionRepository _positionRepository;

        public DeleteModel(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        [BindProperty]
        public Position Position { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var position = await _positionRepository.GetByIdAsync(id);
            if (position == null)
            {
                return NotFound();
            }

            Position = position;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Position.Id == null)
            {
                return NotFound();
            }

            await _positionRepository.DeleteAsync(Position.Id);
            return RedirectToPage("./Index");
        }
    }
}
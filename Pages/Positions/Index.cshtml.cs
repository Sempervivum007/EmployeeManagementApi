using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Pages.Positions
{
    public class IndexModel : PageModel
    {
        private readonly IPositionRepository _positionRepository;

        public IndexModel(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public List<Position> Positions { get; set; } = [];

        public async Task OnGetAsync()
        {
            var positions = await _positionRepository.GetAllAsync();
            Positions = positions.ToList();
        }
    }
}
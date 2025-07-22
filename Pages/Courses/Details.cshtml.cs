using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentGradeTrackingSystem.Models;
using StudentGradeTrackingSystem.Data;
using System.Threading.Tasks;

namespace StudentGradeTrackingSystem.Pages.Courses
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public Course Course { get; set; } = new Course();
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Course = await _context.Courses.FindAsync(id);
            if (Course == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

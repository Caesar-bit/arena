using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentGradeTrackingSystem.Models;
using StudentGradeTrackingSystem.Data;
using System.Threading.Tasks;

namespace StudentGradeTrackingSystem.Pages.Courses
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Course? Course { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Course = await _context.Courses.FindAsync(id);
            if (Course == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var course = await _context.Courses.FindAsync(Course?.Id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}

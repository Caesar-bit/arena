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
        public Course Course { get; set; } = new Course();
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            Course = course;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var course = await _context.Courses.FindAsync(Course.Id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}

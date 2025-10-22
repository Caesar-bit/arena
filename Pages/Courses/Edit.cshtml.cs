using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentGradeTrackingSystem.Models;
using StudentGradeTrackingSystem.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StudentGradeTrackingSystem.Pages.Courses
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public EditModel(ApplicationDbContext context)
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
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _context.Attach(Course).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentGradeTrackingSystem.Models;
using StudentGradeTrackingSystem.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace StudentGradeTrackingSystem.Pages.Grades
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Grade Grade { get; set; } = new Grade();
        public SelectList StudentOptions { get; set; } = default!;
        public SelectList CourseOptions { get; set; } = default!;
        public SelectList AssessmentOptions { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Grade = await _context.Grades.Include(g => g.Student).Include(g => g.Course).FirstOrDefaultAsync(g => g.Id == id);
            if (Grade == null)
            {
                return NotFound();
            }
            StudentOptions = new SelectList(_context.Students.ToList(), "Id", "FullName");
            CourseOptions = new SelectList(_context.Courses.ToList(), "Id", "CourseName");
            AssessmentOptions = new SelectList(new[] { "Quiz", "Assignment", "Midterm", "Final" });
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                StudentOptions = new SelectList(_context.Students.ToList(), "Id", "FullName");
                CourseOptions = new SelectList(_context.Courses.ToList(), "Id", "CourseName");
                AssessmentOptions = new SelectList(new[] { "Quiz", "Assignment", "Midterm", "Final" });
                return Page();
            }
            _context.Attach(Grade).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}

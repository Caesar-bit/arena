using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentGradeTrackingSystem.Models;
using StudentGradeTrackingSystem.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace StudentGradeTrackingSystem.Pages.Grades
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Grade Grade { get; set; } = new Grade();
        public SelectList StudentOptions { get; set; } = default!;
        public SelectList CourseOptions { get; set; } = default!;
        public void OnGet()
        {
            StudentOptions = new SelectList(_context.Students.ToList(), "Id", "FullName");
            CourseOptions = new SelectList(_context.Courses.ToList(), "Id", "CourseName");
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                StudentOptions = new SelectList(_context.Students.ToList(), "Id", "FullName");
                CourseOptions = new SelectList(_context.Courses.ToList(), "Id", "CourseName");
                return Page();
            }
            _context.Grades.Add(Grade);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentGradeTrackingSystem.Data;
using StudentGradeTrackingSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace StudentGradeTrackingSystem.Pages.Reports
{
    public class StudentReportModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public StudentReportModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Student? Student { get; set; }
        public IQueryable<Grade> Grades { get; set; } = Enumerable.Empty<Grade>().AsQueryable();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Student = await _context.Students.FindAsync(id);
            if (Student == null)
            {
                return NotFound();
            }

            Grades = _context.Grades
                .Include(g => g.Course)
                .Where(g => g.StudentId == id);

            return Page();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentGradeTrackingSystem.Models;
using StudentGradeTrackingSystem.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StudentGradeTrackingSystem.Pages.Grades
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Grade Grade { get; set; } = new Grade();
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Grade = await _context.Grades.Include(g => g.Student).Include(g => g.Course).FirstOrDefaultAsync(g => g.Id == id);
            if (Grade == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var grade = await _context.Grades.FindAsync(Grade.Id);
            if (grade != null)
            {
                _context.Grades.Remove(grade);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentGradeTrackingSystem.Models;
using StudentGradeTrackingSystem.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentGradeTrackingSystem.Pages.Grades
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public Grade Grade { get; set; } = new Grade();
        public IList<GradeComment> Comments { get; set; } = new List<GradeComment>();

        [BindProperty]
        [Required]
        public string? NewComment { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Grade = await _context.Grades.Include(g => g.Student).Include(g => g.Course).FirstOrDefaultAsync(g => g.Id == id);
            if (Grade == null)
            {
                return NotFound();
            }
            Comments = await _context.GradeComments.AsNoTracking().Where(c => c.GradeId == id).ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAddCommentAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return await OnGetAsync(id);
            }

            var comment = new GradeComment
            {
                GradeId = id,
                Comment = NewComment ?? string.Empty
            };

            _context.GradeComments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id });
        }
    }
}

using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentGradeTrackingSystem.Models;
using StudentGradeTrackingSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentGradeTrackingSystem.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<Student> Students { get; set; } = new List<Student>();
        public async Task OnGetAsync()
        {
            Students = await _context.Students.ToListAsync();
        }
    }
}

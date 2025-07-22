using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentGradeTrackingSystem.Models;
using StudentGradeTrackingSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace StudentGradeTrackingSystem.Pages.Grades
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<Grade> Grades { get; set; } = new List<Grade>();
        public List<(string Student, double Average)> StudentAverages { get; set; } = new();
        public List<(string Course, double Average)> CourseAverages { get; set; } = new();
        public async Task OnGetAsync()
        {
            Grades = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Course)
                .ToListAsync();

            StudentAverages = Grades
                .GroupBy(g => g.Student?.FullName)
                .Where(g => g.Key != null)
                .Select(g => (Student: g.Key!, Average: g.Average(x => x.WeightedScore)))
                .OrderBy(x => x.Student)
                .ToList();

            CourseAverages = Grades
                .GroupBy(g => g.Course?.CourseName)
                .Where(g => g.Key != null)
                .Select(g => (Course: g.Key!, Average: g.Average(x => x.WeightedScore)))
                .OrderBy(x => x.Course)
                .ToList();
        }
    }
}

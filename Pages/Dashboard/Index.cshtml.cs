using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentGradeTrackingSystem.Data;
using StudentGradeTrackingSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentGradeTrackingSystem.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<(string Semester, double Gpa)> GpaTrends { get; set; } = new();
        public List<(string CourseName, double AverageScore)> CourseAverages { get; set; } = new();
        public List<(string StudentName, double AverageScore)> StudentsAtRisk { get; set; } = new();

        public async Task OnGetAsync()
        {
            // For simplicity, assuming Semester is part of CourseName or Grade (not in current model)
            // This is a placeholder for actual semester tracking logic

            var grades = await _context.Grades.Include(g => g.Student).Include(g => g.Course).ToListAsync();

            // Calculate course averages
            CourseAverages = grades
                .GroupBy(g => g.Course?.CourseName)
                .Where(g => g.Key != null)
                .Select(g => (CourseName: g.Key!, AverageScore: g.Average(x => x.WeightedScore)))
                .OrderBy(x => x.CourseName)
                .ToList();

            // Calculate students at risk (average score below 50)
            StudentsAtRisk = grades
                .GroupBy(g => g.Student?.FullName)
                .Where(g => g.Key != null)
                .Select(g => (StudentName: g.Key!, AverageScore: g.Average(x => x.WeightedScore)))
                .Where(x => x.AverageScore < 50)
                .OrderBy(x => x.StudentName)
                .ToList();

            // GPA trends placeholder - no semester data, so just overall GPA
            var gpaService = new Services.GpaService();
            var studentGrades = grades.GroupBy(g => g.StudentId);
            GpaTrends = studentGrades.Select(g =>
            {
                var gpa = gpaService.CalculateGpa(g);
                return ("Overall", gpa);
            }).ToList();
        }
    }
}

using StudentGradeTrackingSystem.Models;

namespace StudentGradeTrackingSystem.Services
{
    public class GpaService
    {
        public double CalculateGpa(IEnumerable<Grade> grades)
        {
            if (!grades.Any()) return 0;
            // Simple GPA calculation: map score 0-100 to 0.0-4.0 scale
            var points = grades.Average(g => g.Score) / 25.0;
            return Math.Round(points, 2);
        }
    }
}

using StudentGradeTrackingSystem.Models;

namespace StudentGradeTrackingSystem.Services
{
    public class GpaService
    {
        public double CalculateGpa(IEnumerable<Grade> grades)
        {
            if (!grades.Any()) return 0;
            // GPA based on weighted scores
            var avg = grades.Average(g => g.WeightedScore);
            var points = avg / 25.0;
            return Math.Round(points, 2);
        }

        public string ToLetterGrade(double score)
        {
            if (score >= 90) return "A";
            if (score >= 80) return "B";
            if (score >= 70) return "C";
            if (score >= 60) return "D";
            return "F";
        }
    }
}

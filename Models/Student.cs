namespace StudentGradeTrackingSystem.Models
{
    public class Student
    {
        public int Id { get; set; }

        // Unique identifier used by the school
        public string StudentNumber { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        // E.g. "Grade 9", "Year 3"
        public string GradeLevel { get; set; } = string.Empty;

        // Simple contact field for demonstration purposes
        public string ContactEmail { get; set; } = string.Empty;

        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}

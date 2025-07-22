namespace StudentGradeTrackingSystem.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; } = string.Empty;

        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}

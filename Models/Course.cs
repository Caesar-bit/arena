namespace StudentGradeTrackingSystem.Models
{
    public class Course
    {
        public int Id { get; set; }

        public string CourseName { get; set; } = string.Empty;

        // Which grade level this course is intended for
        public string GradeLevel { get; set; } = string.Empty;

        // Weighting for midterm and final exam scores (should add up to 100)
        public int MidtermWeight { get; set; } = 40;
        public int FinalExamWeight { get; set; } = 60;

        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}

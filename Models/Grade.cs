namespace StudentGradeTrackingSystem.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }

        // Assessment type e.g. "Quiz", "Midterm", "Final"
        public string AssessmentType { get; set; } = string.Empty;

        // Percentage weight of this assessment towards the course grade
        public int Weight { get; set; } = 100;

        // Numeric score 0-100
        public int Score { get; set; }

        public Student? Student { get; set; }
        public Course? Course { get; set; }

        // Calculate the contribution to the course grade
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public double WeightedScore => Score * (Weight / 100.0);
    }
}

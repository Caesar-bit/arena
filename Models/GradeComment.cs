namespace StudentGradeTrackingSystem.Models
{
    public class GradeComment
    {
        public int Id { get; set; }
        public int GradeId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Grade? Grade { get; set; }
    }
}

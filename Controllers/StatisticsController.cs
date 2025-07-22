using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentGradeTrackingSystem.Data;

namespace StudentGradeTrackingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("student-averages")]
        public async Task<IActionResult> GetStudentAverages()
        {
            var averages = await _context.Grades
                .Include(g => g.Student)
                .GroupBy(g => g.Student!.FullName)
                .Select(g => new { Student = g.Key, Average = g.Average(x => x.Score) })
                .ToListAsync();
            return Ok(averages);
        }

        [HttpGet("course-averages")]
        public async Task<IActionResult> GetCourseAverages()
        {
            var averages = await _context.Grades
                .Include(g => g.Course)
                .GroupBy(g => g.Course!.CourseName)
                .Select(g => new { Course = g.Key, Average = g.Average(x => x.Score) })
                .ToListAsync();
            return Ok(averages);
        }
    }
}

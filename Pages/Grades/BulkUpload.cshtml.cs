using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using StudentGradeTrackingSystem.Data;
using StudentGradeTrackingSystem.Models;
using ClosedXML.Excel;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace StudentGradeTrackingSystem.Pages.Grades
{
    public class BulkUploadModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public BulkUploadModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public IFormFile? UploadFile { get; set; }

        public string Message { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (UploadFile == null || UploadFile.Length == 0)
            {
                Message = "Please select a valid Excel file.";
                return Page();
            }

            using var stream = new MemoryStream();
            await UploadFile.CopyToAsync(stream);
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.First();

            var gradesToAdd = new List<Grade>();

            // Assuming columns: StudentId, CourseId, Score starting from row 2
            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                if (int.TryParse(row.Cell(1).GetString(), out int studentId) &&
                    int.TryParse(row.Cell(2).GetString(), out int courseId) &&
                    int.TryParse(row.Cell(3).GetString(), out int score))
                {
                    if (score >= 0 && score <= 100)
                    {
                        gradesToAdd.Add(new Grade
                        {
                            StudentId = studentId,
                            CourseId = courseId,
                            Score = score
                        });
                    }
                }
            }

            if (gradesToAdd.Count > 0)
            {
                _context.Grades.AddRange(gradesToAdd);
                await _context.SaveChangesAsync();
                Message = $"Successfully uploaded {gradesToAdd.Count} grades.";
            }
            else
            {
                Message = "No valid grades found in the file.";
            }

            return Page();
        }
    }
}

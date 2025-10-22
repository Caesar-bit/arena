using Microsoft.EntityFrameworkCore;
using StudentGradeTrackingSystem.Data;
using StudentGradeTrackingSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using StudentGradeTrackingSystem.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();

// Add Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add DbContext using SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add SignalR services
builder.Services.AddSignalR();

var app = builder.Build();

// Seed sample data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // Recreate database if schema is missing tables
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
    if (!context.Users.Any())
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        // Create roles
        var roles = new[] { "Admin", "Lecturer", "Student" };
        foreach (var role in roles)
        {
            if (!roleManager.RoleExistsAsync(role).Result)
                roleManager.CreateAsync(new IdentityRole(role)).Wait();
        }
        // Create default admin user
        var adminUser = new ApplicationUser { UserName = "admin@school.com", Email = "admin@school.com", EmailConfirmed = true };
        userManager.CreateAsync(adminUser, "Admin123!").Wait();
        userManager.AddToRoleAsync(adminUser, "Admin").Wait();
    }
    if (!context.Students.Any() && !context.Courses.Any() && !context.Grades.Any())
    {
        var students = new[]
        {
            new Student { StudentNumber = "S001", FullName = "Alice Johnson", GradeLevel = "Grade 9", ContactEmail = "alice@example.com" },
            new Student { StudentNumber = "S002", FullName = "Bob Smith", GradeLevel = "Grade 9", ContactEmail = "bob@example.com" },
            new Student { StudentNumber = "S003", FullName = "Carol Lee", GradeLevel = "Grade 10", ContactEmail = "carol@example.com" }
        };
        var courses = new[]
        {
            new Course { CourseName = "Mathematics", GradeLevel = "Grade 9", MidtermWeight = 40, FinalExamWeight = 60 },
            new Course { CourseName = "History", GradeLevel = "Grade 9", MidtermWeight = 30, FinalExamWeight = 70 },
            new Course { CourseName = "Science", GradeLevel = "Grade 10", MidtermWeight = 50, FinalExamWeight = 50 }
        };
        context.Students.AddRange(students);
        context.Courses.AddRange(courses);
        context.SaveChanges();
        var grades = new[]
        {
            new Grade { StudentId = students[0].Id, CourseId = courses[0].Id, AssessmentType = "Midterm", Weight = courses[0].MidtermWeight, Score = 95 },
            new Grade { StudentId = students[1].Id, CourseId = courses[1].Id, AssessmentType = "Final", Weight = courses[1].FinalExamWeight, Score = 88 },
            new Grade { StudentId = students[2].Id, CourseId = courses[2].Id, AssessmentType = "Midterm", Weight = courses[2].MidtermWeight, Score = 76 },
            new Grade { StudentId = students[0].Id, CourseId = courses[1].Id, AssessmentType = "Quiz", Weight = 10, Score = 82 }
        };
        context.Grades.AddRange(grades);
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

// Map SignalR hubs
app.MapHub<GradeHub>("/gradeHub");

app.Run();

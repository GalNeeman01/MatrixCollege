using Microsoft.EntityFrameworkCore;

namespace Matrix;

public class MatrixCollegeContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Course> Courses { get; set; }

    public DbSet<Lesson> Lessons { get; set; }

    public DbSet<Enrollment> Enrollments { get; set; }

    public DbSet<Progress> Progresses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(AppConfig.ConnectionString);

        base.OnConfiguring(optionsBuilder);
    }
}

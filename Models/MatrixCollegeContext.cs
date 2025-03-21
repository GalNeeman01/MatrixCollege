using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Matrix;

public class MatrixCollegeContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Course> Courses { get; set; }

    public DbSet<Lesson> Lessons { get; set; }

    public DbSet<Enrollment> Enrollments { get; set; }

    public DbSet<Progress> Progresses { get; set; }

    private readonly DatabaseSettings _dbSettings;

    public MatrixCollegeContext(IOptions<DatabaseSettings> dbSettings)
    {
        _dbSettings = dbSettings.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_dbSettings.DefaultConnection);

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed default roles
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 3, Name = "Professor" },
            new Role { Id = 2, Name = "Student" }
        );

        // Seed default roles
        modelBuilder.Entity<User>().HasData( // Must add hardcoded guid's due to EF errors popping up otherwise
            new User { Id = Guid.Parse("0759b274-5fc0-4119-8bd5-36ef8ff7f291"), Name = "admin", Email = "admin@gmail.com", Password = Encryptor.GetHashed("adminadmin"), RoleId = 1 },
            new User { Id = Guid.Parse("6ddc0899-88d6-4a80-929e-b910683656a2"), Name = "student", Email = "student@gmail.com", Password = Encryptor.GetHashed("studentstudent"), RoleId = 2 },
            new User { Id = Guid.Parse("c485947c-2ad4-4818-ab26-cb4e45e33136"), Name = "professor", Email = "professor@gmail.com", Password = Encryptor.GetHashed("professor"), RoleId = 3 }
        );
    }
}

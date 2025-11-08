using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduPlatform.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Quiz> Quizzes => Set<Quiz>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<AnswerOption> AnswerOptions => Set<AnswerOption>();
        public DbSet<Attempt> Attempts => Set<Attempt>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Course>()
                .HasMany(c => c.Quizzes)
                .WithOne(q => q.Course!)
                .HasForeignKey(q => q.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity(typeof(Quiz));

            builder.Entity<Quiz>()
                .HasMany(q => q.Questions)
                .WithOne(qn => qn.Quiz!)
                .HasForeignKey(qn => qn.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Question>()
                .HasMany(qn => qn.Options)
                .WithOne(o => o.Question!)
                .HasForeignKey(o => o.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Quiz>()
                .HasMany(q => q.Attempts)
                .WithOne(a => a.Quiz!)
                .HasForeignKey(a => a.QuizId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

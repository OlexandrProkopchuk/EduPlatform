using EduPlatform.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using System.Linq;

namespace EduPlatform.IntegrationTests.Infrastructure
{
    public class CustomWebAppFactory : WebApplicationFactory<Program>
    {
        private SqliteConnection _connection;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                // 1️⃣  Фейкова автентифікація
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                    options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    TestAuthHandler.SchemeName, options => { });

                // 2️⃣  SQLite In-Memory (один конект на всю фабрику)
                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();

                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(_connection));

                // 3️⃣  ВИМКНУТИ AntiForgery (саме сюди вставляєш цей рядок)
                services.PostConfigure<MvcOptions>(o =>
                {
                    o.Filters.Add(new IgnoreAntiforgeryTokenAttribute());
                });

                // 4️⃣  Seed тестових даних
                using var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                db.Database.EnsureCreated();

                if (!db.Quizzes.Any())
                {
                    var quiz = new Quiz
                    {
                        Id = 1,
                        Title = "Тест для інтеграції",
                        TimeLimitMinutes = 10,
                        Course = new Course { Title = "Курс інтеграційних тестів" },
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                Id = 1,
                                Text = "2 + 2",
                                AnswerOptions = new List<AnswerOption>
                                {
                                    new AnswerOption { Id = 1, Text = "3", IsCorrect = false },
                                    new AnswerOption { Id = 2, Text = "4", IsCorrect = true },
                                    new AnswerOption { Id = 3, Text = "5", IsCorrect = false }
                                }
                            }
                        }
                    };
                    db.Quizzes.Add(quiz);
                    db.SaveChanges();
                }
            });
        }
    }
}

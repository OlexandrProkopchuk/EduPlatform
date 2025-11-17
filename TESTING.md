## Testing Project Structure
EduPlatform/
EduPlatform.Tests/                 ← Unit tests
EduPlatform.IntegrationTests/      ← Integration tests
│
├── Infrastructure/
│   ├── CustomWebAppFactory.cs     ← Test server factory
│   ├── TestAuthHandler.cs         ← Fake authentication
│
├── Pages/
│   ├── QuizzesPageTests.cs        ← Integration tests for quiz flow
│
└── ...

## Unit Testing

✔ Score calculation (QuizService)
Tests verify:
All answers correct
Some answers wrong
No answers provided
Example Unit Test (xUnit)

[Fact]
public void Score_CalculatesCorrectly_WhenAllAnswersCorrect()
{
    var service = new QuizService();
    var answers = new Dictionary<int, int[]> {
        { 1, new [] {2} }
    };

    var result = service.ScoreQuiz(1, answers);

    Assert.Equal(1, result.CorrectAnswers);
}

dotnet test

## Integration Testing

Integration tests simulate real user behavior:

✔ Accessing /Quizzes
✔ Starting a quiz
✔ Submitting answers
✔ Redirecting to results
✔ Rendering UI correctly

Integration tests use:

CustomWebAppFactory
Replaces SQL Server → SQLite in-memory
Seeds fake test data
Injects a fake authenticated user
Disables AntiForgery validation

Example Test (start quiz)

[Fact]
public async Task TakeQuiz_Start_ShowsQuestions()
{
    var client = _factory.CreateClient();

    var resp = await client.GetAsync("/Quiz/Start/1");
    resp.EnsureSuccessStatusCode();

    var html = await resp.Content.ReadAsStringAsync();
    Assert.Contains("2 + 2", html);
}


## Custom Test Server Setup

📌 Fake authentication

TestAuthHandler logs in a fake user automatically.

📌 Disable AntiForgery

Required for POST tests:

services.PostConfigure<MvcOptions>(o =>
{
    o.Filters.Add(new IgnoreAntiforgeryTokenAttribute());
});


📌 SQLite In-Memory

Ensures fast DB tests:

services.AddDbContext<AppDbContext>(
    options => options.UseSqlite(_connection)
);


📌 Test Data Seed

Inserted in CustomWebAppFactory:

db.Quizzes.Add(new Quiz {
    Id = 1,
    Title = "Test Quiz",
    Questions = new List<Question> {
        new Question {
            Id = 1,
            Text = "2 + 2",
            AnswerOptions = new List<AnswerOption> {
                new AnswerOption { Id = 1, Text="3", IsCorrect=false },
                new AnswerOption { Id = 2, Text="4", IsCorrect=true }
            }
        }
    }
});
db.SaveChanges();


## Running Tests

🔹 Using Visual Studio

Open Test Explorer

Click Run All

See results with green/red indicators

🔹 Using CLI (recommended)
dotnet test

🔹 Run only one project
dotnet test EduPlatform.IntegrationTests
dotnet test EduPlatform.Tests

🔹 Run a single test file
dotnet test --filter "FullyQualifiedName~QuizzesPageTests"

## Debugging Integration Tests

Open test file
Click Debug Test near [Fact]
Set breakpoints in:
Controllers
Views
Test setup
The app will run through Kestrel in test mode

Tip:
var html = await resp.Content.ReadAsStringAsync();
Console.WriteLine(html);
This prints the page rendered by the app — invaluable for diagnosing test failures.

## Common Issues & Fixes

❌ 400 BadRequest on Submit

Fix: Disable AntiForgery in tests.

❌ Test cannot find “2 + 2”

Fix:

Seed test data correctly

Use WebUtility.HtmlDecode(html)

❌ Database not created

Fix: Ensure:

db.Database.EnsureCreated();

❌ Multiple entry points

Remove custom Main methods in test projects.

## How to Add New Tests

Add a new Integration Test:

Go to:

EduPlatform.IntegrationTests/Pages/


Create a new file:

NewFeatureTests.cs


Add:

public class NewFeatureTests : IClassFixture<CustomWebAppFactory>
{
    private readonly HttpClient _client;

    public NewFeatureTests(CustomWebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Page_Loads_Successfully()
    {
        var resp = await _client.GetAsync("/SomePage");
        resp.EnsureSuccessStatusCode();
    }
}

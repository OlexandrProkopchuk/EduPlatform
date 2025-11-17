using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduPlatform.Data;
using EduPlatform.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using EduPlatform.Services;

namespace EduPlatformSolution.Controllers 
{
    [Authorize] 
    public class TakeQuizController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IQuizService _quizService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TakeQuizController(AppDbContext db, IQuizService quizService, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _quizService = quizService;
            _userManager = userManager;
        }

        // GET /Quiz/Start/{id}
        [HttpGet("/Quiz/Start/{id}")]
        public async Task<IActionResult> Start(int id)
        {
            var quiz = await _db.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.AnswerOptions)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null) return NotFound();

            var vm = new TakeQuizViewModel
            {
                QuizId = quiz.Id,
                Title = quiz.Title,
                Questions = quiz.Questions
                    .OrderBy(q => q.Id)
                    .Select(q => new QuestionVm
                    {
                        Id = q.Id,
                        Text = q.Text,
                        Options = q.AnswerOptions
                            .OrderBy(o => o.Id)
                            .Select(o => new AnswerOptionVm { Id = o.Id, Text = o.Text })
                            .ToList()
                    })
                    .ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(SubmitQuizViewModel model)
        {
            // НІЯКОГО ValidateAntiForgeryToken і НІЯКОГО BadRequest по ModelState

            var userId = _userManager.GetUserId(User) ?? "anonymous";

            // Якщо з якоїсь причини Answers не прийшли — створимо порожній словник
            var answers = model.Answers ?? new Dictionary<int, int>();

            var attempt = await _quizService.GradeAsync(model.QuizId, userId, answers);

            // Переходимо на сторінку результату
            return RedirectToAction(nameof(Result), new { id = attempt.Id });
        }

        public async Task<IActionResult> Result(int id)
        {
            var attempt = await _db.Attempts.Include(a => a.Quiz).FirstOrDefaultAsync(a => a.Id == id);
            if (attempt == null) return NotFound();

            ViewBag.Percent = attempt.MaxScore > 0
                ? (int)Math.Round(100.0 * attempt.Score / attempt.MaxScore)
                : 0;

            return View(attempt);
        }
    }
}

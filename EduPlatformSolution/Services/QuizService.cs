using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduPlatform.Data;
using Microsoft.EntityFrameworkCore;

namespace EduPlatform.Services
{
    public class QuizService : IQuizService
    {
        private readonly AppDbContext _context;

        public QuizService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Attempt> GradeAsync(int quizId, string userId, Dictionary<int, int> answers)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.AnswerOptions)
                .FirstOrDefaultAsync(q => q.Id == quizId);

            if (quiz == null)
                throw new Exception("Quiz not found");

            int max = quiz.Questions.Count;
            int score = 0;

            foreach (var q in quiz.Questions)
            {
                if (answers.TryGetValue(q.Id, out var selectedOptionId))
                {
                    var correctOption = q.AnswerOptions.FirstOrDefault(a => a.IsCorrect);
                    if (correctOption != null && correctOption.Id == selectedOptionId)
                        score++;
                }
            }

            var attempt = new Attempt
            {
                QuizId = quizId,
                UserId = userId,
                Score = score,
                MaxScore = max,
                FinishedAt = DateTime.UtcNow
            };

            _context.Attempts.Add(attempt);
            await _context.SaveChangesAsync();

            return attempt;
        }
    }
}

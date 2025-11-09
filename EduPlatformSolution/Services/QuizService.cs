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

        // Одновибір: answers[questionId] = selectedOptionId    
        public async Task<Attempt> GradeAsync(int quizId, string userId, Dictionary<int, int[]> answers)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(qn => qn.AnswerOptions)
                .FirstOrDefaultAsync(q => q.Id == quizId);

            if (quiz == null)
                throw new InvalidOperationException($"Quiz {quizId} not found");

            int max = quiz.Questions.Count;
            int score = 0;

            foreach (var q in quiz.Questions)
            {
                if (answers.TryGetValue(q.Id, out var selectedIdsArray))
                {
                    var selected = selectedIdsArray?.Distinct().ToHashSet() ?? new HashSet<int>();

                    var correct = q.AnswerOptions
                        .Where(a => a.IsCorrect)
                        .Select(a => a.Id)
                        .ToHashSet();

                    if (selected.SetEquals(correct))
                        score++;
                }
            }

            var attempt = new Attempt
            {
                QuizId = quizId,
                UserId = userId,
                Score = score,
                // переконайся, що у моделі Attempt є це поле
                MaxScore = max,
                FinishedAt = DateTime.UtcNow
            };

            _context.Attempts.Add(attempt);
            await _context.SaveChangesAsync();

            return attempt;
        }
    }
}

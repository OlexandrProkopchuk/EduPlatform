using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EduPlatform.Data;

namespace EduPlatformSolution.Controllers
{
    public class AnswerOptionsController : Controller
    {
        private readonly AppDbContext _context;

        public AnswerOptionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AnswerOptions
        public async Task<IActionResult> Index(int? questionId)
        {
            IQueryable<AnswerOption> query =
                _context.AnswerOptions
                        .Include(a => a.Question);

            if (questionId.HasValue)
                query = query.Where(a => a.QuestionId == questionId.Value);

            ViewBag.Question = questionId.HasValue
                ? await _context.Questions
                                .Include(q => q.Quiz)
                                .FirstOrDefaultAsync(q => q.Id == questionId.Value)
                : null;

            var list = await query.OrderBy(a => a.Id).ToListAsync();
            return View(list);
        }


        // GET: AnswerOptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answerOption = await _context.AnswerOptions
                .Include(a => a.Question)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answerOption == null)
            {
                return NotFound();
            }

            return View(answerOption);
        }

        // GET: AnswerOptions/Create
        public IActionResult Create(int? questionId)
        {
            // красиві підписи: НазваТесту / ТекстПитання
            ViewData["QuestionId"] = new SelectList(
                _context.Questions.Include(q => q.Quiz)
                    .Select(q => new { q.Id, Title = q.Quiz.Title + " / " + q.Text }),
                "Id", "Title", questionId);

            return View(new AnswerOption { QuestionId = questionId ?? 0 });
        }

        // POST: AnswerOptions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,QuestionId,Text,IsCorrect")] AnswerOption answerOption)
        {
            if (ModelState.IsValid)
            {
                if (answerOption.IsCorrect)
                {
                    var others = await _context.AnswerOptions
                        .Where(a => a.QuestionId == answerOption.QuestionId && a.IsCorrect)
                        .ToListAsync();
                    foreach (var o in others) o.IsCorrect = false;
                }

                _context.Add(answerOption);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { questionId = answerOption.QuestionId });
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Text", answerOption.QuestionId);
            return View(answerOption);
        }

        // GET: AnswerOptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answerOption = await _context.AnswerOptions.FindAsync(id);
            if (answerOption == null)
            {
                return NotFound();
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Text", answerOption.QuestionId);
            return View(answerOption);
        }

        // POST: AnswerOptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,QuestionId,Text,IsCorrect")] AnswerOption answerOption)
        {
            if (id != answerOption.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (answerOption.IsCorrect)
                    {
                        var others = await _context.AnswerOptions
                            .Where(a => a.QuestionId == answerOption.QuestionId && a.Id != answerOption.Id && a.IsCorrect)
                            .ToListAsync();
                        foreach (var o in others) o.IsCorrect = false;
                    }

                    _context.Update(answerOption);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnswerOptionExists(answerOption.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index), new { questionId = answerOption.QuestionId });
            }

            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Text", answerOption.QuestionId);
            return View(answerOption);
        }

        // GET: AnswerOptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answerOption = await _context.AnswerOptions
                .Include(a => a.Question)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answerOption == null)
            {
                return NotFound();
            }

            return View(answerOption);
        }

        // POST: AnswerOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var answerOption = await _context.AnswerOptions.FindAsync(id);
            int? qid = answerOption?.QuestionId;

            if (answerOption != null)
                _context.AnswerOptions.Remove(answerOption);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { questionId = qid });
        }

        private bool AnswerOptionExists(int id)
        {
            return _context.AnswerOptions.Any(e => e.Id == id);
        }
    }
}

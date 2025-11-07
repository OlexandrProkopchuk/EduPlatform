using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Data
{
    public class Question
    {
        public int Id { get; set; }

        [Required]
        public int QuizId { get; set; }
        public Quiz? Quiz { get; set; }

        [Required, StringLength(2000)]
        public string Text { get; set; } = string.Empty;

        // "single" | "multiple"
        [Required, StringLength(20)]
        public string Type { get; set; } = "single";

        public ICollection<AnswerOption> Options { get; set; } = new List<AnswerOption>();
    }
}

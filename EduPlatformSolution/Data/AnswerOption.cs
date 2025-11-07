using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Data
{
    public class AnswerOption
    {
        public int Id { get; set; }

        [Required]
        public int QuestionId { get; set; }
        public Question? Question { get; set; }

        [Required, StringLength(1000)]
        public string Text { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
    }
}

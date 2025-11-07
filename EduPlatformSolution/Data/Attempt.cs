using System;
using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Data
{
    public class Attempt
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty; // посилання на ApplicationUser.Id

        [Required]
        public int QuizId { get; set; }
        public Quiz? Quiz { get; set; }

        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? FinishedAt { get; set; }

        [Range(0, int.MaxValue)]
        public int Score { get; set; }
    }
}
 
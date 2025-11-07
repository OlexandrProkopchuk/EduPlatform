using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Data
{
    public class Quiz
    {
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }
        public Course? Course { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Range(0, 600)]
        public int TimeLimitMinutes { get; set; } = 0;

        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<Attempt> Attempts { get; set; } = new List<Attempt>();
    }
}

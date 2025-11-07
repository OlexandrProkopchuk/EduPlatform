using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Data
{
    public class Course
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? Description { get; set; }

        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    }
}
    
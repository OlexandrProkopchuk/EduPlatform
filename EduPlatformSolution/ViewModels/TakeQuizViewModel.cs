namespace EduPlatform.Data.ViewModels
{
    public class TakeQuizViewModel
    {
        public int QuizId { get; set; }
        public string Title { get; set; } = string.Empty;
        public IList<QuestionVm> Questions { get; set; } = new List<QuestionVm>();
    }

    public class QuestionVm
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public IList<AnswerOptionVm> Options { get; set; } = new List<AnswerOptionVm>();
    }

    public class AnswerOptionVm
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}

namespace EduPlatform.ViewModels
{
    public class QuizResultViewModel
    {
        public string QuizTitle { get; set; } = string.Empty;
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public List<QuestionResult> QuestionResults { get; set; } = new();
    }

    public class QuestionResult
    {
        public string QuestionText { get; set; } = string.Empty;
        public string UserAnswer { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
        public bool IsCorrect => UserAnswer == CorrectAnswer;
    }
}

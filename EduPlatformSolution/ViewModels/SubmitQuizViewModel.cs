namespace EduPlatform.Data.ViewModels
{
    // Біндинг словника: ключ = Id питання, значення = масив обраних варіантів
    public class SubmitQuizViewModel
    {
        public int QuizId { get; set; }

        // name="Answers[<questionId>]" value="<optionId>"
        public Dictionary<int, int[]> Answers { get; set; } = new();
    }
}

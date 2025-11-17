namespace EduPlatform.Data.ViewModels
{
    // Біндинг словника: ключ = Id питання, значення = масив обраних варіантів
    public class SubmitQuizViewModel
    {
        public int QuizId { get; set; }

        // ключ = Id питання, значення = Id обраного варіанту
        public Dictionary<int, int> Answers { get; set; } = new();
    }
}

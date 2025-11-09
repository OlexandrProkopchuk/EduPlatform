using System.Collections.Generic;
using System.Threading.Tasks;
using EduPlatform.Data;

namespace EduPlatform.Services;

public interface IQuizService
{
    Task<Attempt> GradeAsync(int quizId, string userId, Dictionary<int, int[]> answers);
}

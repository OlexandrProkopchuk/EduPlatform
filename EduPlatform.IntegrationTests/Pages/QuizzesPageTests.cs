using EduPlatform.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace EduPlatform.IntegrationTests.Infrastructure
{
    public class QuizzesPageTests : IClassFixture<CustomWebAppFactory>
    {
        private readonly CustomWebAppFactory _factory;

        public QuizzesPageTests(CustomWebAppFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Quizzes_Index_ReturnsOk_And_TitlePresent()
        {
            var client = _factory.CreateClient();
            var resp = await client.GetAsync("/Quizzes");
            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

            var html = await resp.Content.ReadAsStringAsync();
            Assert.Contains("Список тестів", html); // заголовок з твого View
        }

        [Fact]
        public async Task TakeQuiz_Start_ShowsQuestions()
        {
            // Arrange
            var client = _factory.CreateClient();

            // витягуємо перший quiz id зі сторінки списку (або хардкодь 1, якщо знаєш що саме 1)
            // простіше: під час seed залишити quiz.Id = 1 (прописати вручну перед SaveChanges)

            var respList = await client.GetAsync("/Quizzes"); // якщо є такий маршрут
            respList.EnsureSuccessStatusCode();

            // Act
            var resp = await client.GetAsync("/Quiz/Start/1"); // якщо в seed виставиш Id=1
            resp.EnsureSuccessStatusCode();
            var html = await resp.Content.ReadAsStringAsync();

            // ДЕКОДУЄМО HTML-ентіті: &#x2B; -> '+', &amp; -> '&' і т.д.
            var decoded = WebUtility.HtmlDecode(html);

            Assert.Contains("2 + 2", decoded);
        }


        [Fact]
        public async Task TakeQuiz_Submit_RedirectsToResult()
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            // Переконаємось, що старт сторінка існує
            var start = await client.GetAsync("/Quiz/Start/1");
            start.EnsureSuccessStatusCode();

            var form = new Dictionary<string, string>
            {
                ["QuizId"] = "1",
                ["Answers[1]"] = "2"   // 1 — Question.Id, 2 — AnswerOption.Id (правильний)
            };

            var content = new FormUrlEncodedContent(form);
            var resp = await client.PostAsync("/TakeQuiz/Submit", content);

            if (resp.StatusCode == HttpStatusCode.BadRequest)
            {
                var body = await resp.Content.ReadAsStringAsync();
                // ти вже бачив, що body порожній — після спрощення Submit цього більше не буде
                throw new Xunit.Sdk.XunitException("400 BadRequest:\n" + body);
            }

            Assert.Equal(HttpStatusCode.Found, resp.StatusCode);
            var loc = resp.Headers.Location?.ToString() ?? "";
            Assert.Contains("/TakeQuiz/Result", loc);
        }


    }
}

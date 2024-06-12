using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using ticket_office_WEB.Models;

namespace ticket_office_WEB.Controllers
{
    public class OpenAIController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly TMDBController _tmdbController;
        public string recommendationsString;
        public List<string> recommendationsList;
        public List<MovieModel> movieResults;

        public OpenAIController(TMDBController tmdbController)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.openai.com/")
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sk-proj-iiTrCLsl7bcZHoBSdI8MT3BlbkFJuu6JTgWbz0jBBsgcUBIk");
            _tmdbController = tmdbController;
            movieResults = new List<MovieModel>(); 
        }

        public IActionResult Index()
        {
            return View("~/Views/Home/Index.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> GetRecommendations(string userQuery)
        {
            ViewBag.UserQuery = userQuery; 
            var requestData = new
            {
                model = "gpt-4",
                messages = new[]
                {
                    new { role = "user", content = $"Suggest movie based on the following description: {userQuery}. You can use TMDB data base. Only movies that actually exist. " +
                    $"Just the name of the movie. Give the names of 10 movies, series, or cartoons according to the template Title|Title|Title" }
                },
                temperature = 0.1,
                max_tokens = 200
            };

            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("v1/chat/completions", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
                recommendationsString = responseObject.choices[0].message.content;
                recommendationsList = recommendationsString.Split('|').ToList();

                foreach (var title in recommendationsList)
                {
                    var movieResponse = await _tmdbController.SearchAll(title, 1);
                    if (movieResponse is ViewResult viewResult && viewResult.Model is MovieResponseModel movieData)
                    {
                        var movie = movieData.Results.FirstOrDefault();
                        if (movie != null)
                        {
                            movieResults.Add(movie);
                        }
                    }
                }

                ViewBag.MovieResults = movieResults;
                ViewBag.MovieGenres = _tmdbController._movieGenres;
                ViewBag.TvGenres = _tmdbController._tvGenres;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ViewBag.Recommendations = $"Ошибка API: {response.StatusCode} - {errorContent}";
            }

            return View("~/Views/Home/Index.cshtml");
        }
    }
}

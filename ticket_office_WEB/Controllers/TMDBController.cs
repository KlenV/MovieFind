using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ticket_office_WEB.Models;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ticket_office_WEB.Controllers
{
    public class TMDBController : Controller
    {
        private readonly HttpClient _client;
        private readonly ILogger<TMDBController> _logger;
        public Dictionary<int, string> _movieGenres { get; private set; }
        public Dictionary<int, string> _tvGenres { get; private set; }

        public TMDBController(ILogger<TMDBController> logger)
        {
            _client = new HttpClient();
            _client.BaseAddress = new System.Uri("https://api.themoviedb.org/3/");
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJiYWZjYWVlNzA0ZTYyZGI1N2NkYmY0YjgwNzk0MzhmMyIsInN1YiI6IjY2NDBlMjI1NDcwNDJhYTZkNmYwYzBhZiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.N0FYUHBrmpp447uMM3mF35ItuffisJzEM38tcck0CVk");
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _movieGenres = new Dictionary<int, string>();
            _tvGenres = new Dictionary<int, string>();
            InitializeGenres().Wait();
        }

        private async Task InitializeGenres()
        {
            await GetMovieGenres();
            await GetTvGenres();
        }

        public async Task<IActionResult> TMDB(int page = 1)
        {
            var response = await _client.GetAsync($"movie/top_rated?language=uk-UK&page={page}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var movieResponse = JsonConvert.DeserializeObject<MovieResponseModel>(content);
                ViewBag.MovieGenres = _movieGenres;

                foreach (var result in movieResponse.Results)
                {
                    if (string.IsNullOrEmpty(result.MediaType))
                    {
                        result.MediaType = "movie";
                    }
                }

                movieResponse.Page = page;               
                return View(movieResponse);
            }
            return View("Error");
        }

        public async Task<IActionResult> TMDBPopular(int page = 1)
        {
            var response = await _client.GetAsync($"https://api.themoviedb.org/3/movie/popular?language=uk-UK&page={page}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var movieResponse = JsonConvert.DeserializeObject<MovieResponseModel>(content);
                ViewBag.MovieGenres = _movieGenres;

                foreach (var result in movieResponse.Results)
                {
                    if (string.IsNullOrEmpty(result.MediaType))
                    {
                        result.MediaType = "movie"; 
                    }
                }

                movieResponse.Page = page;               
                return View(movieResponse);
            }
            return View("Error");
        }

        // search method -----------------------------------------------------------

        public async Task<IActionResult> SearchAll(string query, int page = 1)
        {
            ViewBag.SearchQuery = query;
            ViewBag.MovieGenres = _movieGenres;
            ViewBag.TvGenres = _tvGenres;
            var response = await _client.GetAsync($"search/multi?query={Uri.EscapeDataString(query)}&language=uk-UK&page={page}&include_adult=false");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var movieResponse = JsonConvert.DeserializeObject<MovieResponseModel>(content);
                movieResponse.Page = page;
                return View("SearchResults", movieResponse);
            }
            return View("Error");
        }

        public async Task<MovieModel> GetMovieById(int id)
        {
            var response = await _client.GetAsync($"movie/{id}?language=uk");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var movie = JsonConvert.DeserializeObject<MovieModel>(content);
                return movie;
            }
            return null;
        }

        public async Task<IActionResult> SearchMovie(string query, int page = 1)
        {
            ViewBag.SearchQuery = query;
            ViewBag.MovieGenres = _movieGenres;
            var response = await _client.GetAsync($"search/movie?query={Uri.EscapeDataString(query)}&language=uk-UK&page={page}&include_adult=false");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var movieResponse = JsonConvert.DeserializeObject<MovieResponseModel>(content);

                foreach (var result in movieResponse.Results)
                {
                    if (string.IsNullOrEmpty(result.MediaType))
                    {
                        result.MediaType = "movie";
                    }
                }

                movieResponse.Page = page;
                return View("SearchResults", movieResponse);
            }
            return View("Error");
        }

        public async Task<IActionResult> SearchTV(string query, int page = 1)
        {
            ViewBag.SearchQuery = query;
            ViewBag.MovieGenres = _movieGenres;
            ViewBag.TvGenres = _tvGenres;
            var response = await _client.GetAsync($"search/tv?query={Uri.EscapeDataString(query)}&include_adult=false&language=uk-UK&page={page}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var movieResponse = JsonConvert.DeserializeObject<MovieResponseModel>(content);

                foreach (var result in movieResponse.Results)
                {
                    if (string.IsNullOrEmpty(result.MediaType))
                    {
                        result.MediaType = "tv";
                    }
                }

                movieResponse.Page = page;
                return View("SearchResults", movieResponse);
            }
            return View("Error");
        }

        public async Task<IActionResult> SearchCartoons(string query, int page = 1)
        {
            ViewBag.SearchQuery = query;
            ViewBag.MovieGenres = _movieGenres;
            var response = await _client.GetAsync($"search/movie?query={Uri.EscapeDataString(query)}&language=uk-UK&page={page}&include_adult=false");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var movieResponse = JsonConvert.DeserializeObject<MovieResponseModel>(content);

                movieResponse.Results = movieResponse.Results.Where(movie => movie.Genres.Contains(16)).ToList();

                foreach (var result in movieResponse.Results)
                {
                    if (string.IsNullOrEmpty(result.MediaType))
                    {
                        result.MediaType = "movie";
                    }
                }

                movieResponse.Page = page;
                return View("SearchResults", movieResponse);
            }
            return View("Error");
        }


        //change pages ------------------------------------------------------------------------------

        public async Task<IActionResult> ChangeRatePage(int page = 1)
        {
            var response = await _client.GetAsync($"movie/top_rated?language=uk-UK&page={page}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var movieResponse = JsonConvert.DeserializeObject<MovieResponseModel>(content);

                foreach (var result in movieResponse.Results)
                {
                    if (string.IsNullOrEmpty(result.MediaType))
                    {
                        result.MediaType = "movie";
                    }
                }

                movieResponse.Page = page; // текущая страница
                ViewBag.MovieGenres = _movieGenres;
                return View("TMDB", movieResponse); 
            }
            return View("Error");
        }

        public async Task<IActionResult> ChangePopularPage(int page = 1)
        {
            var response = await _client.GetAsync($"https://api.themoviedb.org/3/movie/popular?language=uk-UK&page={page}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var movieResponse = JsonConvert.DeserializeObject<MovieResponseModel>(content);

                foreach (var result in movieResponse.Results)
                {
                    if (string.IsNullOrEmpty(result.MediaType))
                    {
                        result.MediaType = "movie";
                    }
                }

                movieResponse.Page = page; // текущая страница
                ViewBag.MovieGenres = _movieGenres;
                return View("TMDBPopular", movieResponse); 
            }
            return View("Error");
        }

        public async Task<IActionResult> ChangeSearchPage(string query, int page = 1)
        {
            ViewBag.SearchQuery = query;
            ViewBag.MovieGenres = _movieGenres;
            ViewBag.TvGenres = _tvGenres;
            var response = await _client.GetAsync($"search/multi?query={Uri.EscapeDataString(query)}&language=uk-UK&page={page}&include_adult=false");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var movieResponse = JsonConvert.DeserializeObject<MovieResponseModel>(content);
                movieResponse.Page = page; // текущая страница         
                return View("SearchResults", movieResponse);
            }
            return View("Error");
        }


        //робота з жанрами ---------------------------------------------------------------

        private async Task GetMovieGenres()
        {
            var response = await _client.GetAsync("genre/movie/list?language=uk");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var genres = JsonConvert.DeserializeObject<GenreModel>(content);
                _movieGenres = genres.Genres.ToDictionary(g => g.Id, g => g.Name);
            }
        }

        private async Task GetTvGenres()
        {
            var response = await _client.GetAsync("genre/tv/list?language=uk");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var genres = JsonConvert.DeserializeObject<GenreModel>(content);
                _tvGenres = genres.Genres.ToDictionary(g => g.Id, g => g.Name);
            }
        }
    }
}

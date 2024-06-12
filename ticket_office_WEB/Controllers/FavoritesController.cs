using Microsoft.AspNetCore.Mvc;
using BLL;
using Ninject;
using ticket_office_WEB.Models;
using DAL;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ticket_office_WEB.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        readonly IKernel ikernel = new StandardKernel(new IoCCon_BLL());
        private readonly TMDBController _tmdbController;
        private readonly IBLogic _logic;

        public FavoritesController(TMDBController tmdbController)
        {
            _logic = ikernel.Get<IBLogic>();
            _tmdbController = tmdbController;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ListOfFavorites()
        {
            var favorites = _logic.GetAllFavorites();

            var favoriteMovies = new List<MovieModel>();
            foreach (var favorite in favorites)
            {
                var movie = await _tmdbController.GetMovieById(favorite.MovieID);
                if (movie != null)
                {
                    favoriteMovies.Add(movie);
                }
            }
            return View("ListOfFavorites", favoriteMovies);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddToFavorites(int movieId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                _logic.AddFavorite(movieId, userId);
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "User not authenticated" });
        }


        public IActionResult Delete(int MovieId)
        {
            var logic = ikernel.Get<IBLogic>();
            var favorite = logic.GetAllFavorites();
            foreach (var c in favorite)
            {
                if (c.MovieID == MovieId)
                {
                    FavoriteModel favoriteModel = new()
                    {
                        MovieId = c.MovieID,
                    };
                    return View(favoriteModel);
                }
            }
            return NotFound();
        }

    }
}
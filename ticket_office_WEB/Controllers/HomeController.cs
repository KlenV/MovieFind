using BLL;
using Microsoft.AspNetCore.Mvc;
using Ninject;
using System.Diagnostics;
using ticket_office_WEB.Models;

namespace ticket_office_WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly IKernel ikernel = new StandardKernel(new IoCCon_BLL());

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var logic = ikernel.Get<IBLogic>();

            var MovieHouses = logic.GetAllFavorites();
            return View(MovieHouses);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
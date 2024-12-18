using System.Diagnostics;
using Kursverwaltung.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kursverwaltung.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController( ILogger<HomeController> logger )
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToAction( "Index" , "Kurse" );
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache( Duration = 0 , Location = ResponseCacheLocation.None , NoStore = true )]
        public IActionResult Error()
        {
            return View( new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier } );
        }
    }
}
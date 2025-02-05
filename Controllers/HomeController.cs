using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BaseballStatsApp.Controllers
{
    public class HomeController : Controller
    {
        // Display the main HTML page
        public IActionResult Index()
        {
            return View();
        }

    }

  
}


using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace StatsTracker.Controllers
{
    public class AddController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddGame(BatterGameModel model)
        {
            if (ModelState.IsValid)
            {
                // Here you can process the model data, for example:
                // Save it to a database, send it to another service, etc.

                // For now, we can return a success message
                return Content("Game added successfully!");  // You can redirect to another page or render a view.
            }

            // If the model is not valid, return the same view with validation errors
            return View("Index", model);  // You can return a different view if necessary
        }


    }
}

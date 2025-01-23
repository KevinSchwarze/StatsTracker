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

        [HttpPost]
        public IActionResult AddBatterGame([FromBody] GameData game)
        {
            // Validate game data
            if (game == null || string.IsNullOrEmpty(game.PlayerName))
            {
                return BadRequest("Invalid game data.");
            }

            // File path where game data will be stored
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "BatterData.json");

            try
            {
                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                // Read existing data from the file
                List<GameData> games;
                if (System.IO.File.Exists(filePath))
                {
                    string existingData = System.IO.File.ReadAllText(filePath);
                    games = string.IsNullOrEmpty(existingData)
                        ? new List<GameData>()
                        : System.Text.Json.JsonSerializer.Deserialize<List<GameData>>(existingData);
                }
                else
                {
                    games = new List<GameData>();
                }

                // Append the new game data
                games.Add(game);

                // Write the updated list back to the file
                string jsonData = System.Text.Json.JsonSerializer.Serialize(games, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText(filePath, jsonData);

                // Return success response
                return Ok(new { message = "Batter game added successfully!" });
            }
            catch (Exception ex)
            {
                // Handle errors (e.g., file write permission issues)
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPost]
        public JsonResult Update()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "gamedata.json");

            // Read the JSON file
            if (!System.IO.File.Exists(filePath))
            {
                return Json(new { error = "Data file not found." });
            }

            string jsonData = System.IO.File.ReadAllText(filePath);

            // Deserialize the JSON data into a list of player stats
            var playerStats = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PlayerStats>>(jsonData);

            if (playerStats == null)
            {
                return Json(new { error = "Failed to parse data." });
            }

            // Sort the data by Player, Year, and Team
            var sortedStats = playerStats
                .OrderBy(p => p.PlayerName)
                .ThenBy(p => p.TeamYear) // Assuming GameDate is a DateTime property
                .ThenBy(p => p.TeamName)
                .ToList();

            // Return the sorted data as JSON
            return Json(sortedStats);
        }
    }


    public class GameData
    {
        public string PlayerName { get; set; }
        public string TeamName { get; set; }
        public string GameDate { get; set; }
        public int AB { get; set; }       // At Bats
        public int H { get; set; }        // Hits
        public int Doubles { get; set; }  // Doubles
        public int Triples { get; set; }  // Triples
        public int HR { get; set; }       // Home Runs
        public int R { get; set; }        // Runs
    }

}

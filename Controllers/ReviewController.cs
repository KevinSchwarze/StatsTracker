using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace StatsTracker.Controllers
{
    public class ReviewController : Controller
    {
        // GET: ReviewController
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetPlayerData(string username, string dataType, string filterOption)
        {
            string dataPath = (dataType.ToUpper() == "BATTERSTATS.JSON") ?
                Path.Combine(Directory.GetCurrentDirectory(), "data", "BatterData.json") :
                Path.Combine(Directory.GetCurrentDirectory(), "data", "PitcherData.json");

            try
            {
                if (string.IsNullOrEmpty(dataPath) || !System.IO.File.Exists(dataPath))
                {
                    ModelState.AddModelError("", "Invalid data type or file not found.");
                    return View("Error");
                }

                string jsonData = System.IO.File.ReadAllText(dataPath).Trim();

                // Ensure JSON is in an array format
                if (!jsonData.StartsWith("[") && !jsonData.EndsWith("]"))
                {
                    jsonData = "[" + jsonData + "]";
                }

                List<PlayerStats> allData = new List<PlayerStats>();

                // Deserialize based on dataType without casting
                if (dataType.ToUpper() == "BATTERSTATS.JSON")
                {
                    // Deserialize directly to List<BatterGameModel>
                    var batterData = JsonSerializer.Deserialize<List<BatterGameModel>>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    // Ensure that allData is populated with the deserialized data
                    allData = batterData.Select(batter => new PlayerStats
                    {
                        PlayerName = batter.PlayerName,
                        TeamName = batter.TeamName,
                        GameDate = batter.GameDate,
                        AB = batter.AB,
                        H = batter.H,
                        Doubles = batter.Doubles,
                        Triples = batter.Triples,
                        HR = batter.HR,
                        R = batter.R,
                        RBI = batter.RBI,
                        BB = batter.BB,
                        SO = batter.SO,
                        SB = batter.SB,
                        CS = batter.CS
                    }).ToList();
                }
                else
                {
                    // Deserialize directly to List<PitcherGameModel>
                    var pitcherData = JsonSerializer.Deserialize<List<PitcherGameModel>>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    // Map PitcherGameModel to PlayerStats
                    allData = pitcherData.Select(pitcher => new PlayerStats
                    {
                        PlayerName = pitcher.PlayerName,
                        TeamName = pitcher.TeamName,
                        GameDate = pitcher.GameDate,
                        W = pitcher.W,
                        L = pitcher.L,
                        Runs = pitcher.Runs,
                        SO = pitcher.SO
                        // Map any other PitcherGameModel properties to PlayerStats as needed
                    }).ToList();
                }

                if (allData == null || allData.Count == 0)
                {
                    ModelState.AddModelError("", "No player data found.");
                    return View("Error");
                }

                // Filter by username if provided
                var userStats = string.IsNullOrEmpty(username)
                    ? allData  // Show all stats if no username is provided
                    : allData.Where(data => data.PlayerName == username).ToList();

                // If filterOption is provided, filter by TeamName
                if (!string.IsNullOrEmpty(filterOption))
                {
                    userStats = userStats.Where(data => data.TeamName.Contains(filterOption, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                // Return appropriate view based on dataType (batting or pitching)
                if (dataType.ToUpper() == "BATTERSTATS.JSON")
                {
                    return View("StatsView", userStats);  // Batting stats view
                }
                else
                {
                    return View("PitchingStatsView", userStats);  // Pitching stats view
                }
            }
            catch (JsonException)
            {
                ModelState.AddModelError("", "Error parsing JSON file.");
                return View("Error");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An unexpected error occurred.");
                return View("Error");
            }
        }





        [HttpPost]
        public IActionResult GetAllData(string dataType)
        {
            if (string.IsNullOrEmpty(dataType) || !System.IO.File.Exists(dataType))
            {
                return BadRequest("Invalid data type or file not found.");
            }

            string jsonData = System.IO.File.ReadAllText(dataType);
            var allData = JsonSerializer.Deserialize<List<PlayerStats>>(jsonData);

            return View("StatsView", allData);
        }
    }



}


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
        public IActionResult AddGame(BatterGameModel batterModel, PitcherGameModel pitcherModel, string gameType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string jsonData = string.Empty;
                    string dataPath = string.Empty;

                    // Determine if the game is for a Batter or Pitcher based on the gameType
                    if (gameType == "Batter" && batterModel != null && !string.IsNullOrEmpty(batterModel.PlayerName))
                    {
                        // Define file path for batter stats
                        dataPath = Path.Combine(Directory.GetCurrentDirectory(), "data", "BatterData.json");

                        // Serialize the batter model to JSON
                        jsonData = JsonConvert.SerializeObject(batterModel, Formatting.Indented);
                    }
                    else if (gameType == "Pitcher" && pitcherModel != null && !string.IsNullOrEmpty(pitcherModel.PlayerName))
                    {
                        // Define file path for pitcher stats
                        dataPath = Path.Combine(Directory.GetCurrentDirectory(), "data", "PitcherData.json");

                        // Reverse the conversion of pitcherModel properties from strings to their original types
                        var pitcherDataAsStrings = new
                        {
                            W = pitcherModel.W.ToString(),
                            L = pitcherModel.L.ToString(),
                            IP = pitcherModel.IP.ToString(),
                            HA = pitcherModel.HA.ToString(),
                            Runs = pitcherModel.Runs.ToString(),
                            SO = pitcherModel.SO.ToString(),
                            TeamName = pitcherModel.TeamName,
                            PlayerName = pitcherModel.PlayerName,
                            GameDate = pitcherModel.GameDate.ToString("yyyy-MM-dd"), // Ensure the date is in string format
                            ERA = pitcherModel.ERA.ToString()
                        };

                        // Deserialize it back into the correct types
                        var pitcherModelRestored = new PitcherGameModel
                        {
                            W = int.Parse(pitcherDataAsStrings.W),
                            L = int.Parse(pitcherDataAsStrings.L),
                            IP = double.Parse(pitcherDataAsStrings.IP),
                            HA = int.Parse(pitcherDataAsStrings.HA),
                            Runs = double.Parse(pitcherDataAsStrings.Runs),
                            SO = int.Parse(pitcherDataAsStrings.SO),
                            TeamName = pitcherDataAsStrings.TeamName,
                            PlayerName = pitcherDataAsStrings.PlayerName,
                            GameDate = DateTime.Parse(pitcherDataAsStrings.GameDate),
 
                        };

                        // Serialize the restored pitcherModel to JSON
                        jsonData = JsonConvert.SerializeObject(pitcherModelRestored, Formatting.Indented);
                    }
                    else
                    {
                        return Content("No valid game data provided.");
                    }

                    // Ensure the directory exists
                    var directory = Path.GetDirectoryName(dataPath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Check if the file exists and has data
                    if (!System.IO.File.Exists(dataPath) || new FileInfo(dataPath).Length == 0)
                    {
                        // If the file is empty, start with an opening bracket and add the new data
                        jsonData = "[" + jsonData + "]";
                        System.IO.File.WriteAllText(dataPath, jsonData);
                    }
                    else
                    {
                        // If the file exists, read the existing data and append the new data
                        var existingData = System.IO.File.ReadAllText(dataPath).Trim();

                        // Check if the existing data ends with a closing bracket ']', indicating it's an array
                        if (existingData.EndsWith("]"))
                        {
                            // Remove the closing bracket ']' and add a comma before appending the new data
                            existingData = existingData.Substring(0, existingData.Length - 1);
                        }

                        // Append the new data with a comma if it's not the first item
                        existingData += "," + jsonData;

                        // Close the JSON array by adding "]"
                        existingData += "]";

                        // Write the updated content back to the file
                        System.IO.File.WriteAllText(dataPath, existingData);
                    }

                    // Return success message with form reset script
                    return Content("Game Added");
                }
                catch (Exception ex)
                {
                    // Log the exception (optional)
                    return Content($"An error occurred while writing to the file: {ex.Message}");
                }
            }

            // If the model is not valid, return the same view with validation errors
            return View("Index", new { batterModel, pitcherModel });
        }







    }
}

public class PitcherGameModel
{
    public int W { get; set; } // Wins
    public int L { get; set; } // Losses
    public double IP { get; set; } // Innings Pitched
    public int HA { get; set; } // Hits Allowed
    public double Runs { get; set; } // Runs Allowed
    public int SO { get; set; } // Strikeouts
    public string TeamName { get; set; }
    public string PlayerName { get; set; }
    public DateTime GameDate { get; set; }

    // Calculate ERA dynamically, do not include it during serialization
    public double ERA
    {
        get
        {
            return IP == 0 ? 0 : (Runs / IP) * 9; // ERA = (Runs / IP) * 9
        }
    }

    // Add any additional methods or properties needed for your use case
}

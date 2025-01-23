using System;
using System.Collections.Generic;
using System.Linq;

// Player Stats class
public class PlayerStats
{
    public string PlayerName { get; set; }
    public string PlayerID { get; set; }
    public string TeamName { get; set; }

    public DateTime TeamYear { get; set; }
    public BatterStats BatterStats { get; set; }
    public PitcherStats PitcherStats { get; set; }

    public PlayerStats(string playerName, string playerID, string teamName)
    {
        PlayerName = playerName;
        PlayerID = playerID;
        TeamName = teamName;
        BatterStats = new BatterStats();
        PitcherStats = new PitcherStats();
    }

    public void AddGameBatterStats(BatterStats gameStats)
    {
        BatterStats.AddGameStats(gameStats);
    }

    public void AddGamePitcherStats(PitcherStats gameStats)
    {
        PitcherStats.AddGameStats(gameStats);
    }

    public void CompileSeasonStats()
    {
        BatterStats.CalculateSeasonStats();
    
    }
}

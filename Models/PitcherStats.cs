public class PitcherStats
{
    public int W { get; set; } // Wins
    public int L { get; set; } // Losses
    public double IP { get; set; } // Innings Pitched
    public int HA { get; set; } // Hits Allowed
    public int RA { get; set; } // Runs Allowed
    public int SO { get; set; } // Strikeouts
    public string? Team { get; set; }
    public double ERA => IP == 0 ? 0 : (RA / IP) * 9; // Earned Run Average

    public DateTime GameDate { get; set; }

    public void AddGameStats(PitcherStats gameStats)
    {
        W += gameStats.W;
        L += gameStats.L;
        IP += gameStats.IP;
        HA += gameStats.HA;
        RA += gameStats.RA;
        SO += gameStats.SO;
    }
}


public class BatterStats
{
    public int AB { get; set; } // At Bats
    public int R { get; set; }  // Runs
    public int H { get; set; }  // Hits
    public int Doubles { get; set; } // 2B
    public int Triples { get; set; } // 3B
    public int HR { get; set; } // Home Runs
    public int RBI { get; set; } // Runs Batted In
    public int BB { get; set; } // Base on Balls
    public int SO { get; set; } // Strikeouts
    public int SB { get; set; } // Stolen Bases
    public int CS { get; set; } // Caught Stealing

    public DateTime GameDate { get; set; }
    public string? Team {get; set; }

    public double AVG => AB == 0 ? 0 : (double)H / AB; // Batting Average

    public void AddGameStats(BatterStats gameStats)
    {
        AB += gameStats.AB;
        R += gameStats.R;
        H += gameStats.H;
        Doubles += gameStats.Doubles;
        Triples += gameStats.Triples;
        HR += gameStats.HR;
        RBI += gameStats.RBI;
        BB += gameStats.BB;
        SO += gameStats.SO;
        SB += gameStats.SB;
        CS += gameStats.CS;
    }

    public void CalculateSeasonStats()
    {
        // In this context, the calculation logic is already integrated into the properties.
    }
}
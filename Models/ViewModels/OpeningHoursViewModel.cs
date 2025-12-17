namespace RVBARBER.ViewModels;

public class ModificaOrario
{
    public TimeSpan MorningOpen { get; set; }
    
    public TimeSpan MorningClose { get; set; }
    
    public TimeSpan AfternoonOpen { get; set; }
    
    public TimeSpan AfternoonClose { get; set; }

    public bool IsClosed { get; set; }
}
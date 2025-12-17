namespace RVBARBER.ViewModels;

public class ClosureViewModel 
{
    public DateTime? Date { get; set; }

    public TimeSpan? MorningOpen { get; set; }
    
    public TimeSpan? MorningClose { get; set; }
    
    public TimeSpan? AfternoonOpen { get; set; }
    
    public TimeSpan? AfternoonClose { get; set; }

    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }

    public bool IsOpen { get; set; }
}
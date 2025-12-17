using System.ComponentModel.DataAnnotations;

namespace RVBARBER.Models;

public class OpeningHour
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DayOfWeek DayOfWeek { get; set; }

    public TimeSpan MorningOpen { get; set; }
    
    public TimeSpan MorningClose { get; set; }
    
    public TimeSpan AfternoonOpen { get; set; }
    
    public TimeSpan AfternoonClose { get; set; }

    public bool IsClosed { get; set; }
}
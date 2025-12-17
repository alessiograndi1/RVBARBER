using RVBARBER.Models;

namespace RVBARBER.ViewModels;

public class HomePageViewModel
{
    public List<OpeningHour> OpeningHours { get; set; } = null!;
    public List<Treatment> Treatments { get; set; } = null!;
}
using RVBARBER.Models;

namespace RVBARBER.ViewModels;

public class ReservationViewModel
{
    public List<string> Trattamenti { get; set; } = [];
    public decimal Totale { get; set; }
    public decimal Durata { get; set; }
    public DateTime Slot { get; set; }
    public string Barbiere { get; set; } = null!;
    public string Cliente { get; set; } = null!;
}

public class PrenotaOraViewModel
{
    public List<Treatment>? Trattamenti { get; set; }
    public List<User>? Users { get; set; }
}
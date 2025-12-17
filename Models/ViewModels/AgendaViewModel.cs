using RVBARBER.Models;

namespace RVBARBER.ViewModels;

public class AgendaViewModel
{
    public DateTime Date { get; set; }
    public List<DateTime>? Slots { get; set; }
    public List<Reservation>? Reservations { get; set; }

    public List<User>? Users { get; set; }
    public List<Treatment>? Treatments { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace RVBARBER.Models;

public class Reservation
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime DataPrenotazione { get; set; } = DateTime.Now;

    [Required]
    public DateTime DataAppuntamento { get; set; }

    [Required]
    public decimal Totale { get; set; }

    [Required]
    public string TrattamentiJson { get; set; } = null!;

    [Required]
    public decimal Durata { get; set; }

    public string? Cliente { get; set; }
    
    public string? Phone { get; set; }

    public User User { get; set; } = null!;

    [Required]
    public Guid UserId { get; set; }

    public string? Barbiere { get; set; }
}

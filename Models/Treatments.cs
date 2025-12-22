using System.ComponentModel.DataAnnotations;

namespace RVBARBER.Models;

public class Treatment
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string NomeTrattamento { get; set; } = null!;

    public string? Descrizione { get; set; }

    [Required]
    public double Prezzo { get; set; }

    [Required]
    public int DurataMinuti { get; set; }

    [Required]
    public string Categoria { get; set; } = null!;
}

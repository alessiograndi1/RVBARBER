namespace RVBARBER.ViewModels;

public class TreatmentViewModel
{
    public string NomeTrattamento { get; set; } = null!;
    public string? Descrizione { get; set; }
    public decimal Prezzo { get; set; }
    public int DurataMinuti { get; set; }
    public string Categoria { get; set; } = null!;
}
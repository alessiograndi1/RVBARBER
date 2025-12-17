using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RVBARBER.Data;
using RVBARBER.Models;
using RVBARBER.ViewModels;

namespace RVBARBER.Controllers;

[Authorize]
public class ReservationsController(ILogger<ReservationsController> logger, AppDbContext dbContext) : Controller
{
    private readonly ILogger<ReservationsController> _logger = logger;
    private readonly AppDbContext _dbContext = dbContext;

    [HttpGet("/reservations")]
    public async Task<IActionResult> Index()
    {
        var servizi = await _dbContext.Treatments.ToListAsync();
        var barbieri = await _dbContext.Users.Where(x => x.Role == "admin").ToListAsync();

        var model = new PrenotaOraViewModel { Trattamenti = servizi, Users = barbieri };

        return View(model);
    }

    [HttpGet("/reservations/available-slots")]
    public IActionResult GetDisponibilita([FromQuery] DateTime date, string barber, int durataMinuti)
    {
        var intervallo = TimeSpan.FromMinutes(15);
        var slotsDisponibili = new List<DateTime>();

        // Cerca eventuale chiusura o apertura straordinaria
        var chiusuraStraordinaria = _dbContext.Closures
            .FirstOrDefault(c =>
                (c.Date.HasValue && c.Date.Value.Date == date.Date) ||
                (c.StartDate.HasValue && c.EndDate.HasValue &&
                c.StartDate.Value.Date <= date.Date && c.EndDate.Value.Date >= date.Date)
            );

        TimeSpan morningOpen, morningClose, afternoonOpen, afternoonClose;

        if (chiusuraStraordinaria != null)
        {
            if (!chiusuraStraordinaria.IsOpen)
            {
                return Ok(new List<string>()); // Chiuso straordinariamente
            }

            // Usa orari straordinari se disponibili, altrimenti default a TimeSpan.Zero
            morningOpen = chiusuraStraordinaria.MorningOpen ?? TimeSpan.Zero;
            morningClose = chiusuraStraordinaria.MorningClose ?? TimeSpan.Zero;
            afternoonOpen = chiusuraStraordinaria.AfternoonOpen ?? TimeSpan.Zero;
            afternoonClose = chiusuraStraordinaria.AfternoonClose ?? TimeSpan.Zero;
        }
        else
        {
            // Orari standard settimanali
            var openingHours = _dbContext.OpeningHours
                .FirstOrDefault(o => o.DayOfWeek == date.DayOfWeek);

            if (openingHours == null || openingHours.IsClosed)
            {
                return Ok(new List<string>()); // Giorno normalmente chiuso
            }

            morningOpen = openingHours.MorningOpen;
            morningClose = openingHours.MorningClose;
            afternoonOpen = openingHours.AfternoonOpen;
            afternoonClose = openingHours.AfternoonClose;
        }

        void AggiungiSlot(TimeSpan start, TimeSpan end)
        {
            if (start == end || (start == TimeSpan.Zero && end == TimeSpan.Zero))
            {
                return;
            }

            var current = date.Date + start;
            var fine = date.Date + end;

            while (current + intervallo <= fine)
            {
                slotsDisponibili.Add(current);
                current += intervallo;
            }
        }

        // Mattina
        AggiungiSlot(morningOpen, morningClose);

        // Pomeriggio
        AggiungiSlot(afternoonOpen, afternoonClose);

        // Recupera le prenotazioni per il barbiere in quella data
        var prenotazioni = _dbContext.Reservations
            .Where(r => r.Barbiere == barber && r.DataAppuntamento.Date == date.Date)
            .ToList();

        // Calcola gli slot occupati in base alla durata dei trattamenti
        var slotOccupati = new HashSet<DateTime>();

        foreach (var res in prenotazioni)
        {
            var nomiTrattamenti = JsonSerializer.Deserialize<List<string>>(res.TrattamentiJson) ?? new();
            var durataTotale = _dbContext.Treatments
                .Where(t => nomiTrattamenti.Contains(t.NomeTrattamento))
                .Sum(t => t.DurataMinuti);

            var inizio = res.DataAppuntamento;
            var fine = inizio.AddMinutes(durataTotale);

            var blocco = inizio;
            while (blocco < fine)
            {
                slotOccupati.Add(blocco);
                blocco += intervallo;
            }
        }

        // var slotLiberi = slotsDisponibili
        //     .Where(s => !slotOccupati.Contains(s))
        //     .Select(s => s.ToString("yyyy-MM-ddTHH:mm"))
        //     .ToList();

        // return Ok(slotLiberi);

        // Filtra gli slot per avere abbastanza intervalli consecutivi liberi
        var slotValidi = new List<string>();
        var intervalliRichiesti = (int)Math.Ceiling(durataMinuti / 15.0);

        foreach (var slot in slotsDisponibili)
        {
            bool isDisponibile = true;
            for (int i = 0; i < intervalliRichiesti; i++)
            {
                var s = slot + TimeSpan.FromMinutes(15 * i);
                if (!slotsDisponibili.Contains(s) || slotOccupati.Contains(s))
                {
                    isDisponibile = false;
                    break;
                }
            }

            if (isDisponibile)
            {
                slotValidi.Add(slot.ToString("yyyy-MM-ddTHH:mm"));
            }
        }

        return Ok(slotValidi);
    }

    [HttpPost("/reservations")]
    public async Task<IActionResult> Prenota([FromBody] ReservationViewModel reservation)
    {
        try
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized(new { message = "Claim NameIdentifier non valido." });
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return Unauthorized(new { message = "Utente non trovato nel database." });
            }

            if(reservation.Cliente == null)
            {
                var newReservation = new Reservation
                {
                    DataAppuntamento = reservation.Slot,
                    Totale = reservation.Totale,
                    TrattamentiJson = JsonSerializer.Serialize(reservation.Trattamenti),
                    Durata = reservation.Durata,
                    Cliente = $"{user.Name} {user.Surname}",
                    Phone = user.Phone,
                    UserId = userId,
                    Barbiere = reservation.Barbiere
                };

                _dbContext.Reservations.Add(newReservation);
            }
            else
            {
                var nome = reservation.Cliente!.Split(" ")[0];
                var cognome = reservation.Cliente.Split(" ")[1];
                var cliente = await _dbContext.Users.Where(x => x.Name == nome && x.Surname == cognome).FirstOrDefaultAsync();

                if(cliente != null)
                {
                    var newReservation = new Reservation
                    {
                        DataAppuntamento = reservation.Slot,
                        Totale = reservation.Totale,
                        TrattamentiJson = JsonSerializer.Serialize(reservation.Trattamenti),
                        Durata = reservation.Durata,
                        Cliente = $"{nome} {cognome}",
                        Phone = user.Phone,
                        UserId = cliente.Id,
                        Barbiere = reservation.Barbiere
                    };

                    _dbContext.Reservations.Add(newReservation);
                }
            }

            await _dbContext.SaveChangesAsync();

            return Ok(new { success = true });
        }
        catch (DbUpdateException dbEx)
        {
            return StatusCode(500, new { message = "Errore durante il salvataggio della prenotazione.", detail = dbEx.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Si è verificato un errore inatteso.", detail = ex.Message });
        }
    }

    [HttpPut("/reservations")]
    public async Task<IActionResult> ModificaPrenotazione(int id, [FromBody] ReservationViewModel reservation)
    {
        try
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized(new { message = "Claim NameIdentifier non valido." });
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return Unauthorized(new { message = "Utente non trovato nel database." });
            }

            var currentReservation = await _dbContext.Reservations.Where(x => x.Id == id).FirstOrDefaultAsync();
            if(currentReservation != null)
            {
                var nome = reservation.Cliente!.Split(" ")[0];
                var cognome = reservation.Cliente.Split(" ")[1];
                
                var cliente = await _dbContext.Users.Where(x => x.Name == nome && x.Surname == cognome).FirstOrDefaultAsync();
                if(cliente != null)
                {
                    _dbContext.Reservations.Remove(currentReservation);

                    var newReservation = new Reservation
                    {
                        DataAppuntamento = reservation.Slot,
                        Totale = reservation.Totale,
                        TrattamentiJson = JsonSerializer.Serialize(reservation.Trattamenti),
                        Durata = reservation.Durata,
                        Cliente = $"{cliente!.Name} {cliente!.Surname}",
                        Phone = cliente.Phone,
                        UserId = cliente.Id,
                        Barbiere = reservation.Barbiere
                    };

                    _dbContext.Reservations.Add(newReservation);
                    await _dbContext.SaveChangesAsync();

                    return Ok(new { success = true, reservationId = newReservation.Id });
                }
            }

            return NotFound();
        }
        catch (DbUpdateException dbEx)
        {
            return StatusCode(500, new { message = "Errore durante il salvataggio della prenotazione.", detail = dbEx.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Si è verificato un errore inatteso.", detail = ex.Message });
        }
    }

    [HttpDelete("/reservations/{id}")]
    public async Task<IActionResult> DeleteReservation(int id)
    {
        var reservation = await _dbContext.Reservations
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if(reservation != null)
        {
            _dbContext.Reservations.Remove(reservation);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        return NotFound();
    }
}

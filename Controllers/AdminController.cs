using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RVBARBER.Data;
using RVBARBER.Models;
using RVBARBER.ViewModels;

namespace RVBARBER.Controllers;

[Authorize]
public class AdminController(ILogger<AccountController> logger, AppDbContext dbContext) : Controller
{
    private readonly ILogger<AccountController> _logger = logger;
    private readonly AppDbContext _dbContext = dbContext;

    [HttpGet("/admin")]
    public IActionResult Home()
    {
        var role = User.FindFirstValue(ClaimTypes.Role);

        if (role != "admin")
        {
            return Unauthorized(new { message = "Claim Role non valido." });
        }

        return View();
    }

    [HttpGet("/admin/orari")]
    public async Task<IActionResult> Orari()
    {
        var role = User.FindFirstValue(ClaimTypes.Role);

        if (role != "admin")
        {
            return Unauthorized(new { message = "Claim Role non valido." });
        }

        var openingHours = await _dbContext.OpeningHours.ToListAsync();

        return View(openingHours);
    }

    [HttpPut("/admin/orari/{id}")]
    public async Task<IActionResult> ModificaOrario(int id, [FromForm] ViewModels.ModificaOrario nuovoOrario)
    {
        var role = User.FindFirstValue(ClaimTypes.Role);

        if (role != "admin")
        {
            return Unauthorized(new { message = "Claim Role non valido." });
        }

        var orario = await _dbContext.OpeningHours
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if(orario != null)
        {
            orario.IsClosed = nuovoOrario.IsClosed;
            orario.MorningOpen = nuovoOrario.MorningOpen;
            orario.MorningClose = nuovoOrario.MorningClose;
            orario.AfternoonOpen = nuovoOrario.AfternoonOpen;
            orario.AfternoonClose = nuovoOrario.AfternoonClose;

            await _dbContext.SaveChangesAsync();
        }

        return NoContent();
    }

    [HttpGet("/admin/trattamenti")]
    public async Task<IActionResult> Trattamenti()
    {
        var role = User.FindFirstValue(ClaimTypes.Role);

        if (role != "admin")
        {
            return Unauthorized(new { message = "Claim Role non valido." });
        }

        var trattamenti = await _dbContext.Treatments.ToListAsync();

        return View(trattamenti);
    }

    [HttpPost("/admin/trattamenti")]
    public async Task<IActionResult> NuovoTrattamento([FromForm] ViewModels.TreatmentViewModel nuovoTrattamento)
    {
        var role = User.FindFirstValue(ClaimTypes.Role);

        if (role != "admin")
        {
            return Unauthorized(new { message = "Claim Role non valido." });
        }

        await _dbContext.Treatments.AddAsync(new Treatment {
            NomeTrattamento = nuovoTrattamento.NomeTrattamento,
            Descrizione = nuovoTrattamento.Descrizione,
            Prezzo = nuovoTrattamento.Prezzo,
            DurataMinuti = nuovoTrattamento.DurataMinuti,
            Categoria = nuovoTrattamento.Categoria
        });

        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("/admin/trattamenti/{id}")]
    public async Task<IActionResult> ModificaTrattamento(int id, [FromForm] ViewModels.TreatmentViewModel modificaTrattamento)
    {
        var role = User.FindFirstValue(ClaimTypes.Role);

        if (role != "admin")
        {
            return Unauthorized(new { message = "Claim Role non valido." });
        }

        var trattamento = await _dbContext.Treatments
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if(trattamento != null)
        {
            trattamento.NomeTrattamento = modificaTrattamento.NomeTrattamento;
            trattamento.Descrizione = modificaTrattamento.Descrizione;
            trattamento.Prezzo = modificaTrattamento.Prezzo;
            trattamento.DurataMinuti = modificaTrattamento.DurataMinuti;
            trattamento.Categoria = modificaTrattamento.Categoria;

            await _dbContext.SaveChangesAsync();
        }

        return NoContent();
    }

    [HttpDelete("/admin/trattamenti/{id}")]
    public async Task<IActionResult> EliminaTrattamento(int id)
    {
        var role = User.FindFirstValue(ClaimTypes.Role);

        if (role != "admin")
        {
            return Unauthorized(new { message = "Claim Role non valido." });
        }

        var trattamento = await _dbContext.Treatments
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if(trattamento != null)
        {
            _dbContext.Treatments.Remove(trattamento);

            await _dbContext.SaveChangesAsync();
        }

        return NoContent();
    }

    [HttpGet("/admin/chiusure")]
    public async Task<IActionResult> Chiusure()
    {
        var role = User.FindFirstValue(ClaimTypes.Role);

        if (role != "admin")
        {
            return Unauthorized(new { message = "Claim Role non valido." });
        }

        var chiusure = await _dbContext.Closures.ToListAsync();

        return View(chiusure);
    }

    [HttpPost("/admin/chiusure")]
    public async Task<IActionResult> AggiungiChiusura([FromForm] ClosureViewModel closure)
    {
        var role = User.FindFirstValue(ClaimTypes.Role);

        if (role != "admin")
        {
            return Unauthorized(new { message = "Claim Role non valido." });
        }

        if(closure.Date.HasValue)
        {
            await _dbContext.Closures.AddAsync(new Closure {
                Date = closure.Date.Value,
                MorningOpen = closure.MorningOpen,
                MorningClose = closure.MorningClose,
                AfternoonOpen = closure.AfternoonOpen,
                AfternoonClose = closure.AfternoonClose,
                IsOpen = closure.IsOpen
            });
        }
        else if(closure.StartDate.HasValue && closure.EndDate.HasValue)
        {
            await _dbContext.Closures.AddAsync(new Closure {
                StartDate = closure.StartDate,
                EndDate = closure.EndDate,
                IsOpen = closure.IsOpen
            });
        }

        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("/admin/chiusure/{id}")]
    public async Task<IActionResult> EliminaChiusura(int id)
    {
        var role = User.FindFirstValue(ClaimTypes.Role);

        if (role != "admin")
        {
            return Unauthorized(new { message = "Claim Role non valido." });
        }

        var chiusura = await _dbContext.Closures.Where(x => x.Id == id).FirstOrDefaultAsync();

        if(chiusura != null)
        {
            _dbContext.Closures.Remove(chiusura);

            await _dbContext.SaveChangesAsync();
        }

        return NoContent();
    }

    [HttpGet("/admin/prenotazioni")]
    public async Task<IActionResult> Prenotazioni()
    {
        var role = User.FindFirstValue(ClaimTypes.Role);

        if (role != "admin")
        {
            return Unauthorized(new { message = "Claim Role non valido." });
        }

        var prenotazioni = await _dbContext.Reservations.ToListAsync();

        return View(prenotazioni);
    }

    [HttpDelete("/admin/prenotazioni/{id}")]
    public async Task<IActionResult> EliminaPrenotazione(int id)
    {
        var role = User.FindFirstValue(ClaimTypes.Role);

        if (role != "admin")
        {
            return Unauthorized(new { message = "Claim Role non valido." });
        }

        var prenotazione = await _dbContext.Reservations.Where(x => x.Id == id).FirstOrDefaultAsync();

        if(prenotazione != null)
        {
            _dbContext.Reservations.Remove(prenotazione);

            await _dbContext.SaveChangesAsync();
        }

        return NoContent();
    }

    [HttpGet("/admin/agenda")]
    public async Task<IActionResult> Agenda([FromQuery] string date)
    {
        DateTime currentDate = DateTime.Parse(date);

        var intervallo = TimeSpan.FromMinutes(15);
        var slotsDisponibili = new List<DateTime>();

        var role = User.FindFirstValue(ClaimTypes.Role);

        if (role != "admin")
        {
            return Unauthorized(new { message = "Claim Role non valido." });
        }

        // Cerca eventuale chiusura o apertura straordinaria
        var chiusuraStraordinaria = _dbContext.Closures
            .FirstOrDefault(c =>
                (c.Date.HasValue && c.Date.Value.Date == currentDate.Date) ||
                (c.StartDate.HasValue && c.EndDate.HasValue &&
                c.StartDate.Value.Date <= currentDate.Date && c.EndDate.Value.Date >= currentDate.Date)
            );

        TimeSpan morningOpen, morningClose, afternoonOpen, afternoonClose;

        if (chiusuraStraordinaria != null)
        {
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
                .FirstOrDefault(o => o.DayOfWeek == currentDate.DayOfWeek);

            morningOpen = openingHours!.MorningOpen;
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

            var current = currentDate.Date + start;
            var fine = currentDate.Date + end;

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

        var prenotazioni = await _dbContext.Reservations.Where(x => x.DataAppuntamento.Date == currentDate.Date && x.Barbiere == User.Identity!.Name).ToListAsync();
        var clienti = await _dbContext.Users.ToListAsync();
        var trattamenti = await _dbContext.Treatments.ToListAsync();

        return View(new AgendaViewModel {
            Date = currentDate,
            Slots = slotsDisponibili,
            Reservations = prenotazioni,
            Users = clienti,
            Treatments = trattamenti
        });
    }

    [HttpGet("/admin/clienti")]
    public async Task<IActionResult> Clienti()
    {
        var role = User.FindFirstValue(ClaimTypes.Role);

        if (role != "admin")
        {
            return Unauthorized(new { message = "Claim Role non valido." });
        }

        var clienti = await _dbContext.Users.Where(x => x.Role == "base").ToListAsync();

        return View(clienti);
    }

}

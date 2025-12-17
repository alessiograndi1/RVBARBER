using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RVBARBER.Data;
using RVBARBER.Models;

namespace RVBARBER.Controllers;

[Route("account")]
public class AccountController(ILogger<AccountController> logger, AppDbContext dbContext) : Controller
{
    private readonly ILogger<AccountController> _logger = logger;
    private readonly AppDbContext _dbContext = dbContext;

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest model)
    {
        var utente = await _dbContext.Users
            .Where(x => x.Email == model.Email 
                && x.Password == model.Password)
            .FirstOrDefaultAsync();

        if(utente != null)
        {
            var claims = new List<Claim> 
            {
                new(ClaimTypes.NameIdentifier, utente.Id.ToString()),
                new(ClaimTypes.Role, utente.Role.ToString()),
                new(ClaimTypes.Name, $"{utente.Name} {utente.Surname}"),
                new(ClaimTypes.Email, utente.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok(utente); 
        }

        return Unauthorized();
    }

    [HttpPost("registration")]
    public async Task<IActionResult> Registration(ViewModels.RegistrationRequest model)
    {
        var utente = await _dbContext.Users
            .Where(x => x.Email == model.Email)
            .FirstOrDefaultAsync();

        if(utente == null)
        {
            var nuovoUtente = new User
            {
                Id = Guid.NewGuid(),
                Role = "base",
                Username = string.Format($"{model.Name.ElementAt(0)}{model.Surname}").ToLower().ToString(),
                Name = model.Name,
                Surname = model.Surname,
                Phone = model.Phone,
                Email = model.Email,
                Password = model.Password
            };

            await _dbContext.Users.AddAsync(nuovoUtente);
            await _dbContext.SaveChangesAsync();

            var claims = new List<Claim> 
            {
                new(ClaimTypes.NameIdentifier, nuovoUtente.Id.ToString()),
                new(ClaimTypes.Role, nuovoUtente.Role.ToString()),
                new(ClaimTypes.Name, $"{nuovoUtente.Name} {nuovoUtente.Surname}"),
                new(ClaimTypes.Email, nuovoUtente.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok(nuovoUtente); 
        }

        return Conflict();
    }

    public async Task<IActionResult> Logout()
    {
        try
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Pages");
        }
        catch
        {
            return RedirectToAction("Index", "Pages");
        }
    }

    [Authorize]
    [HttpGet("/profile")]
    public IActionResult Profile()
    {
        return View();
    }

    [Authorize]
    [HttpGet("/profile/details")]
    public async Task<IActionResult> ProfileDetails()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdString, out Guid userId))
        {
            return Unauthorized(new { message = "Claim NameIdentifier non valido." });
        }
        
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if(user != null)
        {
            return View(new ViewModels.UserViewModel {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Phone = user.Phone
            });
        }
        else
        {
            return View();
        }
    }

    [Authorize]
    [HttpPut("/profile/details")]
    public async Task<IActionResult> EditProfile([FromForm] ViewModels.UserViewModel userFields)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdString, out Guid userId))
        {
            return Unauthorized(new { message = "Claim NameIdentifier non valido." });
        }
        
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if(user != null)
        {
            user.Name = userFields.Name;
            user.Surname = userFields.Surname;
            user.Email = userFields.Email;
            user.Phone = userFields.Phone;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }

    [Authorize]
    [HttpDelete("/profile")]
    public async Task<IActionResult> DeleteProfile()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdString, out Guid userId))
        {
            return Unauthorized(new { message = "Claim NameIdentifier non valido." });
        }
        
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if(user != null)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            await Logout();
        }

        return NoContent();
    }


    [Authorize]
    [HttpGet("/profile/reservations")]
    public async Task<IActionResult> ReservationsHistory()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdString, out Guid userId))
        {
            return Unauthorized(new { message = "Claim NameIdentifier non valido." });
        }

        var reservations = await _dbContext.Reservations
            .Where(x => x.UserId == userId)
            .ToListAsync();

        if(reservations.Count > 0)
        {
            return View(reservations);
        }
        else
        {
            return View();
        }
    }
}

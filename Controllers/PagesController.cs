using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RVBARBER.Data;
using RVBARBER.ViewModels;

namespace RVBARBER.Controllers;

public class PagesController(ILogger<PagesController> logger, AppDbContext dbContext) : Controller
{
    private readonly ILogger<PagesController> _logger = logger;
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<IActionResult> Index()
    {
        var openingHours = await _dbContext.OpeningHours.ToListAsync();
        var treatments = await _dbContext.Treatments.ToListAsync();

        return View(new HomePageViewModel { 
            OpeningHours = openingHours, 
            Treatments = treatments
        });
    }

    [HttpGet("gallery")]
    public IActionResult Galleria()
    {
        return View();
    }
}

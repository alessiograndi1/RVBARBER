using Microsoft.EntityFrameworkCore;
using RVBARBER.Models;

namespace RVBARBER.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<OpeningHour> OpeningHours { get; set; }
    public DbSet<Treatment> Treatments { get; set; }
    public DbSet<Closure> Closures { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var openingHours = new List<OpeningHour>
        {
            new OpeningHour
            {
                Id = 1,
                DayOfWeek = DayOfWeek.Sunday,
                IsClosed = true
            },
            new OpeningHour
            {
                Id = 2,
                DayOfWeek = DayOfWeek.Monday,
                IsClosed = true
            },
            new OpeningHour
            {
                Id = 3,
                DayOfWeek = DayOfWeek.Tuesday,
                MorningOpen = new TimeSpan(10, 0, 0),
                MorningClose = new TimeSpan(12, 0, 0),
                AfternoonOpen = new TimeSpan(14, 0, 0),
                AfternoonClose = new TimeSpan(19, 0, 0),
                IsClosed = false
            },
            new OpeningHour
            {
                Id = 4,
                DayOfWeek = DayOfWeek.Wednesday,
                MorningOpen = new TimeSpan(13, 0, 0),
                MorningClose = new TimeSpan(21, 0, 0),
                AfternoonOpen = TimeSpan.Zero,
                AfternoonClose = TimeSpan.Zero,
                IsClosed = false
            },
            new OpeningHour
            {
                Id = 5,
                DayOfWeek = DayOfWeek.Thursday,
                MorningOpen = new TimeSpan(10, 0, 0),
                MorningClose = new TimeSpan(12, 0, 0),
                AfternoonOpen = new TimeSpan(14, 0, 0),
                AfternoonClose = new TimeSpan(19, 0, 0),
                IsClosed = false
            },
            new OpeningHour
            {
                Id = 6,
                DayOfWeek = DayOfWeek.Friday,
                MorningOpen = new TimeSpan(10, 0, 0),
                MorningClose = new TimeSpan(12, 0, 0),
                AfternoonOpen = new TimeSpan(14, 0, 0),
                AfternoonClose = new TimeSpan(19, 0, 0),
                IsClosed = false
            },
            new OpeningHour
            {
                Id = 7,
                DayOfWeek = DayOfWeek.Saturday,
                MorningOpen = new TimeSpan(10, 0, 0),
                MorningClose = new TimeSpan(16, 0, 0),
                AfternoonOpen = TimeSpan.Zero,
                AfternoonClose = TimeSpan.Zero,
                IsClosed = false
            }
        };

        modelBuilder.Entity<OpeningHour>().HasData(openingHours);
    }
}
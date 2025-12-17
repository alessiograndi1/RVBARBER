namespace RVBARBER.Models;

public class User
{
    public Guid Id { get; set; }
    
    public string Role { get; set; } = null!;
    
    public string Username { get; set; } = null!;
    
    public string Email { get; set; } = null!;
    
    public string Password { get; set; } = null!;
    
    public string Name { get; set; } = null!;
    
    public string Surname { get; set; } = null!;
    
    public string Phone { get; set; } = null!;
}
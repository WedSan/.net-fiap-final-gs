namespace BeSafe.Models;

public class Contact
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    
    // Navigation property
    public User User { get; set; }
}
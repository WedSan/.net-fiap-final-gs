namespace BeSafe.Models;

public class User
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public DateTime DataNascimento { get; set; }
    
    // Navigation properties
    public Contact Contact { get; set; }
    public Address Address { get; set; }
}
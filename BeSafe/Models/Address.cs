namespace BeSafe.Models;

public class Address
{
    public int Id { get; set; }
    public string Logradouro { get; set; }
    public string Bairro { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
    
    public User User { get; set; }
}
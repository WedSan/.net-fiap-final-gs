namespace BeSafe.Models;

public class RiskArea
{
    public int Id { get; set; }
    public string Mensagem { get; set; }
    public DateTime? DataEnvio { get; set; }
    public int TipoAlerta { get; set; }
    
    // Navigation property
    public ICollection<Alert> Alerts { get; set; }
}
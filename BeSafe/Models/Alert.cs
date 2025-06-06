namespace BeSafe.Models;

public class Alert
{
    public int Id { get; set; }
    public int AreaRiscoId { get; set; }
    public string Mensagem { get; set; }
    public DateTime DataEnvio { get; set; }
    public string TipoAlerta { get; set; }
    
    // Navigation property
    public RiskArea RiskArea { get; set; }
}
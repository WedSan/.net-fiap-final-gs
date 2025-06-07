namespace BeSafe.Models;

public class AlertFeature
{
    public int AreaRiscoId { get; set; }

    public float TipoAlerta { get; set; }

    public float DiaDoEnvio { get; set; } 
    public float HoraDoEnvio { get; set; }
}
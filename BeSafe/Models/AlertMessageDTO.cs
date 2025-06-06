namespace BeSafe.Models;

using System;

public class AlertMessageDto
{
    public int Id { get; set; }
    public int AreaRiscoId { get; set; }
    public string Mensagem { get; set; }
    public DateTime DataEnvio { get; set; }
    public string TipoAlerta { get; set; }
    public string Username { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
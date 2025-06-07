using Microsoft.ML.Data;

namespace BeSafe.Services;


public class AlertAnomalyPrediction
{
    [ColumnName("PredictedLabel")]
    public bool IsNormal { get; set; }

    public float Score { get; set; }

    public int AreaRiscoId { get; set; }
}
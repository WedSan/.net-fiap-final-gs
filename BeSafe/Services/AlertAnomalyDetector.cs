using BeSafe.Models;
using Microsoft.ML;

namespace BeSafe.Services;

public class AlertAnomalyPredictor
{
    private readonly MLContext _mlContext;

    public AlertAnomalyPredictor()
    {
        _mlContext = new MLContext();
    }

    public List<AlertAnomalyPrediction> GetAnomalies(List<Alert> alerts)
    {
        var alertFeatures = alerts.Select(a => new AlertFeature
        {
            TipoAlerta = float.TryParse(a.TipoAlerta, out var tipo) ? tipo : 0f,
            DiaDoEnvio = a.DataEnvio.Day,
            HoraDoEnvio = a.DataEnvio.Hour + a.DataEnvio.Minute / 60f,
            AreaRiscoId = a.AreaRiscoId
        }).ToList();
        
        var dataView = _mlContext.Data.LoadFromEnumerable(alertFeatures);
        
        var pipeline = _mlContext.Transforms
            .Concatenate("Features", nameof(AlertFeature.TipoAlerta), nameof(AlertFeature.DiaDoEnvio), nameof(AlertFeature.HoraDoEnvio))
            .Append(_mlContext.AnomalyDetection.Trainers.RandomizedPca("Features", rank: 2));

        var model = pipeline.Fit(dataView);

        var transformedData = model.Transform(dataView);

        var scoredData = _mlContext.Data.CreateEnumerable<AlertAnomalyPrediction>(transformedData, reuseRowObject: false).ToList();
        
        for (int i = 0; i < scoredData.Count; i++)
        {
            scoredData[i].AreaRiscoId = alertFeatures[i].AreaRiscoId;
        }

        return scoredData;
    }
}
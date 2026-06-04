using Microsoft.ML.Data;

namespace WarehouseMonitor.Infrastructure.ML;

public class SalesPrediction
{
    [ColumnName("ForecastedQuantities")]
    public float[] ForecastedQuantities { get; set; } = Array.Empty<float>();
    
    [ColumnName("LowerBound")]
    public float[] LowerBound { get; set; } = Array.Empty<float>();
    
    [ColumnName("UpperBound")]
    public float[] UpperBound { get; set; } = Array.Empty<float>();
}
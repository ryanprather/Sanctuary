using Sanctuary.ChartWriter.Models;

namespace Sanctuary.ChartWriter.Services
{
    public interface IJsonDataWriterBlobWriter
    {
        Task<Uri> WriteOutlierChartToBlob(string containerName, string fileName, OutlierChartDto outlierChartDto);
    }
}

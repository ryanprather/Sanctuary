using Sanctuary.Models.Statistics;

namespace Sanctuary.ChartReader.Services
{
    public interface IChartReaderService
    {
        Task<ChartDto> RetrieveChartData(Uri chartLocation);
    }
}

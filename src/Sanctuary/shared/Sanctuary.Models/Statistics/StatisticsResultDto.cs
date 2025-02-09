namespace Sanctuary.Models.Statistics
{
    public class StatisticsResultDto
    {
        public Guid Id { get; set; }
        public StatisticsResultType StatisticsResultType { get; set; }
        public string ResultType { get { return StatisticsResultType.ToString(); } }
        public Uri? ChartBlobUri { get; set; } = default;
        public Uri? DataBlobUri { get; set; } = default;

        public StatisticsResultOptions ResultOptions { get; set; } = new StatisticsResultOptions();
    }
}

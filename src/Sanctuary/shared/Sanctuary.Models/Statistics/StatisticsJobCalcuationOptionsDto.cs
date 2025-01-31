namespace Sanctuary.Models.Statistics
{
    public class StatisticsJobCalcuationOptionsDto
    {
        public StatisticsJobType JobType { get; set; }
        public int? MovingAverageWindow {  get; set; }
        public int? OutlierDeviation  { get; set; }
    }
}

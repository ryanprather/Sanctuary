using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace Sanctuary.Models.Statistics
{
    public class StatisticsJobCalcuationOptionsDto
    {
        public StatisticsJobType JobType { get; set; }
        public string JobTypeStr { get { return JobType.ToString(); } }
        public int? MovingAverageWindow {  get; set; }
        public int? OutlierDeviation  { get; set; }
    }
}

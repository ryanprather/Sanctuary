using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace Sanctuary.Models.Statistics
{
    public class StatisticsJobDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public StatisticsJobStatus JobStatus { get; set; }
        public string JobStatusStr { get { return JobStatus.ToString(); } }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Completed { get; set; }
        public StatisticsJobOptionsDto Options { get; set; }
        public StatisticsResultDto[] StatisticsResults { get; set; }

    }

    
}

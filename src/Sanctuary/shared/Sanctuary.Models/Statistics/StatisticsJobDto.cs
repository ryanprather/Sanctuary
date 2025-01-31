namespace Sanctuary.Models.Statistics
{
    public class StatisticsJobDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public StatisticsJobStatus JobStatus { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Completed { get; set; }

        public StatisticsJobOptionsDto Options { get; set; }
    
    }

    
}

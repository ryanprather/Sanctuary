namespace Sanctuary.Models.Statistics
{
    public class StatisticsJobProcessingDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public StatisticsJobStatus JobStatus { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Completed { get; set; }
        public DataFileDto[] DataFiles { get; set; }
        public StatisticsPatientDto[] PatientIds { get; set; }
        public DataFileEndpointDto[] Endpoints { get; set; }
        public StatsJobTypeDto StatsJobType { get; set; }
    }

    public enum StatisticsJobStatus
    {
        Pending = 0,
        Processing = 1,
        Completed = 2,
        Errored = 3
    }
}

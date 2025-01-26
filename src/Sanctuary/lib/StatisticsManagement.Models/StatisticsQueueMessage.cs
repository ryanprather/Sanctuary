using Sanctuary.Models;
using Sanctuary.Models.Statistics;

namespace StatisticsManagement.Models
{
    public class StatisticsQueueMessage
    {
        public Guid Id { get; set; }
        public string Description {  get; set; }
        public DataFileDto[] DataFiles { get; set; }
        public StatisticsPatientDto[] Patients { get; set; }
        public DataFileEndpointDto[] Endpoints { get; set; }
        public StatsJobTypeDto StatsJobType { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanctuary.Statistics.Repository.Datasets
{
    [Table(TableNames.StatisticsJob, Schema = SqlSchemas.SchemaName)]
    public class StatisticsJob
    {
        [Key]
        public Guid Id { get; set; }
        public int StatisticsJobTypeId { get; set; }
        [ForeignKey(nameof(StatisticsJobTypeId))]
        public StatisticsJobType StatisticsJobType { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Started { get; set; }
        public DateTimeOffset? Completed { get; set; }
        public ICollection<StatisticsJobPatient> StatisticsJobPatients { get; set; }
        public ICollection<StatisticsJobEndpoint> StatisticsJobEndpoints { get; set; }
        public ICollection<StatisticsJobDataFile> StatisticsJobDataFiles { get; set; }
    }
}

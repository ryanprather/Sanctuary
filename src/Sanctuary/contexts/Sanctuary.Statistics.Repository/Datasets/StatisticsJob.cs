using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanctuary.Statistics.Repository.Datasets
{
    [Table(TableNames.StatisticsJob, Schema = SqlSchemas.SchemaName)]
    public class StatisticsJob
    {
        [Key]
        public Guid Id { get; set; }

        public Guid StatisticsJobTypeId { get; set; }
        [ForeignKey(nameof(StatisticsJobTypeId))]
        public StatisticsJobType StatisticsJobType { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Started { get; set; }
        public DateTimeOffset Completed { get; set; }
    }
}

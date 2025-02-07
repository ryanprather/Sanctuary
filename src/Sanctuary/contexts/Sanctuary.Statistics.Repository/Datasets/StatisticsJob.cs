using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanctuary.Statistics.Repository.Datasets
{
    [Table(TableNames.StatisticsJob, Schema = SqlSchemas.SchemaName)]
    public class StatisticsJob
    {
        [Key]
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Started { get; set; }
        public DateTimeOffset? Completed { get; set; }
        public string Status { get; set; }
        public string StatisticsJobDetailsJson { get; set; }
        public virtual ICollection<StatisticalResult> StatisticalResults { get; set; } = new List<StatisticalResult>();
    }
}

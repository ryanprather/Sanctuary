using System.ComponentModel.DataAnnotations.Schema;

namespace Sanctuary.Statistics.Repository.Datasets
{
    [Table(TableNames.StatisticsJobEndpoints, Schema = SqlSchemas.SchemaName)]
    public class StatisticsJobEndpoint
    {
        public Guid Id { get; set; }
        public Guid StatisticsJobId { get; set; }
        [ForeignKey(nameof(StatisticsJobId))]
        public StatisticsJob StatisticsJob { get; set; }
        public string EndpointMapJson {  get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace Sanctuary.Statistics.Repository.Datasets
{
    [Table(TableNames.StatisticsJobDataFiles, Schema = SqlSchemas.SchemaName)]
    public class StatisticsJobDataFile
    {
        public Guid Id { get; set; }
        public Guid StatisticsJobId { get; set; }
        [ForeignKey(nameof(StatisticsJobId))]
        public StatisticsJob StatisticsJob { get; set; }
        public string BlobUrl {  get; set; }
        public string DataFileMapJson {  get; set; }
    }
}

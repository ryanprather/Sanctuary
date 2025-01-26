using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanctuary.Statistics.Repository.Datasets
{
    [Table(TableNames.StatisticsJobTypes, Schema = SqlSchemas.SchemaName)]
    public class StatisticsJobType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

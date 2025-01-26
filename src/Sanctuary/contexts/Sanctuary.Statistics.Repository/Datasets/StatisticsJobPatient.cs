using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanctuary.Statistics.Repository.Datasets
{
    [Table(TableNames.StatisticsJobPatients, Schema = SqlSchemas.SchemaName)]
    public class StatisticsJobPatient
    {
        [Key]
        public Guid Id { get; set; }
        public Guid StatisticsJobId { get; set; }
        [ForeignKey(nameof(StatisticsJobId))]
        public StatisticsJob StatisticsJob { get; set; }
        public string PatientIdentifer { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanctuary.Statistics.Repository.Datasets
{
    [Table(TableNames.StatisticsJobType, Schema = SqlSchemas.SchemaName)]
    public class StatisticsJobType
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}

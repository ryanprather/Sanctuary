﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanctuary.Statistics.Repository.Datasets
{
    [Table(TableNames.StatisticalAnalysis, Schema = SqlSchemas.SchemaName)]
    public class StatisticalAnalysis
    {
        public Guid Id { get; set; }
        public Guid StatisticsJobId { get; set; }
        
        [ForeignKey(nameof(StatisticsJobId))]
        public StatisticsJob StatisticsJob { get; set; }

        public string GraphData { get; set; } = "{}"; 
    }
}

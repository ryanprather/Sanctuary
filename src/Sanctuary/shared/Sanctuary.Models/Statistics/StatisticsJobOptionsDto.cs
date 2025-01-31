using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanctuary.Models.Statistics
{
    public class StatisticsJobOptionsDto
    {
        public DataFileDto[] DataFiles { get; set; }
        public StatisticsPatientDto[] Patients { get; set; }
        public DataFileEndpointDto[] Endpoints { get; set; }
        public StatisticsJobCalcuationOptionsDto StatsJobType { get; set; }
    }
}

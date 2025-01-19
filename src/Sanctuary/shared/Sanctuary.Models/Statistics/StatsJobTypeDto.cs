using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanctuary.Models.Statistics
{
    public class StatsJobTypeDto
    {
        public StatisticsJobType JobType { get; set; }
        public int? MovingAverageWindow {  get; set; }
        public int? OutlierDevation { get; set; }
    }
}

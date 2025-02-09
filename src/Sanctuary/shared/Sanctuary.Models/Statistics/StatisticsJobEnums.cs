using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanctuary.Models.Statistics
{
    public enum StatisticsJobType
    {
        OutlierAnalysis = 1,
        MovingAverage = 2
    }
    public enum StatisticsJobStatus
    {
        Pending = 0,
        Processing = 1,
        Completed = 2,
        Errored = 3
    }

    public enum StatisticsResultType 
    {
        Patient = 0,
    }
}

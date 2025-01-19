using Sanctuary.StatisticsCalculation.OutlierAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sanctuary.StatisticsCalculation.OutlierAnalysis.Models
{
    public class OutlierAnalysisInputContainer
    {
        public IEnumerable<OutlierInputDataEndpoint> Dataset { get; set; }
        public double StardardDevationMeasure { get; set; }
    }
}

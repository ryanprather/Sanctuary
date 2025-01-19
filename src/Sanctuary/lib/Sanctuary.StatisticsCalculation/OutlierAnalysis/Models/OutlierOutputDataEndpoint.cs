using System;

namespace Sanctuary.StatisticsCalculation.OutlierAnalysis.Models
{
    public class OutlierOutputDataEndpoint
    {
        public double Value { get; set; }
        public decimal ZValue { get; set; }
        public DateTime Timestamp { get; set; }
        public string EndpointName { get; set; }
        public bool IsOutlier { get; set; }
      
    }
}

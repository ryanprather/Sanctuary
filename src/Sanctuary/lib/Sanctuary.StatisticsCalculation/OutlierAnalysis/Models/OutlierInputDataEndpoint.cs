using System;
using System.Collections.Generic;
using System.Text;

namespace Sanctuary.StatisticsCalculation.OutlierAnalysis.Models
{
    public class OutlierInputDataEndpoint
    {
        public readonly double Value;
        public readonly DateTime Timestamp;
        public readonly string Key;
        public OutlierInputDataEndpoint(double value, DateTime timestamp, string key) 
        {
            Value = value; 
            Timestamp = timestamp;
            Key = key;
        }

    }
}

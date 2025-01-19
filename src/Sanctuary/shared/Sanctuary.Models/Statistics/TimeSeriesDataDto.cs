﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanctuary.Models.Statistics
{
    public class TimeSeriesDataDto
    {
        public TimeSeriesDataDto(DateTime timestamp, string key)
        {
            Timestamp = timestamp;
            Key = key;
            DoubleEndpoints = new Dictionary<string, double?>();
            IntEndpoints = new Dictionary<string, int?>();
            BoolEndpoints = new Dictionary<string, bool?>();
            FloatEndpoints = new Dictionary<string, float?>();
        }

        public DateTime Timestamp { get; set; }
        public string Key { get; set; }
        public Dictionary<string, double?> DoubleEndpoints { get; set; }
        public Dictionary<string, int?> IntEndpoints { get; set; }
        public Dictionary<string, bool?> BoolEndpoints { get; set; }
        public Dictionary<string, float?> FloatEndpoints { get; set; }
    }
}

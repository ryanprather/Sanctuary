using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatisticsLib.MovingAverage
{
    public class MovingAverageService : IMovingAverageService
    {
        
        public MovingAverageService() { }

        public IEnumerable<double> CalculateMovingAverage(IEnumerable<int> values, int windowSize) 
        {
            return Statistics.MovingAverage(values.Select(x => (double)x), windowSize);
        }

        public IEnumerable<double> CalculateMovingAverage(IEnumerable<double> values, int windowSize)
        {
            return Statistics.MovingAverage(values.Select(x => (double)x), windowSize);
        }

        public IEnumerable<double> CalculateMovingAverage(IEnumerable<Decimal> values, int windowSize)
        {
            return Statistics.MovingAverage(values.Select(x => (double)x), windowSize);
        }

        public IEnumerable<double> CalculateMovingAverage(IEnumerable<float> values, int windowSize)
        {
            return Statistics.MovingAverage(values.Select(x => (double)x), windowSize);
        }
    }
}

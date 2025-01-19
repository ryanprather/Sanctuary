using System;
using System.Collections.Generic;
using System.Text;

namespace StatisticsLib.MovingAverage
{
    public interface IMovingAverageService
    {
        IEnumerable<double> CalculateMovingAverage(IEnumerable<float> values, int windowSize);
        IEnumerable<double> CalculateMovingAverage(IEnumerable<int> values, int windowSize);
        IEnumerable<double> CalculateMovingAverage(IEnumerable<double> values, int windowSize);
    }
}

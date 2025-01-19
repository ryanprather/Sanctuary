using FluentResults;
using Sanctuary.StatisticsCalculation.OutlierAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sanctuary.StatisticsCalculation.OutlierAnalysis
{
    public interface IOutlierAnalysisService
    {
        Result<IEnumerable<OutlierOutputDataEndpoint>> GetStatisticalOutliers(OutlierAnalysisInputContainer dataContainer);
    }
}

using FluentResults;
using MathNet.Numerics.Statistics;
using Sanctuary.StatisticsCalculation.OutlierAnalysis.Models;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("StatisticsLib.Test")]
namespace Sanctuary.StatisticsCalculation.OutlierAnalysis
{
    public class OutlierAnalysisService : IOutlierAnalysisService
    {

        public Result<IEnumerable<OutlierOutputDataEndpoint>> GetStatisticalOutliers(OutlierAnalysisInputContainer dataContainer)
        {
            if (dataContainer.StardardDevationMeasure <= 0)
                return Result.Fail(ErrorMessages.InvalidStardardDevationMeasure);
            if (dataContainer.Dataset is null || !dataContainer.Dataset.Any())
                return Result.Fail(ErrorMessages.EmptyDataset);

            try
            {
                var results = CalculateStatisticalOutLiers(dataContainer.Dataset, dataContainer.StardardDevationMeasure);
                return Result.Ok(results.Select(x => x.Value));
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }


        internal IEnumerable<Result<OutlierOutputDataEndpoint>> CalculateStatisticalOutLiers(IEnumerable<OutlierInputDataEndpoint> dataset, double standardDeviationMeasure)
        {
            var endpointValues = dataset.Select(x => x.Value);
            double avg = endpointValues.Average();
            double mean = endpointValues.Mean();
            double standardDeviation = Math.Sqrt(endpointValues.Average(v => Math.Pow(v - avg, 2)));
            foreach (var endpoint in dataset)
            {
                var value = endpoint.Value;
                var zValue = Math.Round((decimal)((value - mean) / standardDeviation), 4);
                var returnResult = new OutlierOutputDataEndpoint()
                {
                    Value = value,
                    Timestamp = endpoint.Timestamp,
                    EndpointName = endpoint.Key,
                    ZValue = zValue,
                };
                if (Math.Abs(value - avg) > standardDeviationMeasure * standardDeviation)
                {
                    returnResult.IsOutlier = true;
                    yield return Result.Ok(returnResult);
                }
                else
                {
                    returnResult.IsOutlier = false;
                    yield return Result.Ok(returnResult);
                }
            }

            yield break;
        }

        internal class ErrorMessages
        {
            public static readonly string InvalidStardardDevationMeasure = "Standard Devation must be greater than 0";
            public static readonly string EmptyDataset = "Dataset must not be empty";
        }
    }
}

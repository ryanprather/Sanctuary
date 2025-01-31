using FluentResults;
using Sanctuary.ChartWriter.Models;
using Sanctuary.ChartWriter.Services;
using Sanctuary.CsvDataReader.Extensions;
using Sanctuary.CsvDataReader.Service;
using Sanctuary.Models;
using Sanctuary.Models.Statistics;
using Sanctuary.StatisticsCalculation.OutlierAnalysis;
using Sanctuary.StatisticsCalculation.OutlierAnalysis.Models;
using ServiceRemoting;
using StatisticsLib.MovingAverage;
using StatisticsPatientWorker.Models;
using System.Data;

namespace StatisticsPatientWorker.Logic
{
    public class StatisticsPatientLogic: IStatisticsPatientLogic
    {
        private readonly IMovingAverageService _movingAverageService;
        private readonly IOutlierAnalysisService _outlierAnalysisService;
        private readonly ICsvDataBlobReader _csvDataBlobReader;

        public StatisticsPatientLogic(
                IMovingAverageService movingAverageService,
                IOutlierAnalysisService outlierAnalysisService,
                ICsvDataBlobReader csvDataBlobReader)
        {
            _csvDataBlobReader = csvDataBlobReader;
            _movingAverageService = movingAverageService;
            _outlierAnalysisService = outlierAnalysisService;
        }

        public async Task<IEnumerable<PatientBlobProcessingResult>> ProcessRequest(StatisticsPatientDto patientDto, DataFileDto[] dataFiles, DataFileEndpointDto[] dataFileEndpoints, StatisticsJobCalcuationOptionsDto statsJobType)
        {
            var results = new List<PatientBlobProcessingResult>();
            try
            {
                
                var fileData = _csvDataBlobReader.RetrieveCsvData(dataFiles.First(), dataFileEndpoints);
                var dataset = fileData
                    .Where(x => x.Value.Key == patientDto.Identifier && x.IsSuccess)
                    .Select(x => x.Value);

                switch (statsJobType.JobType)
                {
                    case StatisticsJobType.OutlierAnalysis:
                        results.AddRange(await CalculateOutlierResults(patientDto, statsJobType.OutlierDeviation.GetValueOrDefault(), dataset, dataFileEndpoints));
                    break;

                    case StatisticsJobType.MovingAverage:
                        // TODO
                    break;
                }



            }
            catch (Exception ex)
            {
                var shit = ex.Message;
            }
            return results;
        }

        private async Task<IEnumerable<PatientBlobProcessingResult>> CalculateOutlierResults(StatisticsPatientDto patientDto, int outlierDevation, IEnumerable<TimeSeriesDataDto> dataset, DataFileEndpointDto[] dataFileEndpoints) 
        {
            var uriList = new List<PatientBlobProcessingResult>();
            var datasetContainer = new OutlierAnalysisInputContainer() { StardardDevationMeasure = (double)outlierDevation };
            var outlierResults = new List<Result<IEnumerable<OutlierOutputDataEndpoint>>>();
            foreach (var dataEndpoint in dataFileEndpoints)
            {
                var endpoint = dataEndpoint.ToEndpoint();
                if (endpoint.DataType == typeof(double))
                {
                    datasetContainer.Dataset = dataset.Select(x => new
                    {
                        x.Timestamp,
                        Endpoint = x.DoubleEndpoints.FirstOrDefault(y => y.Key == endpoint.Name)
                    }).Select(x => new OutlierInputDataEndpoint(x.Endpoint.Value.GetValueOrDefault(), x.Timestamp, x.Endpoint.Key));
                }
                else if (endpoint.DataType == typeof(float))
                {
                    datasetContainer.Dataset = dataset.Select(x => new
                    {
                        x.Timestamp,
                        Endpoint = x.FloatEndpoints.FirstOrDefault(y => y.Key == endpoint.Name)
                    }).Select(x => new OutlierInputDataEndpoint(x.Endpoint.Value.GetValueOrDefault(), x.Timestamp, x.Endpoint.Key));
                }
                else if (endpoint.DataType == typeof(int))
                {
                    datasetContainer.Dataset = dataset.Select(x => new
                    {
                        x.Timestamp,
                        Endpoint = x.IntEndpoints.FirstOrDefault(y => y.Key == endpoint.Name)
                    }).Select(x => new OutlierInputDataEndpoint(x.Endpoint.Value.GetValueOrDefault(), x.Timestamp, x.Endpoint.Key))
                    .ToList();
                    
                }
                
                var results = _outlierAnalysisService.GetStatisticalOutliers(datasetContainer);
                var chart = new OutlierChartDto()
                {
                    OutlierDevation = (int)datasetContainer.StardardDevationMeasure,
                    PatientId = patientDto.Identifier,
                    ChartType = "line",
                    Labels = results.Value.Select(x => x.Timestamp.ToShortDateString()).ToList(),
                    DatasetLabel = endpoint.Name,
                    Data = results.Value.Select(x => x.ZValue).ToList(),
                };
                var chartService = new JsonDataWriterBlobWriter();
                uriList.Add(new PatientBlobProcessingResult() 
                { 
                  ChartBlobUri = await chartService.WriteOutlierChartToBlob("trial666-statistics", $"{patientDto.Id}{endpoint.Name}_OutlierChart.json", chart) 
                });
            }
            return uriList;
        }
       
    }
}

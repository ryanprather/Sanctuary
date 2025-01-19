using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using StatisticsPatientWorker.Interfaces;
using Sanctuary.Models.Statistics;
using Sanctuary.Models;
using Sanctuary.CsvDataReader.Service;
using ServiceRemoting;
using StatisticsLib.MovingAverage;
using Sanctuary.StatisticsCalculation.OutlierAnalysis;
using Sanctuary.CsvDataReader.Extensions;
using Sanctuary.StatisticsCalculation.OutlierAnalysis.Models;
using System.Data;
using FluentResults;
using Sanctuary.ChartWriter.Models;
using Sanctuary.ChartWriter.Services;

namespace StatisticsPatientWorker
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class StatisticsPatientWorker : Actor, IStatisticsPatientWorker
    {

        private readonly IServiceRemotingFactory _serviceRemotingFactory;
        private readonly IMovingAverageService _movingAverageService;
        private readonly IOutlierAnalysisService _outlierAnalysisService;
        private readonly ICsvDataBlobReader _csvDataBlobReader;
        /// <summary>
        /// Initializes a new instance of StatisticsPatientWorker
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public StatisticsPatientWorker(
            ActorService actorService, 
            ActorId actorId,
            IServiceRemotingFactory serviceRemotingFactory,
            IMovingAverageService movingAverageService,
            IOutlierAnalysisService outlierAnalysisService,
            ICsvDataBlobReader csvDataBlobReader) 
            : base(actorService, actorId)
        {
            _serviceRemotingFactory = serviceRemotingFactory;
            _movingAverageService = movingAverageService;
            _outlierAnalysisService = outlierAnalysisService;
            _csvDataBlobReader = csvDataBlobReader;
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");
            return Task.CompletedTask;
        }

        public async Task ProcessPatientJob(StatisticsPatientDto patientDto, DataFileDto[] dataFiles, DataFileEndpointDto[] dataFileEndpoints, StatsJobTypeDto statsJobType)
        {
            try 
            {
                var fileData = _csvDataBlobReader.RetrieveCsvData(dataFiles.First(), dataFileEndpoints);
                var dataset = fileData
                    .Where(x => x.Value.Key == patientDto.Identifier && x.IsSuccess)
                    .Select(x=>x.Value);
                
                switch (statsJobType.JobType)
                {
                    case StatisticsJobType.OutlierAnalysis:
                        var datasetContainer = new OutlierAnalysisInputContainer() { StardardDevationMeasure = (double)statsJobType.OutlierDevation.Value };
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
                                var results  = _outlierAnalysisService.GetStatisticalOutliers(datasetContainer);
                                
                            }
                            else if (endpoint.DataType == typeof(float))
                            {
                                datasetContainer.Dataset = dataset.Select(x => new
                                {
                                    x.Timestamp,
                                    Endpoint = x.FloatEndpoints.FirstOrDefault(y => y.Key == endpoint.Name)
                                }).Select(x => new OutlierInputDataEndpoint(x.Endpoint.Value.GetValueOrDefault(), x.Timestamp, x.Endpoint.Key));
                                var results = _outlierAnalysisService.GetStatisticalOutliers(datasetContainer);
                                var test = results.Value.ToList();
                            }
                            else if (endpoint.DataType == typeof(int))
                            {
                                datasetContainer.Dataset = dataset.Select(x => new
                                {
                                    x.Timestamp,
                                    Endpoint = x.IntEndpoints.FirstOrDefault(y => y.Key == endpoint.Name)
                                }).Select(x => new OutlierInputDataEndpoint(x.Endpoint.Value.GetValueOrDefault(), x.Timestamp, x.Endpoint.Key))
                                .ToList();
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
                                await chartService.WriteOutlierChartToBlob("trial666-statistics", $"{patientDto.Id}{endpoint.Name}_OutlierChart.json", chart);
                            }
                        }
                        break;

                    case StatisticsJobType.MovingAverage:
                        
                        break;
                }

                

            }
            catch (Exception ex) 
            {
                var shit = ex.Message;
            }
            
        }
            
    }
}

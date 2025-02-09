using Microsoft.AspNetCore.Mvc;
using Sanctuary.ChartReader.Services;
using Sanctuary.Models;
using Sanctuary.Models.Statistics;
using ServiceRemoting;
using StatisticsManagement.Interfaces;
using StatisticsRepository.Interfaces;

namespace Sanctuary.Web.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ILogger<StatisticsController> _logger;
        private readonly IServiceRemotingFactory _serviceRemotingFactory;
        private readonly IChartReaderService _chartReaderService;
        public StatisticsController(
            ILogger<StatisticsController> logger,
            IServiceRemotingFactory serviceRemotingFactory,
            IChartReaderService chartReaderService) 
        {
            _logger = logger;
            _serviceRemotingFactory = serviceRemotingFactory;
            _chartReaderService = chartReaderService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateStatisticsJob()
        {
            var timeStampColumn = new DataFileEndpointDto()
            {
                Name = "DateTime",
                DataType = "System.DateTime",
            };

            var keyColumn = new DataFileEndpointDto()
            {
                Name = "PatientId",
                DataType = "System.String",
            };

            var dataEndpoints = new List<DataFileEndpointDto>()
            {
                new DataFileEndpointDto() { Name = "Steps",DataType = "System.Int32" },
                new DataFileEndpointDto() { Name = "Kcals",DataType = "System.Double" },
                new DataFileEndpointDto() { Name = "HeartRate",DataType = "System.Int32" },
                new DataFileEndpointDto() { Name = "SleepMinutes",DataType = "System.Int32" },
                new DataFileEndpointDto() { Name = "Mvpa",DataType = "System.Int32" },
                new DataFileEndpointDto() { Name = "WearMinutes",DataType = "System.Int32" },
                new DataFileEndpointDto() { Name = "Temperture",DataType = "System.Int32" },
            };

            var options = new StatisticsJobOptionsDto()
            {
                StatsJobType = new StatisticsJobCalcuationOptionsDto() 
                { 
                    JobType= StatisticsJobType.OutlierAnalysis,
                    OutlierDeviation = 2
                },
                DataFiles = new List<DataFileDto>()
                {
                    new DataFileDto()
                    {
                        BlobUrl = "http://127.0.0.1:10000/devstoreaccount1/trial666/dailystats666.csv",
                        FileMap = new DataFileMapDefinitionDto(timeStampColumn, keyColumn, dataEndpoints )
                    }
                }.ToArray(),
                Endpoints = new List<DataFileEndpointDto>() 
                {
                    new DataFileEndpointDto() { Name = "Steps",DataType = "System.Int32" }
                }.ToArray(),
                Patients = new List<StatisticsPatientDto>() { new StatisticsPatientDto() {Id = Guid.NewGuid(), Identifier = "MalUOmqx" } }.ToArray(),
            };

            var reppoService = await _serviceRemotingFactory.GetStatelessServiceAsync<IStatisticsRepository>();
            var job = await reppoService.CreateStatisticsJobAsync("Test Execution", options);
            
            var statsService = await _serviceRemotingFactory.GetStatefulServiceAsync<IStatisticsManagement>();
            await statsService.EnqueueStatisticsJob(job);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetPreviousJobs() 
        {
            var statsService = await _serviceRemotingFactory.GetStatelessServiceAsync<IStatisticsRepository>();
            var results = await statsService.GetPreviousJobs();
            return Ok(results);
        }

        [HttpGet]
        public async Task<IActionResult> Results(Guid id)
        {
            var statsService = await _serviceRemotingFactory.GetStatelessServiceAsync<IStatisticsRepository>();
            var results = await statsService.GetJobById(id);
            return View(results);
        }

        [HttpGet]
        public async Task<IActionResult> GetChartById(Guid id) 
        {
            var statsService = await _serviceRemotingFactory.GetStatelessServiceAsync<IStatisticsRepository>();
            var results = await statsService.GetResultsById(id);
            var chartData = await _chartReaderService.RetrieveChartData(results.ChartBlobUri);

            return Ok(chartData);
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Sanctuary.Models;
using Sanctuary.Models.Statistics;
using ServiceRemoting;
using StatisticsManagement.Interfaces;
using StatisticsManagement.Models;
using StatisticsRepository.Interfaces;

namespace Sanctuary.Web.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ILogger<StatisticsController> _logger;
        private readonly IServiceRemotingFactory _serviceRemotingFactory;
        public StatisticsController(
            ILogger<StatisticsController> logger,
            IServiceRemotingFactory serviceRemotingFactory
            ) 
        {
            _logger = logger;
            _serviceRemotingFactory = serviceRemotingFactory;
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

            var queueMessage = new StatisticsQueueMessage()
            {
                StatsJobType = new StatsJobTypeDto() 
                { 
                    JobType= StatisticsJobType.OutlierAnalysis,
                    OutlierDevation = 2
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
                Description = "Test Execution",
                Patients = new List<StatisticsPatientDto>() { new StatisticsPatientDto() {Id = Guid.NewGuid(), Identifier = "MalUOmqx" } }.ToArray(),
            };

            var reppoService = await _serviceRemotingFactory.GetStatelessServiceAsync<IStatisticsRepository>();
            var job = await reppoService.CreateStatisticsJobAsync( queueMessage );
            
            var statsService = await _serviceRemotingFactory.GetStatefulServiceAsync<IStatisticsManagement>();
            await statsService.EnqueueStatisticsJob(job);

            return Ok();
        }
    }
}

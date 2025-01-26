using Sanctuary.Models.Statistics;
using Sanctuary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanctuary.Statistics.Repository.Datasets;

namespace Sanctuary.Statistics.Repository.Repository
{
    public interface IStatisticsDataRepository
    {
        Task<StatisticsJob> AddStatisticsJob(
            string description,
            DataFileDto[] dataFiles,
            DataFileEndpointDto[] endpoints,
            StatisticsPatientDto[] patients,
            StatsJobTypeDto jobType);

        Task UpdateStatisticsJobStartDate(Guid jobId);

        Task UpdateStatisticsJobCompletedDate(Guid jobId);
    }
}

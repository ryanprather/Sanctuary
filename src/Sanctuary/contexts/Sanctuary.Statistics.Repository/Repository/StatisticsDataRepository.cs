using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sanctuary.Models;
using Sanctuary.Models.Statistics;
using Sanctuary.Statistics.Repository.Context;
using Sanctuary.Statistics.Repository.Datasets;

namespace Sanctuary.Statistics.Repository.Repository
{
    public class StatisticsDataRepository: IStatisticsDataRepository
    {
        private readonly StatisticsContext _context;
        public StatisticsDataRepository(StatisticsContext statisticsContext) 
        {
            _context = statisticsContext;
        }

        public async Task<StatisticsJob> AddStatisticsJob(
            string description, 
            DataFileDto[] dataFiles, 
            DataFileEndpointDto[] endpoints, 
            StatisticsPatientDto[] patients, 
            StatsJobTypeDto jobType)
        {
            var statsJob = new StatisticsJob() 
            {
                Description = description,
                Created = DateTime.UtcNow,
                StatisticsJobTypeId = (int)jobType.JobType
            };
            var jobPatients = patients.Select(x => new StatisticsJobPatient()
            {
                PatientIdentifer = x.Identifier,
                StatisticsJob = statsJob
            });
            var jobEndpoints = endpoints.Select(x => new StatisticsJobEndpoint()
            {
                StatisticsJob = statsJob,
                EndpointMapJson = JsonConvert.SerializeObject(x),
            });
            var jobFiles = dataFiles.Select(x => new StatisticsJobDataFile()
            {
                StatisticsJob = statsJob,
                BlobUrl = x.BlobUrl,
                DataFileMapJson = JsonConvert.SerializeObject(x.FileMap)
            });

            using var transaction = _context.Database.BeginTransaction();
            try 
            {
                await _context.StatisticsJobs.AddAsync(statsJob);
                await _context.StatisticsJobEndpoints.AddRangeAsync(jobEndpoints);
                await _context.StatisticsJobDataFiles.AddRangeAsync(jobFiles);
                await _context.StatisticsJobPatients.AddRangeAsync(jobPatients);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex) 
            {
                transaction.Rollback();
            }

            return statsJob;
        }

        public async Task UpdateStatisticsJobStartDate(Guid jobId) 
        {
            try 
            {
                var statsJob = await _context.StatisticsJobs.FirstOrDefaultAsync(x => x.Id == jobId);
                if (statsJob == null)
                    return;
                statsJob.Started = DateTimeOffset.UtcNow;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var ass = ex.Message;
            }
        }

        public async Task UpdateStatisticsJobCompletedDate(Guid jobId)
        {
            try
            {
                var statsJob = await _context.StatisticsJobs.FirstOrDefaultAsync(x => x.Id == jobId);
                if (statsJob == null)
                    return;
                statsJob.Completed = DateTimeOffset.UtcNow;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var ass = ex.Message;
            }
        }
    }
}

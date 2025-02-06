using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sanctuary.Models.Statistics;
using Sanctuary.Statistics.Repository.Context;
using Sanctuary.Statistics.Repository.Datasets;

namespace Sanctuary.Statistics.Repository.Repository
{
    public class StatisticsDataRepository : IStatisticsDataRepository
    {
        private readonly StatisticsContext _context;
        public StatisticsDataRepository(StatisticsContext statisticsContext)
        {
            _context = statisticsContext;
        }

        public async Task<StatisticsJob> AddStatisticsJob(string description, StatisticsJobOptionsDto options)
        {
            var statsJob = new StatisticsJob()
            {
                Description = description,
                Created = DateTime.UtcNow,
                Status = StatisticsJobStatus.Pending.ToString(),
                StatisticsJobDetailsJson = JsonConvert.SerializeObject(options)
            };

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _context.StatisticsJobs.AddAsync(statsJob);
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
                statsJob.Status = StatisticsJobStatus.Processing.ToString();
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
                statsJob.Status = StatisticsJobStatus.Completed.ToString();
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var ass = ex.Message;
            }
        }

        public async Task AddStatisticsJobResults(Guid jobId, StatisticsResultDto[] statisticsResultDto)
        {
            try
            {
                var statsJob = await _context.StatisticsJobs.FirstOrDefaultAsync(x => x.Id == jobId);
                if (statsJob == null)
                    return;
                var results = statisticsResultDto.Select(x => new StatisticalResult()
                {
                    StatisticsJobId = jobId,
                    ChartDataUri = (x.ChartBlobUri != null) ? x.ChartBlobUri.ToString() : null,
                    CsvDataUri = (x.DataBlobUri != null) ? x.DataBlobUri.ToString() : null,
                });
                _context.StatisticalResults.AddRange(results);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var ass = ex.Message;
            }
        }

        public async Task<IList<StatisticsJob>> GetPreviousJobs() 
        {
            return await _context.StatisticsJobs
                .Where(x=>x.Status == StatisticsJobStatus.Completed.ToString())
                .ToListAsync();
        }

        public async Task<StatisticsJob> GetJobById(Guid Id)
        {
            return await _context.StatisticsJobs
                .FirstOrDefaultAsync(x => x.Id == Id);
        }
    }
}

using FluentResults;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sanctuary.Models.Statistics;
using Sanctuary.Statistics.Repository.Context;
using Sanctuary.Statistics.Repository.Datasets;
using Sanctuary.Statistics.Repository.Models;
using System.Reflection.Metadata;

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
                    ResultOptionsJson = JsonConvert.SerializeObject(x.ResultOptions),
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

        public async Task<Result<StatisticsJob>> QueryJobAsync(QueryJobOptions options)
        {
            try
            {
                var query = _context.StatisticsJobs.AsNoTracking().AsQueryable();
                if (options.IncludeResults.HasValue && options.IncludeResults.Value)
                {
                    query = query.Include(x => x.StatisticalResults
                    .Where(result => result.StatisticsJobId == options.JobId));
                }

                var queryResult = await query.FirstOrDefaultAsync(x => x.Id == options.JobId);
                
                if(queryResult is not null)
                    return Result.Ok(queryResult);
                else
                    return Result.Fail("job not found");
            }
            catch (Exception ex) 
            {
                return Result.Fail(ex.Message);
            }
        }

        public async Task<Result<StatisticalResult>> QueryResultAsync(QueryResultOptions options)
        {
            try
            {
                var query = _context.StatisticalResults.AsNoTracking().AsQueryable();
                if (options.IncludeJob.HasValue && options.IncludeJob.Value)
                {
                    query = query.Include(x => x.StatisticsJob);
                }

                var queryResult = await query.FirstOrDefaultAsync(x => x.Id == options.ResultId);

                if (queryResult is not null)
                    return Result.Ok(queryResult);
                else
                    return Result.Fail("job not found");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}

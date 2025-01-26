namespace Sanctuary.Statistics.Repository
{
    internal static class SqlSchemas
    {
        internal const string SchemaName = "Statistics";
    }

    /// <summary>
    /// Static class with SQL database schema names
    /// </summary>
    internal static class TableNames
    {
        internal const string StatisticsJob = "Jobs";
        internal const string StatisticalResults = "StatisticalResults";
        internal const string StatisticsJobTypes = "JobTypes";
        internal const string StatisticsJobPatients = "JobPatients";
        internal const string StatisticsJobDataFiles = "JobDataFiles";
        internal const string StatisticsJobEndpoints = "JobEndpoints";
    }
}

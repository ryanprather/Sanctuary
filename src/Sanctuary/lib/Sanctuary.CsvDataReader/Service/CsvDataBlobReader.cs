using Azure.Storage.Blobs;
using CsvHelper;
using CsvHelper.Configuration;
using FluentResults;
using Sanctuary.CsvDataReader.Extensions;
using Sanctuary.CsvDataReader.Models;
using Sanctuary.Models;
using Sanctuary.Models.Statistics;
using System.Globalization;
using System.Net;


namespace Sanctuary.CsvDataReader.Service
{
    public class CsvDataBlobReader: ICsvDataBlobReader
    { 
        public IEnumerable<Result<TimeSeriesDataDto>> RetrieveCsvData(DataFileDto dataFile, DataFileEndpointDto[] dataFileEndpoints) 
        {
            var fileMap = dataFile.FileMap;
            var hasHeaderRecord = dataFile.FileMap.HasHeader;
            bool hasBreakingError = false;
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = hasHeaderRecord };
            var blobClient = new BlobClient(new Uri(dataFile.BlobUrl));
            using (var stream = blobClient.OpenRead())
            using (var reader = new StreamReader(stream))
            using (var csvReader = new CsvReader(reader, config))
            {
                while (csvReader.Read())
                {
                    var dataRow = csvReader.GetRecord<dynamic>();
                    var dictionaryRow = (IDictionary<string, object>)dataRow;
                    // Timestamp column // 
                    var timestampValue = GetValueFromDataRow(dictionaryRow, hasHeaderRecord, fileMap.TimestampColumn.ToEndpoint());
                    var dateTimeComlumnResult = GetDateTimeColumnValue(timestampValue, fileMap.TimestampColumn.ToEndpoint());
                    if (!dateTimeComlumnResult.IsSuccess)
                    {
                        hasBreakingError = true;
                        yield return Result.Fail(ErrorMessages.MissingTimestampColumn);
                    }

                    // Key column validation //
                    var keyColumnValue = GetValueFromDataRow(dictionaryRow, hasHeaderRecord, fileMap.KeyColumn.ToEndpoint());
                    if (keyColumnValue is null)
                    {
                        hasBreakingError = true;
                        yield return Result.Fail(ErrorMessages.MissingKeyColumn);
                    }

                    if (hasBreakingError)
                        yield break;

                    var tsd = new TimeSeriesDataDto(dateTimeComlumnResult.Value, keyColumnValue);
                    foreach (var endpoint in dataFileEndpoints)
                    {
                        var endpointType = endpoint.ToEndpoint().DataType;
                        if (endpointType == typeof(Int32))
                        {
                            var value = GetValueFromDataRow(dictionaryRow, hasHeaderRecord, endpoint.ToEndpoint());
                            var endpointResult = GetIntColumnValue(value, endpoint.ToEndpoint());
                            if (!endpointResult.IsSuccess)
                                yield return Result.Fail(ErrorMessages.EndpointColumnParse(endpoint.Name));
                            else
                                tsd.IntEndpoints.Add(endpoint.Name, endpointResult.Value);
                        }
                        else if (endpointType == typeof(double))
                        {
                            var value = GetValueFromDataRow(dictionaryRow, hasHeaderRecord, endpoint.ToEndpoint());
                            var endpointResult = GetDoubleColumnValue(value, endpoint.ToEndpoint());
                            if (!endpointResult.IsSuccess)
                                yield return Result.Fail(ErrorMessages.EndpointColumnParse(endpoint.Name));
                            else
                                tsd.DoubleEndpoints.Add(endpoint.Name, endpointResult.Value);
                        }
                        else if (endpointType == typeof(float))
                        {
                            var value = GetValueFromDataRow(dictionaryRow, hasHeaderRecord, endpoint.ToEndpoint());
                            var endpointResult = GetFloatColumnValue(value, endpoint.ToEndpoint());
                            if (!endpointResult.IsSuccess)
                                yield return Result.Fail(ErrorMessages.EndpointColumnParse(endpoint.Name));
                            else
                                tsd.FloatEndpoints.Add(endpoint.Name, endpointResult.Value);
                        }
                        else if (endpointType == typeof(bool))
                        {
                            var value = GetValueFromDataRow(dictionaryRow, hasHeaderRecord, endpoint.ToEndpoint());
                            var endpointResult = GetBoolColumnValue(value, endpoint.ToEndpoint());
                            if (!endpointResult.IsSuccess)
                                yield return Result.Fail(ErrorMessages.EndpointColumnParse(endpoint.Name));
                            else
                                tsd.BoolEndpoints.Add(endpoint.Name, endpointResult.Value);
                        }
                    }
                    yield return Result.Ok(tsd);
                }
                yield break;
            }
        }

        internal Result<bool> GetBoolColumnValue(string value, DataFileEndpoint endpoint)
        {
            bool boolValue;
            if (String.IsNullOrWhiteSpace(value)
                || !bool.TryParse(value, out boolValue))
                return Result.Fail(ErrorMessages.EndpointColumnParse(endpoint.Name));

            return Result.Ok(boolValue);
        }

        internal Result<float> GetFloatColumnValue(string value, DataFileEndpoint endpoint)
        {
            float floatValue;
            if (String.IsNullOrWhiteSpace(value)
                || !float.TryParse(value, out floatValue))
                return Result.Fail(ErrorMessages.EndpointColumnParse(endpoint.Name));

            return Result.Ok(floatValue);
        }

        internal Result<double> GetDoubleColumnValue(string value, DataFileEndpoint endpoint)
        {
            double doubleValue;
            if (String.IsNullOrWhiteSpace(value)
                || !Double.TryParse(value, out doubleValue))
                return Result.Fail(ErrorMessages.EndpointColumnParse(endpoint.Name));

            return Result.Ok(doubleValue);
        }

        internal Result<Int32> GetIntColumnValue(string value, DataFileEndpoint endpoint)
        {
            Int32 intValue;
            if (String.IsNullOrWhiteSpace(value)
                || !Int32.TryParse(value, out intValue))
                return Result.Fail(ErrorMessages.EndpointColumnParse(endpoint.Name));

            return Result.Ok(intValue);
        }

        internal Result<DateTime> GetDateTimeColumnValue(string value, DataFileEndpoint endpoint)
        {
            DateTime dateTimeValue;
            if (String.IsNullOrWhiteSpace(value)
                || !DateTime.TryParse(value, out dateTimeValue))
                return Result.Fail(ErrorMessages.EndpointColumnParse(endpoint.Name));

            return Result.Ok(dateTimeValue);
        }

        internal string GetValueFromDataRow(IDictionary<string, object> dictionaryRow, bool hasHeaderRecord, DataFileEndpoint endpoint)
        {
            if (!hasHeaderRecord)
                return dictionaryRow.ElementAt(endpoint.Index.GetValueOrDefault()).Value as string;
            else
                return dictionaryRow.FirstOrDefault(x => x.Key == endpoint.Name).Value as string;
        }

        internal class ErrorMessages
        {
            public static readonly string NullMappingErrorMessage = "Mapping could not be loaded";
            public static readonly string MissingTimestampColumn = "Timestamp column could not be found or parsed";
            public static readonly string MissingKeyColumn = "Key column could not be found or parsed";
            public static string EndpointColumnParse(string endpointname) => $"Could not be found or the value could not be parsed {endpointname} into datatype";

        }

    }
}

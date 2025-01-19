using FluentResults;
using Sanctuary.Models.Statistics;
using Sanctuary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanctuary.CsvDataReader.Service
{
    public interface ICsvDataBlobReader
    {
        IEnumerable<Result<TimeSeriesDataDto>> RetrieveCsvData(DataFileDto dataFile, DataFileEndpointDto[] dataFileEndpoints);
    }
}

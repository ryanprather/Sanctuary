using Sanctuary.ChartWriter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanctuary.ChartWriter.Services
{
    public interface IJsonDataWriterBlobWriter
    {
        Task WriteOutlierChartToBlob(string containerName, string fileName, OutlierChartDto outlierChartDto);
    }
}

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Newtonsoft.Json;
using Sanctuary.Models.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanctuary.ChartReader.Services
{
    public class ChartReaderService: IChartReaderService
    {
        private const string connectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;";

        public ChartReaderService() 
        {
        }

        public async Task<ChartDto> RetrieveChartData(Uri chartLocation)
        {
            try
            {
                BlobContainerClient containerClient = new BlobContainerClient(chartLocation);
                BlobClient blobClient = containerClient.GetBlobClient(chartLocation.Segments.LastOrDefault());
                if (await blobClient.ExistsAsync())
                {
                    string data = "";
                    var response = await blobClient.DownloadAsync();
                    using (var streamReader = new StreamReader(response.Value.Content))
                    {
                        data = await streamReader.ReadToEndAsync();
                    }
                    return JsonConvert.DeserializeObject<ChartDto>(data);
                }
            }
            catch (Exception ex)
            {
                var ass = ex;
            }
            return null;
        }

    }
}

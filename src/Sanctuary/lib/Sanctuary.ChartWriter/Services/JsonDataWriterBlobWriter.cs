using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Newtonsoft.Json;
using Sanctuary.ChartWriter.Models;

namespace Sanctuary.ChartWriter.Services
{
    public class JsonDataWriterBlobWriter: IJsonDataWriterBlobWriter
    {
        private const string connectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;";

        public JsonDataWriterBlobWriter() { }
        
        public async Task<Uri> WriteOutlierChartToBlob(string containerName,string fileName, OutlierChartDto outlierChartDto)
        {
            try
            {
                BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
                container.CreateIfNotExists(PublicAccessType.BlobContainer);

                //lines modified
                var blockBlob = container.GetBlobClient(fileName);

                using (var outStream = blockBlob.OpenWrite(true))
                using (TextWriter writer = new StreamWriter(outStream))
                {
                    await writer.WriteAsync(JsonConvert.SerializeObject(outlierChartDto));
                }
                
                return blockBlob.Uri;
            }
            catch (Exception ex)
            {
                var ass = ex;
            }
            return null;
        }
    }
}

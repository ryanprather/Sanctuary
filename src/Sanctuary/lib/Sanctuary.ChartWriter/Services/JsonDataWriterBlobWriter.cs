using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Sanctuary.ChartWriter.Models;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;

namespace Sanctuary.ChartWriter.Services
{
    public class JsonDataWriterBlobWriter: IJsonDataWriterBlobWriter
    {
        private const string connectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;";

        public JsonDataWriterBlobWriter() { }
        
        public async Task WriteOutlierChartToBlob(string containerName,string fileName, OutlierChartDto outlierChartDto)
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
            }
            catch (Exception ex)
            {
                var ass = ex;
            }
        }
    }
}

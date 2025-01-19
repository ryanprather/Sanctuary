using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CsvHelper;
using Sanctuary.StatisticsCalculation.OutlierAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanctuary.CsvDataWriter.Service
{
    public class CsvDataBlobWriter
    {
        private const string connectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;";

        public CsvDataBlobWriter() 
        {
        }

        public static void StreamToBlob(IEnumerable<OutlierOutputDataEndpoint> data)
        {
            try
            {
                BlobContainerClient container = new BlobContainerClient(connectionString, "trial666-statistics");
                container.CreateIfNotExists(PublicAccessType.BlobContainer);

                //lines modified
                var blockBlob = container.GetBlobClient("dailystats666.csv");

                using (var outStream = blockBlob.OpenWrite(true))
                using (TextWriter writer = new StreamWriter(outStream))
                {
                    var csv = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture);
                    csv.WriteRecords(data); // where values implements IEnumerable
                }
            }
            catch (Exception ex)
            {
                var ass = ex;
            }

        }
    }
}

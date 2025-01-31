using Sanctuary.Models;
using Sanctuary.Models.Statistics;
using StatisticsPatientWorker.Models;

namespace StatisticsPatientWorker.Logic
{
    public interface IStatisticsPatientLogic
    {
        Task<IEnumerable<PatientBlobProcessingResult>> ProcessRequest(StatisticsPatientDto patientDto, DataFileDto[] dataFiles, DataFileEndpointDto[] dataFileEndpoints, StatisticsJobCalcuationOptionsDto statsJobType);
    }
}

using AutoMapper;
using Newtonsoft.Json;
using Sanctuary.Models;
using Sanctuary.Statistics.Repository.Datasets;

namespace Sanctuary.Maps
{
    public class StatisticsJobDataFileProfile : Profile
    {
        public StatisticsJobDataFileProfile()
        {
            CreateMap<DataFileDto, StatisticsJobDataFile>();
            CreateMap<StatisticsJobDataFile, DataFileDto>();
        }
    }
    public static class StatisticsJobDataFileMapper
    {
        internal static IMapper Mapper { get; }

        static StatisticsJobDataFileMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<StatisticsJobDataFileProfile>())
                .CreateMapper();
        }

        public static StatisticsJobDataFile ToEntity(this DataFileDto model, Guid statisticsJobId)
        {
            var item = Mapper.Map<StatisticsJobDataFile>(model);
            item.StatisticsJobId = statisticsJobId;
            item.DataFileMapJson = JsonConvert.SerializeObject(model.FileMap);
            return item;
        }

        public static DataFileDto ToDto(this StatisticsJobDataFile model)
        {
            var item = Mapper.Map<DataFileDto>(model);
            item.FileMap = JsonConvert.DeserializeObject<DataFileMapDefinitionDto>(model.DataFileMapJson);
            return item;
        }
    }
}

using AutoMapper;
using Sanctuary.Models.Statistics;
using Sanctuary.Statistics.Repository.Datasets;

namespace Sanctuary.Maps
{
    public class StatisticsJobMapperProfile : Profile
    {
        public StatisticsJobMapperProfile()
        {
            CreateMap<StatisticsJob, StatisticsJobProcessingDto>();
            CreateMap<StatisticsJobProcessingDto, StatisticsJob>();
        }
    }

    public static class StatisticsJobMapper
    {
        internal static IMapper Mapper { get; }

        static StatisticsJobMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<StatisticsJobMapperProfile>())
                .CreateMapper();
        }

        public static StatisticsJob ToEntity(this StatisticsJobProcessingDto model)
        {
            var item = Mapper.Map<StatisticsJob>(model);
            item.StatisticsJobDataFiles = Mapper.Map<List<StatisticsJobDataFile>>(model.DataFiles);
            item.StatisticsJobEndpoints = Mapper.Map<List<StatisticsJobEndpoint>>(model.Endpoints);
            return item;
        }

        public static StatisticsJobProcessingDto ToDto(this StatisticsJob model)
        {
            var item = Mapper.Map<StatisticsJobProcessingDto>(model);
            return item;
        }
    }
}

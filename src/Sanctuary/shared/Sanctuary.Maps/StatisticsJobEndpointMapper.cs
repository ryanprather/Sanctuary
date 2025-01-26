using AutoMapper;
using Newtonsoft.Json;
using Sanctuary.Models;
using Sanctuary.Statistics.Repository.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanctuary.Maps
{
    public class StatisticsJobEndpointProfile : Profile
    {
        public StatisticsJobEndpointProfile()
        {
            CreateMap<DataFileDto, StatisticsJobEndpoint>();
            CreateMap<StatisticsJobEndpoint, DataFileDto>();
        }
    }
    public static class StatisticsJobEndpointMapper
    {
        internal static IMapper Mapper { get; }

        static StatisticsJobEndpointMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<StatisticsJobEndpointProfile>())
                .CreateMapper();
        }

        public static StatisticsJobEndpoint ToEntity(this DataFileEndpointDto model, Guid statisticsJobId)
        {
            // var item = Mapper.Map<StatisticsJobEndpoint>(model);
            // item.StatisticsJobId = statisticsJobId;
            // item.DataFileMapJson = JsonConvert.SerializeObject(model.FileMap);
            // return item;

            return null;
        }

        public static DataFileEndpointDto ToDto(this StatisticsJobEndpoint model)
        {
            //var item = Mapper.Map<DataFileDto>(model);
            //item.FileMap = JsonConvert.DeserializeObject<DataFileMapDefinitionDto>(model.DataFileMapJson);
            //return item;
            return null;
        }
    }
}

using Sanctuary.CsvDataReader.Models;
using Sanctuary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanctuary.CsvDataReader.Extensions
{
    public static class DataFileExtensions
    {
        // This is the extension method.
        // The first parameter takes the "this" modifier
        // and specifies the type for which the method is defined.
        public static DataFileEndpoint ToEndpoint(this DataFileEndpointDto dto)
        {
            return new DataFileEndpoint() { Name = dto.Name, Index = dto.Index, DataType = Type.GetType(dto.DataType)};
        }
    }
}

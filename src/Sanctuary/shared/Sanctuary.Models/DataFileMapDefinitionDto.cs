using System.Diagnostics.CodeAnalysis;

namespace Sanctuary.Models
{
    public class DataFileMapDefinitionDto
    {
        public DataFileMapDefinitionDto() { }

        [SetsRequiredMembers]
        public DataFileMapDefinitionDto(DataFileEndpointDto timestampColumn, DataFileEndpointDto keyColumn, List<DataFileEndpointDto> endpoints, bool hasHeader = true)
        {
            TimestampColumn = timestampColumn;
            KeyColumn = keyColumn;
            Endpoints = endpoints;
            HasHeader = hasHeader;
        }

        public bool HasHeader { get; set; }
        public required DataFileEndpointDto TimestampColumn { get; set; }
        public required DataFileEndpointDto KeyColumn { get; set; }
        public required List<DataFileEndpointDto> Endpoints { get; set; }
    }
}

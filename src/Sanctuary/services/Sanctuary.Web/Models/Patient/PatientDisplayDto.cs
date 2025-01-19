using Sanctuary.Web.Models.DataFile;
using Sanctuary.Web.Models.Shared;
using Sanctuary.Web.Models.Site;
using System.ComponentModel.DataAnnotations;

namespace Sanctuary.Web.Models.Patient
{
    public class PatientDisplayDto
    {
        public Guid Id { get; set; }

        [Required]
        public required string Identifier { get; set; }

        public TimezoneWithOffset CurrentTimezone { get; set; }

        public SiteDisplayDto CurrentSite { get; set; }

        public List<DataFileDisplayDto>? DataFiles { get; set; }

        public DateTimeOffset? DataStartDate { get; set; }

        public DateTimeOffset? DataEndDate { get; set; }
    }
}

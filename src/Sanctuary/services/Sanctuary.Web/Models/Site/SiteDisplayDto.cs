using Sanctuary.Web.Models.Shared;

namespace Sanctuary.Web.Models.Site
{
    public class SiteDisplayDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Identifer { get; set; }
        public TimezoneWithOffset Timezone { get; set; }
    }
}

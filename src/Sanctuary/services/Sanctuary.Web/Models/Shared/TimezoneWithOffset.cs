namespace Sanctuary.Web.Models.Shared
{
    public class TimezoneWithOffset
    {
        public string ZoneId { get; set; }
        public string TimezoneName { get; set; }
        public TimeSpan TimezoneOffset { get; set; }
    }
}

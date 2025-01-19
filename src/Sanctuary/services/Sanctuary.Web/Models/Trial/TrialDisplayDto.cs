namespace Sanctuary.Web.Models.Trial
{
    public class TrialDisplayDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TrialStatus Status { get; set; }

        public BlindType BlindType { get; set; }
    }
}

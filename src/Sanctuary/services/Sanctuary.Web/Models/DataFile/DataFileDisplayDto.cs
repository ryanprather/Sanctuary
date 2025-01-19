namespace Sanctuary.Web.Models.DataFile
{
    public class DataFileDisplayDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SasToken { get; set; }
        public string SasUrl { get; set; }
    }
}

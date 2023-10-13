namespace TNC_API.Models
{
    public class Site
    {
        public int Id { get; set; }
        public string SiteName { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string Address { get;set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string Latitued { get; set; } = string.Empty;
        public int OHID { get; set; }
        public int ASOHID1 { get; set; }
        public int ASOHID2 { get; set; }
        public DateTime Created { get; set; }
    }
}

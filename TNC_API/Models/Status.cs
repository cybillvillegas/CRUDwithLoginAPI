namespace TNC_API.Models
{
    public class Status
    {
        public int Id { get; set; }
        public int PettyCash { get; set; }
        public int Materials { get; set; }
        public int MaterialsRequest { get;set; }
        public int Tools { get; set; }
        public int ToolsRequest { get; set; }
        public int Sites { get; set; }
        public int Users { get; set; }
        public string StatusDescription { get; set; } = string.Empty;
    }
}

namespace TNC_API.DTO.Input
{
    public class PCRRequestDTO
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int TypeId { get; set; }
        public int RCVCR1 { get; set; }
        public int RCVCR2 { get; set; }
        public string CNTCT1 { get; set; } = string.Empty;
        public string CNTCT2 { get; set; } = string.Empty;
        public int PYMNT1 { get; set; }
        public int PYMNT2 { get; set; }
        public int Status { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }
}

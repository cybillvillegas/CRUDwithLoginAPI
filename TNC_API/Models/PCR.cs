namespace TNC_API.Models
{
    public class PCR
    {
        public int PCRID { get; set; }
        public int SiteId { get; set; }
        public int RequestTypeId { get; set; }
        public int UserId { get; set; }
        public int RCPNT2 { get; set; }
        public string CNTCT2 { get; set; } = string.Empty;
        public int PYMNT1 { get; set; }
        public int PYMNT2 { get; set; }
        public DateTime Created { get; set; }
        public bool IsRead { get; set; }
        public int? Status { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }
}

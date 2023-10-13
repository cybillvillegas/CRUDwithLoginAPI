namespace TNC_API.Models
{
    public class RequestType
    {
        public int Id { get; set; }
        public string RequestTypeDescription { get; set; } = string.Empty;
        public int Created { get; set; }
        public int Status { get; set; }
    }
}

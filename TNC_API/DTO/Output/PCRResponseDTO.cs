namespace TNC_API.DTO.Output
{
    public class PCRResponseDTO
    {
        public int Id { get; set; }
        public string Requestor { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Site { get; set; } = string.Empty;
        public float Total { get; set; }
    }
}

namespace TNC_API.DTO.Output
{
    public class PCRLResponseDTO
    {
        public string Item { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float Quantity { get; set; }
        public float Price { get; set; }
    }
}

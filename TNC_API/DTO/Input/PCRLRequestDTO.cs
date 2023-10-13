namespace TNC_API.DTO.Input
{
    public class PCRLRequestDTO
    {
        public string Item { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float Quantity { get; set; }
        public float Price { get; set; }
    }
}

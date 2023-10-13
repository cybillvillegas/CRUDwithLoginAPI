namespace TNC_API.Models
{
    public class PCRL
    {
        public int Id { get; set; }
        public int PCRID { get; set; }
        public string Item { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float Price { get; set; }
        public float Quantity { get; set; }
    }
}

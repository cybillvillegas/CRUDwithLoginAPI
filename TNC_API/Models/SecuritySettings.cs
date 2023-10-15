namespace TNC_API.Models
{
    public class SecuritySettings
    {
        public int NumberOfSalt { get; set; }
        public int NumberOfIterations { get; set; }
        public string DefaultPassword { get; set; }
    }
}

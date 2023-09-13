namespace TNC_API.DTO.Output
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Last_Name { get; set; } = string.Empty;
        public string First_Name { get; set; } = string.Empty;
        public string Middle_Name { get; set; } = string.Empty;
        public string Suffix { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool IsLoggedIn { get; set; }
        public bool IsPasswordChanged { get; set; }
        public DateTime Created { get; set; }
    }
}

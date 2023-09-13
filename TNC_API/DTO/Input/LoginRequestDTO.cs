using System.ComponentModel.DataAnnotations;

namespace TNC_API.DTO.Input
{
    public class LoginRequestDTO
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

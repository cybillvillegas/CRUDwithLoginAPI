using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TNC_API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Last_Name { get; set; } = string.Empty;
        public string First_Name { get; set; } = string.Empty;
        public string Middle_Name { get; set; } = string.Empty;
        public string Suffix { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public int UserRole { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        public bool IsLoggedIn { get; set; }
        public bool IsPasswordChanged { get; set; }
        public int Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastLoggedIn { get; set; }
    }
}

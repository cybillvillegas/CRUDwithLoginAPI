using Microsoft.Extensions.Diagnostics.HealthChecks;
using TNC_API.DTO.Input;
using TNC_API.Models;

namespace TNC_API.Interfaces
{
    public interface ILogin
    {
        string AuthenticateUser(LoginRequestDTO login);
        string GenerateAccessToken(User user);
    }
}

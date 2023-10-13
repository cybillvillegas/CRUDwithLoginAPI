using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Eventing.Reader;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TNC_API.Data;
using TNC_API.DTO.Input;
using TNC_API.Helpers;
using TNC_API.Interfaces;
using TNC_API.Models;

namespace TNC_API.Repositories
{
    public class LoginRepository : ILogin
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;

        public LoginRepository(DatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;   
        }

        public string AuthenticateUser(LoginRequestDTO login)
        {
            int numberofSalt = Convert.ToInt32(Constants.Saltiness);
            int numberofiterations = Convert.ToInt32(Constants.NIterations);
            string AccessToken = "";

            if (login != null)
            {
                var result = _context.Users
                    .Where(u => u.Username == login.Username)
                    .FirstOrDefault();

                if (result == null)
                {
                    return "No user found.";
                } 
                else
                {
                    if (!result.IsLoggedIn)
                    {
                        var HashedPassword = SecurityHelper.HashPassword(login.Password, result.Salt, numberofiterations, numberofSalt);

                        if (result.Hash == HashedPassword)
                        {
                            AccessToken = GenerateAccessToken(result);
                            result.IsLoggedIn = true;
                            result.LastLoggedIn = DateTime.Now;
                            _context.SaveChangesAsync();

                            return AccessToken;
                        } else
                        {
                            return "Invalid credentials.";
                        }
                    } 
                    else
                    {
                        return "User already logged in.";
                    }
                }
            }
            else
            {
                return "No user found.";
            }
        }

        public string GenerateAccessToken(User user)
        {
            string FullName = user.First_Name + " " + (user.Middle_Name != "" ? user.Middle_Name + " " : "") + user.Last_Name + (user.Suffix != "" ? " " + user.Suffix : "");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddDays(7).ToString()),
                new Claim("UserId", user.Id.ToString()),
                new Claim("DisplayName", user.First_Name),
                new Claim("EmployeeName", FullName),
                new Claim("UserRole", user.UserRole.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokens = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: signIn);

            var Token = new JwtSecurityTokenHandler().WriteToken(tokens);

            return Token;
        }
    }
}

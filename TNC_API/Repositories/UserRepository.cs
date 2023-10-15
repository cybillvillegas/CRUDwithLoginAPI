using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Validations.Rules;
using System.Data;
using TNC_API.Data;
using TNC_API.DTO.Input;
using TNC_API.DTO.Output;
using TNC_API.Helpers;
using TNC_API.Interfaces;
using TNC_API.Models;

namespace TNC_API.Repositories
{
    public class UserRepository : IUser
    {
        private readonly DatabaseContext _context;
        private readonly int _numberOfSalt;
        private readonly int _numberOfIterations;
        private readonly string _defaultPassword;

        public UserRepository(DatabaseContext context, SecuritySettings securitySettings)
        {
            _context = context;
            _numberOfSalt = securitySettings.NumberOfSalt;
            _numberOfIterations = securitySettings.NumberOfIterations;
            _defaultPassword = securitySettings.DefaultPassword;
        }

        public async Task<bool> CreateUser(UserRequestDTO user)
        {
            if (user != null)
            {
                try
                {
                    (string Salt, string Hash) = GenerateSaltAndHashForPassword(_defaultPassword);

                    var newUser = new User
                    {
                        First_Name = user.First_Name,
                        Last_Name = user.Last_Name,
                        Middle_Name = user.Middle_Name,
                        Suffix = user.Suffix,
                        Contact = user.Contact,
                        Email = user.Email,
                        Username = user.Username,
                        UserRole = user.UserRole,
                        IsLoggedIn = false,
                        IsPasswordChanged = false,
                        Salt = Salt,
                        Hash = Hash,
                        Status = 1,
                        Created = DateTime.Now,
                    };

                    _context.Users.Add(newUser);

                    await _context.SaveChangesAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return false;
        }

        public async Task<bool> DeleteUser(int id)
        {
            try
            {
                var userToDelete = await _context.Users.FindAsync(id);

                if (userToDelete != null)
                {
                    _context.Users.Remove(userToDelete);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false; // User with the specified ID was not found.
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<UserResponseDTO> GetUser(int id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

                if (user != null)
                {
                    var userResponseDTO = new UserResponseDTO
                    {
                        Id = user.Id,
                        First_Name = user.First_Name,
                        Last_Name = user.Last_Name,
                        Middle_Name = user.Middle_Name,
                        Suffix = user.Suffix,
                        Contact = user.Contact,
                        Email = user.Email,
                        Username = user.Username,
                        Created = user.Created
                    };

                    return userResponseDTO;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<UserResponseDTO>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            var usersList = new List<UserResponseDTO>();

            if (users == null)
            {
                return null;
            }

            foreach (var user in users)
            {
                var userResponseDTO = new UserResponseDTO
                {
                    Id = user.Id,
                    First_Name = user.First_Name,
                    Last_Name = user.Last_Name,
                    Middle_Name = user.Middle_Name,
                    Suffix = user.Suffix,
                    Contact = user.Contact,
                    Email = user.Email,
                    Username = user.Username,
                    Created = user.Created
                };

                usersList.Add(userResponseDTO);
            }

            return usersList;
        }


        public async Task<bool> UpdateUserStatus(int id, int status)
        {
            try
            {
                var userToUpdate = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

                if (userToUpdate != null)
                {
                    userToUpdate.Status = status;
                    await _context.SaveChangesAsync();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateUser(int userId, UserRequestDTO userUpdate)
        {
            try
            {
                var user = new User { Id = userId };

                if (user != null)
                {
                    if (!string.IsNullOrWhiteSpace(userUpdate.Password))
                    {
                        UpdatePassword(user, userUpdate.Password);
                    }
                    else if (!string.IsNullOrWhiteSpace(userUpdate.Status.ToString()))
                    {
                        user.Status = userUpdate.Status;
                    } 
                    else
                    {
                        UpdateUserInfo(user, userUpdate);
                    }

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool UpdatePassword(User user, string newPassword)
        {
            if (user.Hash != SecurityHelper.HashPassword(newPassword, user.Salt, _numberOfIterations, _numberOfSalt)) // Checks if old Hash is equal to new Hash
            {
                (string Salt, string Hash) = GenerateSaltAndHashForPassword(newPassword);
                user.Salt = Salt;
                user.Hash = Hash;

                return true;
            }

            return false;
        }

        private (string Salt, string Hash) GenerateSaltAndHashForPassword(string password)
        {
            var salt = SecurityHelper.GenerateSalt(_numberOfSalt);
            var hash = SecurityHelper.HashPassword(password, salt, _numberOfIterations, _numberOfSalt);
            return (salt, hash);
        }

        private static void UpdateUserInfo(User user, UserRequestDTO userUpdate)
        {
            user.First_Name = userUpdate.First_Name;
            user.Last_Name = userUpdate.Last_Name;
            user.Middle_Name = userUpdate.Middle_Name;
            user.Suffix = userUpdate.Suffix;
            user.Contact = userUpdate.Contact;
            user.Email = userUpdate.Email;
            user.Username = userUpdate.Username;
            user.Status = userUpdate.Status;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations.Rules;
using System.Data;
using TNC_API.Data;
using TNC_API.DTO.Input;
using TNC_API.DTO.Output;
using TNC_API.Helpers;
using TNC_API.Interfaces;
using TNC_API.Models;
using TNC_API.Security;

namespace TNC_API.Repositories
{
    public class UserRepository : IUser
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateUser(UserRequestDTO user)
        {
            if (user != null)
            {
                if (user.Password == user.ConfirmPassword)
                {
                    try
                    {
                        int numberofSalt = Convert.ToInt32(Constants.saltiness);
                        int numberofiterations = Convert.ToInt32(Constants.nIterations);
                        var Salt = SecurityHelper.GenerateSalt(Constants.saltiness);
                        var HashedPassword = SecurityHelper.HashPassword("12345678", Salt, numberofiterations, numberofSalt);

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
                            Hash = HashedPassword,
                            Status = 1,
                            Created = DateTime.Now,
                            LastLoggedIn = DateTime.Now
                        };

                        // Add the new user to the context
                        _context.Users.Add(newUser);

                        // Save changes to the database
                        await _context.SaveChangesAsync();

                        return true; // User creation was successful
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                } else
                {
                    return false;
                }                
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteUser(int id)
        {
            var userToDelete = await _context.Users.FindAsync(id);

            if (userToDelete != null)
            {
                _context.Users.Remove(userToDelete);
                await _context.SaveChangesAsync();
                return true; // User was found and deleted
            }

            return false; // User was not found
        }

        public async Task<UserResponseDTO> GetUser(int id)
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

        public async Task<List<UserResponseDTO>> GetUsers()
        {
            var users =  await _context.Users.ToListAsync(); 
            var usersList = new List<UserResponseDTO>();

            if (users != null)
            {
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
            } else
            {
                return null;
            }
        }

        public async Task<bool> UpdateUserStatus(int id, int status)
        {
            var userToUpdate = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (userToUpdate != null)
            {
                userToUpdate.Status = status;
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> UpdateUser(int userId, UserRequestDTO userUpdate)
        {
            try
            {
                // Find the user in the database by their ID
                var user = await _context.Users.FindAsync(userId);

                int numberofSalt = Convert.ToInt32(Constants.saltiness);
                int numberofiterations = Convert.ToInt32(Constants.nIterations);
                var Salt = SecurityHelper.GenerateSalt(Constants.saltiness);
                var HashedPassword = SecurityHelper.HashPassword(userUpdate.Password, Salt, numberofiterations, numberofSalt);

                if (user != null)
                {
                    // Update user properties with data from the updatedUserData DTO
                    user.First_Name = userUpdate.First_Name;
                    user.Last_Name = userUpdate.Last_Name;
                    user.Middle_Name = userUpdate.Middle_Name;
                    user.Suffix = userUpdate.Suffix;
                    user.Contact = userUpdate.Contact;
                    user.Email = userUpdate.Email;
                    user.Username = userUpdate.Username;
                    user.Hash = HashedPassword;
                    user.Salt = Salt;

                    // Save changes to the database
                    await _context.SaveChangesAsync();

                    return true; // User update was successful
                }
                else
                {
                    // User with the given ID was not found
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, log errors, etc.
                return false;
            }
        }
    }
}

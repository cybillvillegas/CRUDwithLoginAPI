using AutoMapper;
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
        private readonly IMapper _mapper;

        public UserRepository(DatabaseContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _numberOfSalt = configuration.GetValue<int>("SecuritySettings:NumberOfSalt");
            _numberOfIterations = configuration.GetValue<int>("SecuritySettings:NumberOfIterations");
            _defaultPassword = configuration["SecuritySettings:DefaultPassword"];
            _mapper = mapper;
        }

        public async Task<bool> CreateUser(UserRequestDTO user)
        {
            if (user != null)
            {
                try
                {
                    (string Salt, string Hash) = GenerateSaltAndHashForPassword(_defaultPassword);

                    var newUser = _mapper.Map<User>(user);
                    newUser.IsLoggedIn = false;
                    newUser.IsPasswordChanged = false;
                    newUser.Salt = Salt;
                    newUser.Hash = Hash;
                    newUser.Status = 1;
                    newUser.Created = DateTime.Now;

                    await _context.Users.AddAsync(newUser);

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
                    return false;
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
                var user = await _context.Users.FindAsync(id);

                if (user != null)
                {
                    var userResponseDTO = _mapper.Map<UserResponseDTO>(user);

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
                var userResponseDTO = _mapper.Map<UserResponseDTO>(user);

                usersList.Add(userResponseDTO);
            }

            return usersList;
        }

        public async Task<bool> UpdateUser(int userId, UserRequestDTO userUpdate)
        {
            try
            {
                var user = new User { Id = userId };

                if (user != null)
                {
                    if (!(string.IsNullOrWhiteSpace(userUpdate.Password) || string.IsNullOrWhiteSpace(userUpdate.ConfirmPassword)) 
                        && (userUpdate.Password == userUpdate.ConfirmPassword))
                    {
                        UpdatePassword(user, userUpdate.Password);
                    }
                    else if (!string.IsNullOrWhiteSpace(userUpdate.Status.ToString()))
                    {
                        user.Status = userUpdate.Status;
                    } 
                    else
                    {
                        _mapper.Map(user, userUpdate);
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
    }
}

using TNC_API.DTO.Input;
using TNC_API.DTO.Output;
using TNC_API.Models;

namespace TNC_API.Interfaces
{
    public interface IUser
    {
        public Task<List<UserResponseDTO>> GetUsers();
        public Task<UserResponseDTO> GetUser(int id);
        public Task<bool> CreateUser(UserRequestDTO user);
        public Task<bool> UpdateUser(int userId, UserRequestDTO user);
        public Task<bool> DeleteUser(int id);
    }
}

using TNC_API.DTO.Input;
using TNC_API.DTO.Output;

namespace TNC_API.Interfaces
{
    public interface IPettyCashRequest
    {
        public Task<List<PCRResponseDTO>> GetPettyCashRequests();
        public Task<PCRResponseDTO> GetPettyCashRequest(int id);
        public Task<bool> CreatePettyCashRequest(PCRRequestDTO user);
        public Task<bool> UpdatePettyCashRequest(int userId, PCRRequestDTO user);
        public Task<bool> DeletePettyCashRequest(int id);
    }
}

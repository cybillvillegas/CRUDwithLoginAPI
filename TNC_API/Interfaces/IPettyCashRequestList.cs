using TNC_API.DTO.Input;
using TNC_API.DTO.Output;

namespace TNC_API.Interfaces
{
    public interface IPettyCashRequestList
    {
        public Task<List<PCRLResponseDTO>> GetPettyCashRequestList(int id);
        public Task<bool> CreatePettyCashRequestList(List<PCRLRequestDTO> pcrLists, int pcrId);
        public Task<bool> UpdatePettyCashRequestList(List<PCRLRequestDTO> pcrlDetails);
        public Task<bool> DeletePettyCashRequestList(int id);
    }
}

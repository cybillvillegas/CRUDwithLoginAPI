using TNC_API.DTO.Input;
using TNC_API.DTO.Output;

namespace TNC_API.Interfaces
{
    public interface IPettyCashRequestList
    {
        public Task<List<PCRLResponseDTO>> GetPettyCashRequestList(int id);
        public Task<bool> CreatePettyCashRequestList(List<PCRLRequestDTO> pcrLists, int PRCID);
        public Task<bool> UpdatePettyCashRequestList(PCRLRequestDTO pcrlDetails);
        public Task<bool> DeletePettyCashRequestList(int id);
    }
}

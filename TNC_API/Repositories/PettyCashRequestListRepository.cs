using Microsoft.EntityFrameworkCore;
using System.Net;
using TNC_API.Data;
using TNC_API.DTO.Input;
using TNC_API.DTO.Output;
using TNC_API.Interfaces;
using TNC_API.Models;

namespace TNC_API.Repositories
{
    public class PettyCashRequestListRepository : IPettyCashRequestList
    {
        private readonly DatabaseContext _context;

        public async Task<bool> CreatePettyCashRequestList(List<PCRLRequestDTO> pcrLists, int PCRID)
        {
            if (pcrLists != null)
            {
                try
                {
                    foreach (PCRLRequestDTO pcr in pcrLists)
                    {
                        var pcrList = new PCRL
                        {
                            PCRID = PCRID,
                            Item = pcr.Item,
                            Description = pcr.Description,
                            Quantity = pcr.Quantity,
                            Price = pcr.Price
                        };

                        _context.PCRLs.Add(pcrList);
                    }

                    await _context.SaveChangesAsync();
                } 
                catch (Exception ex)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public async Task<List<PCRLResponseDTO>> GetPettyCashRequestList(int id)
        {
            var pcrLists = await _context.PCRLs
                .Where(x => x.PCRID == id)
                .Select(x => new PCRLResponseDTO
                {
                    Item = x.Item,
                    Description = x.Description,
                    Price = x.Price,
                    Quantity = x.Quantity
                })
                .ToListAsync();

            return pcrLists;
        }

        public async Task<bool> DeletePettyCashRequestLists(int id)
        {
            var pcrDetail = await _context.PCRLs.FindAsync(id);
           
            if (pcrDetail != null)
            {
                _context.PCRLs.Remove(pcrDetail);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> UpdatePettyCashRequestList(List<PCRLRequestDTO> pcrDetails)
        {
            try
            {
                foreach (var pcrDetail in pcrDetails)
                {
                    var pcrDetailToUpdate = await _context.PCRLs.FirstOrDefaultAsync(x => x.Id == pcrDetail.DetailId);

                    if (pcrDetailToUpdate != null)
                    {
                        pcrDetailToUpdate.Price = pcrDetail.Price;
                        pcrDetailToUpdate.Quantity = pcrDetail.Quantity;
                        pcrDetailToUpdate.Item = pcrDetail.Item;
                        pcrDetailToUpdate.Description = pcrDetail.Description;

                        await _context.SaveChangesAsync();
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

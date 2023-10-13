using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TNC_API.Data;
using TNC_API.DTO.Input;
using TNC_API.DTO.Output;
using TNC_API.Interfaces;
using TNC_API.Models;

namespace TNC_API.Repositories
{
    public class PettyCashRequestRepository : IPettyCashRequest
    {
        private readonly DatabaseContext _context;

        public async Task<bool> CreatePettyCashRequest(PCRRequestDTO pcr)
        {
            if (pcr != null)
            {
                try
                {
                    var NewRequest = new PCR
                    {
                        SiteId = pcr.SiteId,
                        Remarks = pcr.Remarks,
                        RequestTypeId = pcr.TypeId,
                        IsRead = false,
                        Created = DateTime.Now
                    };

                    _context.PCRs.Add(NewRequest);

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

        public Task<bool> DeletePettyCashRequest(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PCRResponseDTO> GetPettyCashRequest(int id)
        {
            var pcr = await _context.PCRs
                .Where(pcr => pcr.PCRID == id)
                .Join(_context.Users, a => a.UserId, b => b.Id, (a, b) => new { a, b })
                .Join(_context.RequestTypes, c => c.a.RequestTypeId, d => d.Id, (c, d) => new { c, d })
                .Join(_context.Sites, e => e.c.a.SiteId, f => f.Id, (e, f) => new { e, f })
                .Join(_context.Statuses, g => g.e.c.a.Status, h => h.PettyCash, (g, h) => new { g, h })
                .Join(_context.RequestTypes, i => i.g.e.c.a.RequestTypeId, j => j.Id, (i, j) => new { i, j })
                .GroupJoin(_context.PCRLs, k => k.i.g.e.c.a.PCRID, l => l.PCRID, (k, l) => new { k, l })
                .SelectMany(m => m.l.DefaultIfEmpty(), (m, l) => new { m.k, l })
                .GroupBy(m => new
                {
                    m.k.i.g.e.c.a.PCRID,
                    m.k.i.g.e.c.b.First_Name,
                    m.k.i.g.e.c.b.Last_Name,
                    m.k.i.g.e.c.a.Created,
                    m.k.i.g.f.SiteName,
                    m.k.i.g.f.ClientName,
                    m.k.i.h.StatusDescription,
                    m.k.j.RequestTypeDescription
                })
                .Select(x => new PCRResponseDTO
                {
                    Id = x.Key.PCRID,
                    Requestor = x.Key.First_Name + " " + x.Key.Last_Name,
                    Created = x.Key.Created,
                    Site = x.Key.SiteName + " - " + x.Key.ClientName,
                    Status = x.Key.StatusDescription,
                    Type = x.Key.RequestTypeDescription,
                    Total = x.Sum(y => (y.l != null ? y.l.Quantity * y.l.Price : 0))
                }).FirstOrDefaultAsync();

            if (pcr != null)
            {
                return pcr;
            }

            return null;
        }

        public async Task<List<PCRResponseDTO>> GetPettyCashRequests()
        {
            var pcrList = await _context.PCRs
                .Join(_context.Users, a => a.UserId, b => b.Id, (a, b) => new { a, b })
                .Join(_context.RequestTypes, c => c.a.RequestTypeId, d => d.Id, (c, d) => new { c, d })
                .Join(_context.Sites, e => e.c.a.SiteId, f => f.Id, (e, f) => new { e, f })
                .Join(_context.Statuses, g => g.e.c.a.Status, h => h.PettyCash, (g, h) => new { g, h })
                .Join(_context.RequestTypes, i => i.g.e.c.a.RequestTypeId, j => j.Id, (i, j) => new { i, j })
                .GroupJoin(_context.PCRLs, k => k.i.g.e.c.a.PCRID, l => l.PCRID, (k, l) => new { k, l })
                .SelectMany(m => m.l.DefaultIfEmpty(), (m, l) => new { m.k, l })
                .GroupBy(m => new
                {
                    m.k.i.g.e.c.a.PCRID,
                    m.k.i.g.e.c.b.First_Name,
                    m.k.i.g.e.c.b.Last_Name,
                    m.k.i.g.e.c.a.Created,
                    m.k.i.g.f.SiteName,
                    m.k.i.g.f.ClientName,
                    m.k.i.h.StatusDescription,
                    m.k.j.RequestTypeDescription
                })
                .Select(x => new PCRResponseDTO
                {
                    Id = x.Key.PCRID,
                    Requestor = x.Key.First_Name + " " +  x.Key.Last_Name,
                    Created = x.Key.Created,
                    Site = x.Key.SiteName + " - " + x.Key.ClientName,
                    Status = x.Key.StatusDescription,
                    Type = x.Key.RequestTypeDescription,
                    Total = x.Sum(y => (y.l != null ? y.l.Quantity * y.l.Price : 0))
                }).ToListAsync();
            
            if (pcrList != null)
            {
                return pcrList;
            } else
            {
                return null;
            }
        }

        public async Task<bool> UpdatePettyCashRequest(int pcrId, PCRRequestDTO pcrUpdate)
        {
            try
            {
                var pcr = await _context.PCRs.FindAsync(pcrId);

                if (pcr != null)
                {
                    if (pcr.Status != null)
                    {
                        pcr.Status = pcrUpdate.Status;
                    }
                    else
                    {
                        pcr.SiteId = pcrUpdate.SiteId;
                        pcr.RequestTypeId = pcrUpdate.TypeId;
                        pcr.RCPNT2 = pcrUpdate.RCVCR2;
                        pcr.CNTCT2 = pcrUpdate.CNTCT2;
                        pcr.PYMNT1 = pcrUpdate.PYMNT1;
                        pcr.PYMNT2 = pcrUpdate.PYMNT2;
                        pcr.Remarks = pcrUpdate.Remarks;
                    }

                    await _context.SaveChangesAsync();

                    return true;
                }

                return false;
            } 
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

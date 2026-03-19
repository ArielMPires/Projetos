using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NexuSys.Data;
using NexuSys.DTOs.Nfs;
using NexuSys.Entities;
using NexuSys.Interfaces;

namespace NexuSys.Services
{
    public class NFService : INFs
    {
        public readonly ApplicationDbContext _context;
        public readonly IMapper _mapper;
        public NFService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Return> DeleteNFs(int id)
        {
            var result = new Return();
            try
            {
                var nf = await _context.NFs.FirstOrDefaultAsync(e => e.ID == id);
                _context.NFs.Remove(nf);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = $"NF com ID:{id} foi deletada!";
            }
            catch(Exception ex) 
            {
                result.Result= false;
                result.Message = ex.Message;
                result.Error = ex.InnerException.Message;
            }
            return result;
        }

        public async Task<Return> NewNFs(NewNFsDTO NFs)
        {
            var result = new Return();
            try
            {
                var nf = _mapper.Map<NFs>(NFs);
                await _context.NFs.AddAsync(nf);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = $"NF {NFs.Number} adicionado com sucesso";
                result.Data = nf;
            }
            catch(Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException.Message;
            }

            return result;
        }

        public async Task<ByNFsDTO> NFsByID(int id) => await _context.NFs
            .AsNoTracking()
            .ProjectTo<ByNFsDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(e => e.ID == id);

        public async Task<List<NFsDTO>> NFsList() => await _context.NFs
            .AsNoTracking()
            .Include(e => e.customersFK)
            .ProjectTo<NFsDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();

        public async Task<Return> UpdateNFs(EditNFsDTO NFs)
        {
            var result = new Return();
            try
            {
                var nf = _mapper.Map<NFs>(NFs);
                _context.NFs.Update(nf);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = $"NF {NFs.Number} Alterado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException.Message;
            }

            return result;
        }
    }
}

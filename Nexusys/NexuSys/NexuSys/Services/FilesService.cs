using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NexuSys.Data;
using NexuSys.DTOs.Files;
using NexuSys.Entities;
using NexuSys.Interfaces;
using System;

namespace NexuSys.Services
{
    public class FilesService : IFiles
    {
        private readonly ApplicationDbContext _context;
        public readonly IMapper _mapper;

        public FilesService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Files

        public async Task<List<Files>> FilesList() =>
            await _context.Files
                .AsNoTracking()
                .ToListAsync();

        public async Task<Files> Files_SearchByPc(int id) =>
            await _context.Files
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.ID == id);

        public async Task<Return> NewFiles(Files New)
        {
            var result = new Return();
            try
            {
                await _context.Files.AddAsync(New);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = "Arquivo cadastrado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> UpdateFiles(Files UpdateFiles)
        {
            var result = new Return();
            try
            {
                _context.Files.Update(UpdateFiles);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = "Arquivo atualizado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> DeleteFiles(int DeleteFiles)
        {
            var result = new Return();
            try
            {
                var file = await _context.Files.FirstOrDefaultAsync(e => e.ID == DeleteFiles);
                _context.Files.Remove(file);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = $"Arquivo ID:{DeleteFiles} removido";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        #endregion

        #region Files All 

        public async Task<Return> NewFilesAll(List<NewFileDTO> New)
        {
            var result = new Return();
            try
            {
                foreach (var file in New)
                {

                    var arq = _mapper.Map<Files>(file);

                    var entity = _context.Model.FindEntityType(typeof(Files));

                    foreach (var prop in entity.GetProperties())
                    {
                        Console.WriteLine(prop.Name);
                    }


                    var history = arq.historyFK;
                    arq.historyFK = null;
                    await _context.History.AddAsync(history);
                    await _context.SaveChangesAsync();
                    _context.Entry(history).State = EntityState.Detached;

                    arq.History = history.ID;
                    await _context.Files.AddAsync(arq);
                    await _context.SaveChangesAsync();
                    _context.Entry(arq).State = EntityState.Detached;
                }
                result.Result = true;
                result.Message = "Arquivos cadastrado";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> UpdateFilesAllTenant(Files UpdateFiles)
        {
            var result = new Return();
            try
            {
                _context.Files.Update(UpdateFiles);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = "Arquivo atualizado para todos os tenants";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        #endregion

        #region Folder

        public async Task<List<FileFolder>> FolderList() =>
            await _context.FileFolder
                .AsNoTracking()
                .ToListAsync();

        public async Task<FileFolder> Folder_SearchByPc(int id) =>
            await _context.FileFolder
                .AsNoTracking()
              .FirstOrDefaultAsync(e => e.ID == id);

        public async Task<Return> NewFolder(FileFolder NewFolder)
        {
            var result = new Return();
            try
            {
                await _context.FileFolder.AddAsync(NewFolder);
                await _context.SaveChangesAsync();
                _context.Entry(NewFolder).State = EntityState.Detached;
                result.Result = true;
                result.Message = "Pasta criada com sucesso";
                result.Data = await _context.FileFolder.AsNoTracking().FirstOrDefaultAsync(e => e.ID == NewFolder.ID);
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> UpdateFolder(FileFolder Update)
        {
            var result = new Return();
            try
            {
                _context.FileFolder.Update(Update);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = "Pasta atualizada com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> DeleteFolder(int DeleteFolder)
        {
            var result = new Return();
            try
            {
                var folder = await _context.FileFolder.FirstOrDefaultAsync(e => e.ID == DeleteFolder);
                _context.FileFolder.Remove(folder);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = $"Pasta ID:{DeleteFolder} removida";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        #endregion

        #region Folder All Tenant

        public async Task<Return> NewFolderAllTenant(FileFolder NewFolder)
        {
            var result = new Return();
            try
            {
                await _context.FileFolder.AddAsync(NewFolder);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = "Pasta criada para todos os tenants";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> UpdateFolderAllTenant(FileFolder Update)
        {
            var result = new Return();
            try
            {
                _context.FileFolder.Update(Update);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = "Pasta atualizada para todos os tenants";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        #endregion
    }
}

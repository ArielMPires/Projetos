using Domus.Models.DB;
using Domus.Models;
using Domus.Repositories.Interfaces;
using Domus.DataBase;
using Microsoft.EntityFrameworkCore;
using Domus.Services;
using Microsoft.Extensions.Configuration;

namespace Domus.Repositories {
    public class FilesRepository : IFiles {

        #region Property
        private readonly TenantProvider _tenantProvider;
        private readonly ITenantDbContextFactory _tenantDbContext;
        public ApplicationDbContext db { get; set; }
        public FilesRepository(ApplicationDbContext context,ITenantDbContextFactory tenantsContext, TenantProvider tentantprovider)
        {
            db = context;
            _tenantDbContext = tenantsContext;
            _tenantProvider = tentantprovider;
        }
        #endregion

        #region File
        public async Task<Files> Files_SearchByPc(int id) => await db.Files.FirstOrDefaultAsync(e => e.ID == id);
        public Task<List<Files>> FilesList() => db.Files.ToListAsync();

        public async Task<Return> NewFiles(Files New) {
            
            var result = new Return();

            try {
                await db.Files.AddAsync(New);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Novo Arquivo Cadastrado";
            } catch (Exception ex) {
                
                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> UpdateFiles(Files UpdateFiles) {

            var result = new Return();

            try {
                db.Files.Update(UpdateFiles);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Arquivos Atualizado!";

            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> DeleteFiles(int DeleteFiles) {

            var result = new Return();

            try {

                var search = await db.Files.FirstOrDefaultAsync(e => e.ID == DeleteFiles);
                db.Files.Remove(search);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Arquivo Deletado!";
            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> NewFilesAllTenant(Files New)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                        await context.Files.AddAsync(New);
                        await context.SaveChangesAsync();
                        result.Result = true;
                        result.Message = "Novo Arquivo Cadastrado";

                }
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message + ex.InnerException.Message;
            }
            return result;
        }

        public async Task<Return> UpdateFilesAllTenant(Files UpdateFiles)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    context.Files.Update(UpdateFiles);
                    await context.SaveChangesAsync();
                    result.Result = true;
                    result.Message = "Arquivos Atualizado!";

                }
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message + ex.InnerException.Message;
            }
            return result;
        }

        #endregion

        #region Folder
        public Task<FileFolder> Folder_SearchByPc(int id) => db.FileFolder.FirstOrDefaultAsync(e => e.ID == id);
        public Task<List<FileFolder>> FolderList() => db.FileFolder.ToListAsync();
        
        public async Task<Return> NewFolder(FileFolder NewFolder) {
            
            var result = new Return();

            try {
                await db.FileFolder.AddAsync(NewFolder);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = $"{NewFolder.ID}";
                
            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> DeleteFolder(int DeleteFolder) {
            
            var result = new Return();

            try {
                var search = await db.FileFolder.FirstOrDefaultAsync(e => e.ID == DeleteFolder);
                db.FileFolder.Remove(search);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Pasta de Arquivos Deletada!";
            } catch(Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> UpdateFolder(FileFolder Update) {

            var result = new Return();

            try {
                db.FileFolder.Update(Update);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Pasta de Arquivos Atualizada!";

            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> NewFolderAllTenant(FileFolder NewFolder)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    await context.FileFolder.AddAsync(NewFolder);
                    await context.SaveChangesAsync();
                    result.Result = true;
                    result.Message = $"{NewFolder.ID}";

                }
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message + ex.InnerException.Message;
            }
            return result;
        }

        public async Task<Return> UpdateFolderAllTenant(FileFolder Update)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    context.FileFolder.Update(Update);
                    await context.SaveChangesAsync();
                    result.Result = true;
                    result.Message = "Pasta de Arquivos Atualizada!";

                }
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message + ex.InnerException.Message;
            }
            return result;
        }

        #endregion

    }
}

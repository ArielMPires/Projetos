using Domus.DataBase;
using Domus.Models;
using Domus.Models.DB;
using Domus.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Repositories {
    public class ManualsRepository : IManuals {

        #region Property
        public ApplicationDbContext db { get; set; }
        public ManualsRepository(ApplicationDbContext context) => db = context;

        #endregion
        public async Task<Manuals> Manuals_SearchBy(int id) => await db.Manuals.FirstOrDefaultAsync(e => e.ID == id);
        public async Task<List<Manuals>> ListManuals() => await db.Manuals.ToListAsync();

        public async Task<Return> NewManuals(Manuals New) {

            var result = new Return();
            try {
                await db.Manuals.AddAsync(New);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "CheckList Cadastrado!";

            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> UpdateManuals(Manuals Update) {

            var result = new Return();
            try {
                db.Manuals.Update(Update);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "CheckList Atualizado!";

            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> DeleteManuals(int Delete) {
            
            var result = new Return();
            try {
                var search = await db.Manuals.FirstOrDefaultAsync(e => e.ID == Delete);
                db.Manuals.Remove(search);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "CheckList Deletado!";

            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }
    }
}

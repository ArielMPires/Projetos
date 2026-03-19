using Domus.DataBase;
using Domus.Models;
using Domus.Models.DB;
using Domus.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Repositories {

    public class CheckListRepository : ICheckList {

        #region Property
        public ApplicationDbContext db { get; set; }
        public CheckListRepository(ApplicationDbContext context) => db =  context;
        #endregion

        #region CheckList
        public async Task<Checklist> CheckList_SearchByPc(int id) => await db.CheckList.FirstOrDefaultAsync(e => e.ID == id);
        public Task<List<Checklist>> List() => db.CheckList.ToListAsync();

        public async Task<Return> NewCheckList(Checklist New) {
            
            var result = new Return();
            try {
                await db.CheckList.AddAsync(New);
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

        public async Task<Return> DeleteCheckList(int DeleteCheckList) {

            var result = new Return();
            try {
                var search = await db.CheckList.FirstOrDefaultAsync(e => e.ID == DeleteCheckList);
                db.CheckList.Remove(search);
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

        public async Task<Return> UpdateCheckList(Checklist Update) {
            
            var result = new Return();
            try {
                db.CheckList.Update(Update);
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
        #endregion
    }
}

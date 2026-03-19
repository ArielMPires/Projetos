using Domus.DataBase;
using Domus.Models;
using Domus.Models.DB;
using Domus.Services;
using Domus.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Repositories {
    public class MaintenanceRepository : IMaintenance {

        #region Property
        public ApplicationDbContext db { get; set; }

        public MaintenanceRepository(ApplicationDbContext context) => db = context;
        #endregion

        #region Maintenance

        public async Task<Maintenance> Maintenance_SearchByPc(int id) => await db.Maintenance.FirstOrDefaultAsync(e => e.ID == id);

        public Task<List<Maintenance>> MaintenanceList() => db.Maintenance.ToListAsync();

        public async Task<Return> DeleteMaintenance(int MaintenanceDelete) {

            var result = new Return();

            try {
                var search = await db.Maintenance.FirstOrDefaultAsync(e => e .ID == MaintenanceDelete);
                db.Maintenance.Remove(search);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Manutenção Excluida!";
                
            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> UpdateMaintenance(Maintenance Update) {
            
            var result = new Return();

            try {
                db.Maintenance.Update(Update);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Manutenção Atualizada com Sucesso!";

            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
                return result;
        }

        public async Task<Return> NewMaintenance(Maintenance New) {

            var result = new Return();

            try {
                await db.Maintenance.AddAsync(New);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Manutenção Salvar com Sucesso!";
                return result;
            } 
            catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        #endregion

        #region CheckList
        public async Task<Maintenance_CheckList> CheckList_SearchByPc(int id) => await db.Maintenance_CheckList.FirstOrDefaultAsync( e => e.ID == id);
        public Task<List<Maintenance_CheckList>> Checklist_List() => db.Maintenance_CheckList.ToListAsync();
        
        public async Task<Return> Delete_CheckList(int Maintenance_CheckListDelete) {

            var result = new Return();

            try {

                var search = await db.Maintenance_CheckList.FirstOrDefaultAsync(e => e.ID == Maintenance_CheckListDelete);
                db.Maintenance_CheckList.Remove(search);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "CheckList Apagada!";

            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> New_CheckList(Maintenance_CheckList NewCheck) {
            
            var result = new Return();

            try {
                await db.Maintenance_CheckList.AddAsync(NewCheck);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "CheckList de Manunteção Salva com Sucesso!";
                
            }catch(Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> Update_CheckList(Maintenance_CheckList Update) {

            var result = new Return();

            try {

                db.Maintenance_CheckList.Update(Update);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "CheckList Atualizado com Sucesso!";

            }catch(Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }
        #endregion

        #region Scheduled

        public async Task<Scheduled_Maintenance> Scheduled_SearchByPc(int id) => await db.Scheduled_Maintenance.FirstOrDefaultAsync(e => e.ID == id);
        public async Task<List<Scheduled_Maintenance>> ScheduledList() => await db.Scheduled_Maintenance.ToListAsync();
        public async Task<Return> NewScheduled (Scheduled_Maintenance New) {

            var result = new Return();
            try {
                await db.Scheduled_Maintenance.AddAsync(New);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Agendamento Cadastrado com Sucesso!";

            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> DeleteScheduled (int Scheduled_MaintenanceDelete) {

            var result = new Return();

            try {
                var search = await db.Scheduled_Maintenance.FirstOrDefaultAsync(e => e.ID == Scheduled_MaintenanceDelete);
                db.Scheduled_Maintenance.Remove(search);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Agendamento Deletado!";

            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> UpdateScheduled (Scheduled_Maintenance Update) {

            var result = new Return();

            try {
                db.Scheduled_Maintenance.Update(Update);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Agendamento Atualizado!";
                
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

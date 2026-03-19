using Domus.Models.DB;
using Domus.Models;


namespace Domus.Repositories.Interfaces {
    public interface IMaintenance {

        #region Maintenance
        Task<Return> NewMaintenance(Maintenance New);
        Task<Return> DeleteMaintenance(int MaintenanceDelete);
        Task<Return> UpdateMaintenance(Maintenance Update);
        Task<List<Maintenance>> MaintenanceList();
        Task<Maintenance> Maintenance_SearchByPc(int id);
        #endregion

        #region Scheduled_Maintenance
        Task<Return> NewScheduled (Scheduled_Maintenance New);
        Task<Return> DeleteScheduled (int Scheduled_MaintenanceDelete);
        Task<Return> UpdateScheduled (Scheduled_Maintenance Update);
        Task<List<Scheduled_Maintenance>> ScheduledList();
        Task<Scheduled_Maintenance> Scheduled_SearchByPc(int id);
        #endregion

        #region Maintenance_CheckList

        Task<Return> New_CheckList(Maintenance_CheckList NewCheck);
        Task<Return> Delete_CheckList(int Maintenance_CheckListDelete);
        Task<Return> Update_CheckList(Maintenance_CheckList Update);
        Task<List<Maintenance_CheckList>> Checklist_List();
        Task<Maintenance_CheckList> CheckList_SearchByPc(int id);
        #endregion
    }
}

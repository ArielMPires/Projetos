using Domus.Models;
using Domus.Models.DB;

namespace Domus.Repositories.Interfaces {
    public interface ICheckList {

        Task<Return> NewCheckList(Checklist New);
        Task<Return> DeleteCheckList(int DeleteCheckList);
        Task<Return> UpdateCheckList(Checklist Update); 
        Task<List<Checklist>> List();
        Task<Checklist> CheckList_SearchByPc(int id);
    }
}

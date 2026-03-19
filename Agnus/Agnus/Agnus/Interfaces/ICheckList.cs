using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agnus.Models;
using Agnus.Models.DB;

namespace Agnus.Interfaces
{
    public interface ICheckList
    {
        public void SetHeader(string tenantId, string token);
        public void SetTenantHeader(string tenantId);

        Task<Return> NewCheckList(Checklist New);
        Task<Return> DeleteCheckList(int DeleteCheckList);
        Task<Return> UpdateCheckList(Checklist Update);
        Task<List<Checklist>> List();
        Task<Checklist> CheckList_SearchByPc(int id);
    }
}

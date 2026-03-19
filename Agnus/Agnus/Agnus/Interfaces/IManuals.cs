using Agnus.Models;
using Agnus.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Interfaces
{
    public interface IManuals
    {
        public void SetHeader(string tenantId, string token);
        public void SetTenantHeader(string tenantId);

        Task<Return> NewManuals(Manuals New);

        Task<Return> UpdateManuals(Manuals Update);

        Task<Return> DeleteManuals(int Delete);

        Task<List<Manuals>> ListManuals();

        Task<Manuals> Manuals_SearchBy(int id);
        
    }
}

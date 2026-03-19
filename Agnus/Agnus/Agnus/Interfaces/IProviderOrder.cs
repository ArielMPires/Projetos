using Agnus.Models;
using Agnus.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Interfaces
{
    public interface IProviderOrder
    {
        public void SetHeader(string tenantId, string token);
        public void SetTenantHeader(string tenantId);

        #region Provider Order
        Task<Return> NewProvider(Provider_Order New);

        Task<Return> UpdateProvider(Provider_Order Update);

        Task<Return> DeleteProvider(int Delete);

        Task<List<Provider_Order>> ListProvider();

        Task<Provider_Order> Provider_SearchById(int id);
        #endregion

        #region Service Provider
        Task<Return> NewService(Service_Providers New);

        Task<Return> UpdateService(Service_Providers Update);

        Task<Return> DeleteService(int Delete);

        Task<List<Service_Providers>> ListServices();

        Task<Service_Providers> Service_SearchById(int id);
        #endregion
    }
}

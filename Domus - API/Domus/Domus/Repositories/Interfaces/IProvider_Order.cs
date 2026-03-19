using Domus.Models;
using Domus.Models.DB;

namespace Domus.Repositories.Interfaces {
    public interface IProvider_Order {

        #region ProviderOrder
        Task<Return> NewProvider(Provider_Order New);
        Task<Return> UpdateProvider(Provider_Order Update);
        Task<Return> DeleteProvider(int Delete);
        Task<List<Provider_Order>> ListProvider();
        Task<Provider_Order> Provider_SearchById(int id);

        #endregion

        #region ServiceProvider
        Task<Return> NewService (Service_Providers New);
        Task<Return> UpdateService (Service_Providers Update);
        Task<Return> DeleteService (int Delete);
        Task<List<Service_Providers>> ListServices();
        Task<Service_Providers> Service_SearchById(int id);

        #endregion
    }
}

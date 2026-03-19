using Domus.Models;
using Domus.Models.DB;

namespace Domus.Repositories.Interfaces {
    public interface IManuals {

        Task<Return> NewManuals(Manuals New);
        Task<Return> UpdateManuals(Manuals Update);
        Task<Return> DeleteManuals(int Delete);
        Task<List<Manuals>> ListManuals();
        Task<Manuals> Manuals_SearchBy(int id);
    }
}

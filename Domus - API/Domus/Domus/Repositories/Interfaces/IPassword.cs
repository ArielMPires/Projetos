using Domus.DTO.Passwords;
using Domus.Models;
using Domus.Models.DB;

namespace Domus.Repositories.Interfaces {
    public interface IPassword {

        #region Passoword
        Task<Return> NewPassoword(PasswordDTO New);
        Task<Return> DeletePassoword(int Delete);
        Task<Return> UpdatePassoword(Passwords Update);
        Task<IEnumerable<PasswordDTO>> PasswordsList();
        Task<Passwords> Password_SearchBy(int id);

        #endregion

        #region Type

        Task<Return> NewType(Type_Passwords New);
        Task<Return> DeleteType(int Delete);
        Task<Return> UpdateType(Type_Passwords Update);
        Task<List<Type_Passwords>> TypeList();
        Task<Type_Passwords> Type_SearchBy(int id);

        #endregion
    }
}

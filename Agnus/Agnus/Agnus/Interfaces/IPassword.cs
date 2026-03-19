using Agnus.DTO.Passwords;
using Agnus.Models;
using Agnus.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Interfaces
{
    public interface IPassword
    {
        public void SetHeader(string tenantId, string token);
        public void SetTenantHeader(string tenantId);

        #region Password
        Task<List<PasswordDTO>> PasswordsList();
        Task<PasswordByDTO> Password_SearchBy(int id);
        Task<Return> NewPassoword(NewPasswordDTO New);
        Task<Return> DeletePassoword(int Delete);
        Task<Return> UpdatePassoword(EditPasswordDTO Update);
        #endregion

        #region Type Password
        Task<List<Type_Passwords>> TypeList();
        Task<Type_Passwords> Type_SearchBy(int id);
        Task<Return> UpdateType(Type_Passwords Update);
        Task<Return> DeleteType(int Delete);
        Task<Return> NewType(Type_Passwords New);
        #endregion
    }
}

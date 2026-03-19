using Agnus.DTO.Computer;
using Agnus.DTO.Patrimony;
using Agnus.Models;
using Agnus.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Interfaces
{
    public interface IPatrimony
    {
        public void SetHeader(string tenantId, string token);
        public void SetTenantHeader(string tenantId);

        #region Patrimony
        Task<Return> NewPatrimony(Patrimony New);
        Task<Return> UpdatePatrimony(Patrimony Update);
        Task<Return> DeletePatrimony(int Delete);
        Task<IEnumerable<PatrimonyDTO>> PatrimonyList();
        Task<Patrimony> Patrimony_SearchBy(int id);
        Task<IEnumerable<PatrimonyDTO>> PCRegister();

        #endregion

        #region Category
        Task<Return> NewCategory(Patrimony_Category New);
        Task<Return> UpdateCategory(Patrimony_Category Update);
        Task<Return> DeleteCategory(int Delete);
        Task<List<Patrimony_Category>> CategoryList();
        Task<Patrimony_Category> Category_SearchBy(int id);

        #endregion

        #region Computer
        Task<Return> NewComputer(Computer New);
        Task<Return> UpdateComputer(Computer Update);
        Task<Return> DeleteComputer(int Delete);
        Task<List<Computer>> ComputerList();
        Task<IEnumerable<PCDTO>> ComputerListByOwner(int owner);
        Task<Computer> Computer_SearchByPc(int id);
        #endregion
    }
}

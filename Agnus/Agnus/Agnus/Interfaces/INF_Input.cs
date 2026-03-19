using Agnus.Models.DB;
using Agnus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agnus.DTO.NF;

namespace Agnus.Interfaces
{
    public interface INF_Input
    {
        public void SetHeader(string tenantId, string token);
        public void SetTenantHeader(string tenantId);

        #region NF_Input
        Task<Return> InputNew(NF_Input New);
        Task<IEnumerable<InputDTO>> InputList();
        Task<NF_Input> Input_SearchByNf(int id);

        #endregion

        #region NF_Output
        Task<Return> OutputNew(NF_Output New);
        Task<IEnumerable<OutputDTO>> OutputList();
        Task<NF_Output> OutputSearchByNf(int id);

        #endregion
    }
}

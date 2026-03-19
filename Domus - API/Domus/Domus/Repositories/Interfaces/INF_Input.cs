using Domus.DTO.NF;
using Domus.Models;
using Domus.Models.DB;

namespace Domus.Repositories.Interfaces {
    public interface INF_Input {

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

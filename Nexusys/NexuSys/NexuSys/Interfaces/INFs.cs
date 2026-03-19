using NexuSys.DTOs.Nfs;
using NexuSys.Entities;

namespace NexuSys.Interfaces
{
    public interface INFs
    {
        #region NFs
        Task<Return> NewNFs(NewNFsDTO NFs);
        Task<Return> UpdateNFs(EditNFsDTO NFs);
        Task<Return> DeleteNFs(int id);
        Task<List<NFsDTO>> NFsList();
        Task<ByNFsDTO> NFsByID(int id);
        #endregion
    }
}

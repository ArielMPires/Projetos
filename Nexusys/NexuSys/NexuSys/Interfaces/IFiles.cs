using NexuSys.DTOs.Files;
using NexuSys.Entities;

namespace NexuSys.Interfaces
{
    public interface IFiles
    {
        #region Files
        Task<Return> NewFiles(Files New);
        Task<Return> NewFilesAll(List<NewFileDTO> New);
        Task<Return> DeleteFiles(int DeleteFiles);
        Task<Return> UpdateFiles(Files UpdateFiles);
        Task<Return> UpdateFilesAllTenant(Files UpdateFiles);
        Task<List<Files>> FilesList();
        Task<Files> Files_SearchByPc(int id);

        #endregion

        #region FileFolder
        Task<Return> NewFolder(FileFolder NewFolder);
        Task<Return> NewFolderAllTenant(FileFolder NewFolder);
        Task<Return> DeleteFolder(int DeleteFolder);
        Task<Return> UpdateFolder(FileFolder Update);
        Task<Return> UpdateFolderAllTenant(FileFolder Update);
        Task<List<FileFolder>> FolderList();
        Task<FileFolder> Folder_SearchByPc(int id);

        #endregion
    }
}

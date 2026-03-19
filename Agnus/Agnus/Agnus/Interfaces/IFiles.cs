using Agnus.Models;
using Agnus.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Interfaces
{
    public interface IFiles
    {
        public void SetHeader(string tenantId, string token);
        public void SetTenantHeader(string tenantId);

        #region Files
        Task<Return> NewFiles(Files New);
        Task<Return> NewFilesAllTenant(Files New);
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

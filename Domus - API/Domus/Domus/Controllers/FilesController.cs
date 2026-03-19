using Domus.Models;
using Domus.Models.DB;
using Domus.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Domus.Controllers {
    [Route("Domus/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase {

        public readonly IFiles _files;
        public FilesController(IFiles files) => _files = files;

        #region Files
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Files>> ById(int id) => await _files.Files_SearchByPc(id);
        [Authorize]
        [HttpGet("List")]
        public async Task<ActionResult<List<Files>>> List() => await _files.FilesList();
        [Authorize]
        [HttpPut("Update")]
        public async Task<ActionResult<Return>> Update([FromBody] Files update) => await _files.UpdateFiles(update);
        [Authorize]
        [HttpPost("New")]
        public async Task<ActionResult<Return>> New([FromBody] Files newfiles) => await _files.NewFiles(newfiles);
        [Authorize]
        [HttpPut("UpdateAll")]
        public async Task<ActionResult<Return>> UpdateAll([FromBody] Files update) => await _files.UpdateFilesAllTenant(update);
        [Authorize]
        [HttpPost("NewAll")]
        public async Task<ActionResult<Return>> NewAll([FromBody] Files newfiles) => await _files.NewFilesAllTenant(newfiles);
        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<Return>> Delete(int id) => await _files.DeleteFiles(id);

        #endregion

        #region FileFolder
        [Authorize]
        [HttpGet("Folder/{id}")]
        public async Task<ActionResult<FileFolder>> byid(int id) => await _files.Folder_SearchByPc(id);
        [Authorize]
        [HttpGet("Folder/List")]
        public async Task<ActionResult<List<FileFolder>>> ListFolder() => await _files.FolderList();
        [Authorize]
        [HttpPut("Folder/Update")]
        public async Task<ActionResult<Return>> UpdateFolder([FromBody] FileFolder update) => await _files.UpdateFolder(update);
        [Authorize]
        [HttpPost("Folder/New")]
        public async Task<ActionResult<Return>> New([FromBody] FileFolder newFolder) => await _files.NewFolder(newFolder);
        [Authorize]
        [HttpPut("Folder/UpdateAll")]
        public async Task<ActionResult<Return>> UpdateFolderAll([FromBody] FileFolder update) => await _files.UpdateFolderAllTenant(update);
        [Authorize]
        [HttpPost("Folder/NewAll")]
        public async Task<ActionResult<Return>> NewAll([FromBody] FileFolder newFolder) => await _files.NewFolderAllTenant(newFolder);
        [Authorize]
        [HttpDelete("Folder/Delete/{id}")]
        public async Task<ActionResult<Return>> DeleteFolder(int id) => await _files.DeleteFolder(id);

        #endregion
    }
}

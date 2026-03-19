using Domus.DTO.Computer;
using Domus.DTO.Users;
using Domus.Models;
using Domus.Models.DB;
using Domus.Repositories.Interfaces;
using Domus.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Domus.Controllers
{
    [Route("Domus/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        #region Property
        public readonly IUsers _users;
        private readonly IHubContext<NotificationHub> _hubContext;
        #endregion
        public UsersController(IUsers users, IHubContext<NotificationHub> hubContext)
        {
            _users = users;
            _hubContext = hubContext;
        }

        #region Users
        [Authorize]
        [HttpPost("NewUser")]
        public async Task<ActionResult<Return>> NewUser([FromBody] CreateUserDTO user)
        {
            var result = await _users.NewUser(user);
            await _hubContext.Clients.All.SendAsync("UpdateUser");
            return result;
        }
        [Authorize]
        [HttpPost("User/Theme")]
        public async Task<ActionResult<Return>> SwitchTheme([FromBody] ThemeDTO theme)
        {
            var result = await _users.SwitchTheme(theme);
            await _hubContext.Clients.All.SendAsync("UpdateTheme");
            return result;
        }
        [Authorize]
        [HttpGet("User/{id}/Theme")]
        public async Task<ActionResult<ThemeIdDTO>> ThemeByUser(int id) => Ok(await _users.ThemeByUser(id));
        [HttpPost("Log_In")]
        public async Task<ActionResult<Return>> Log_in([FromBody] Log_In user) => await _users.Log_In(user);
        [Authorize]
        [HttpGet("ResetPassword/{id}")]
        public async Task<ActionResult<Return>> ResetPassword(int id) => await _users.ResetPassword(id);
        [Authorize]
        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<Users>>> UserList() => Ok(await _users.ListUsers());

        [Authorize]
        [HttpGet("User/{id}")]
        public async Task<ActionResult<Users>> UserById(int id) => await _users.UserByID(id);
        [HttpPatch("User/Photo/Update")]
        public async Task<ActionResult<Return>> UpdateUser([FromBody] NewPhotoDTO photo)
        {
            var result = await _users.NewPhoto(photo);
            await _hubContext.Clients.All.SendAsync("UpdatePhoto");
            return result;
        }
        [Authorize]
        [HttpGet("User/{id}/Photo")]
        public async Task<ActionResult> Photo(int id)
        {
            var photo = await _users.Photo(id);
            if (photo == null) return NotFound();
            return File(photo, "image/png");
        }
        [Authorize]
        [HttpGet("User/{id}/Signature")]
        public async Task<ActionResult> Signature(int id)
        {
            var signature = await _users.Signature(id);
            if (signature == null) return NotFound();
            return File(signature, "image/png");
        }
        [Authorize]
        [HttpPut("User/Update")]
        public async Task<ActionResult<Return>> UpdateUser([FromBody] Users user)
        {
            var result = await _users.UpdateUser(user);
            await _hubContext.Clients.All.SendAsync("UpdateUser");
            return result;
        }
        [Authorize]
        [HttpPost("SwitchPassword")]
        public async Task<ActionResult<Return>> SwitchPass([FromBody] SwitchPass switchPass) => await _users.SwitchPassword(switchPass);
        #endregion

        #region Department
        [Authorize]
        [HttpPost("Department/NewDepartment")]
        public async Task<ActionResult<Return>> NewDepartment([FromBody] Department department)
        {
            var result = await _users.NewDepartment(department);
            await _hubContext.Clients.All.SendAsync("UpdateDepartment");
            return result;
        }
        [Authorize]
        [HttpGet("Department/List")]
        public async Task<ActionResult<List<Department>>> DepartmentList() => await _users.ListDepartments();
        [Authorize]
        [HttpGet("Department/{id}")]
        public async Task<ActionResult<Department>> DepartmentById(int id) => await _users.DepartmentById(id);
        [Authorize]
        [HttpPut("Department/Update")]
        public async Task<ActionResult<Return>> UpdateDepartment([FromBody] Department department)
        {
            var result = await _users.UpdateDepartment(department);
            await _hubContext.Clients.All.SendAsync("UpdateDepartment");
            return result;
        }
        [Authorize]
        [HttpDelete("Department/Delete/{id}")]
        public async Task<ActionResult<Return>> DeleteDepartment(int id) => await _users.DeleteDepartment(id);
        #endregion

        #region Roles
        [Authorize]
        [HttpPost("Roles/NewRole")]
        public async Task<ActionResult<Return>> NewRole([FromBody] Roles role)
        {
            var result = await _users.NewRole(role);
            await _hubContext.Clients.All.SendAsync("UpdateRole");
            return result;
        }
        [Authorize]
        [HttpGet("Roles/List")]
        public async Task<ActionResult<List<Roles>>> RolesList() => await _users.ListRoles();
        [Authorize]
        [HttpGet("Roles/{id}")]
        public async Task<ActionResult<Roles>> RolesById(int id) => await _users.RoleById(id);
        [Authorize]
        [HttpPut("Roles/Update")]
        public async Task<ActionResult<Return>> UpdateRole([FromBody] Roles role)
        {
            var result = await _users.UpdateRole(role);
            await _hubContext.Clients.All.SendAsync("UpdateRole");
            return result;
        }
        [Authorize]
        [HttpDelete("Roles/Delete/{id}")]
        public async Task<ActionResult<Return>> DeleteRole(int id) => await _users.DeleteRole(id);
        #endregion

        #region Permissions
        [Authorize]
        [HttpPost("Permissions/NewPermission")]
        public async Task<ActionResult<Return>> NewPermission([FromBody] Permissions permission) => await _users.NewPermission(permission);
        [Authorize]
        [HttpPost("Permissions/MultiNewPermission")]
        public async Task<ActionResult<Return>> MultiNewPermission([FromBody] List<Permissions> permissions) => await _users.MultiNewPermission(permissions);
        [Authorize]
        [HttpGet("Permissions/List/{id}")]
        public async Task<ActionResult<List<Permissions>>> ListPermissionsByUser(int id) => await _users.ListPermissionsByUser(id);
        [Authorize]
        [HttpDelete("Permissions/Delete/{id}")]
        public async Task<ActionResult<Return>> DeletePermission(int id) => await _users.DeletePermission(id);
        [Authorize]
        [HttpDelete("Permissions/MultiDelete")]
        public async Task<ActionResult<Return>> MultiDeletePermission([FromBody] List<Permissions> permissions) => await _users.MultiDeletePermission(permissions);
        #endregion
    }
}

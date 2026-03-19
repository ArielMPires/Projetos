using Domus.DTO.Users;
using Domus.Models;
using Domus.Models.DB;
using System.Globalization;

namespace Domus.Repositories.Interfaces
{
    public interface IUsers
    {
        #region Users
        public Task<Return> Log_In(Log_In user);
        public Task<Return> NewUser(CreateUserDTO user);
        public Task<Return> UpdateUser(Users user);
        public Task<IEnumerable<UserDTO>> ListUsers();
        public Task<Users> UserByID(int Userid);
        public Task<Return> ResetPassword(int Id);
        public Task<Byte[]> Photo(int Id);
        public Task<Return> NewPhoto(NewPhotoDTO photo);
        public Task<Byte[]> Signature(int Id);
        public Task<Return> SwitchPassword(SwitchPass switchPass);
        public Task<Return> SwitchTheme(ThemeDTO theme);
        public Task<ThemeIdDTO> ThemeByUser(int id);
        #endregion

        #region Roles
        public Task<Return> NewRole(Roles role);
        public Task<Return> UpdateRole(Roles role);
        public Task<Return> DeleteRole(int RoleId);
        public Task<List<Roles>> ListRoles();
        public Task<Roles> RoleById(int Roleid);
        #endregion

        #region Permissions
        public Task<Return> NewPermission(Permissions permission);
        public Task<Return> MultiNewPermission(List<Permissions> permissions);
        public Task<Return> MultiDeletePermission(List<Permissions> permissions);
        public Task<Return> DeletePermission(int permissionId);
        public Task<List<Permissions>> ListPermissionsByUser(int Permissionid);
        #endregion

        #region Departament
        public Task<Return> NewDepartment(Department department);
        public Task<Return> UpdateDepartment(Department department);
        public Task<Return> DeleteDepartment(int departmentId);
        public Task<List<Department>> ListDepartments();
        public Task<Department> DepartmentById(int departmentId);
        #endregion
    }
}

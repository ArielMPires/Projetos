using NexuSys.DTOs.Departament;
using NexuSys.DTOs.Permissions;
using NexuSys.DTOs.Role;
using NexuSys.DTOs.Seller;
using NexuSys.DTOs.Unit;
using NexuSys.DTOs.Users;
using NexuSys.Entities;

namespace NexuSys.Interfaces
{
    public interface IUsers
    {
        #region Users
        Task<Return> Log_In(Log_In user);
        Task<Return> NewUser(NewUserDTO user);
        Task<Return> UpdateUser(EditUserDTO user);
        Task<Return> DeleteUser(int id);
        Task<List<UserDTO>> UserList();
        Task<ByUserDTO> UserByID(int id);
        #endregion

        #region Role
        Task<Return> NewRole(NewRoleDTO Role);
        Task<Return> UpdateRole(ByRoleDTO Role);
        Task<Return> DeleteRole(int id);
        Task<List<RoleDTO>> RoleList();
        Task<List<PermissionsDTO>> ListarPermissoes(int roleId);
        Task SalvarPermissoes(int roleId, List<PermissionsDTO> permissoes);
        Task<ByRoleDTO> RoleByID(int id);

        #endregion

        #region Seller
        Task<Return> NewSeller(NewSellerDTO Seller);
        Task<Return> UpdateSeller(BySellerDTO Seller);
        Task<Return> DeleteSeller(int id);
        Task<List<SellerDTO>> SellerList();
        Task<BySellerDTO> SellerByID(int id);
        #endregion

        #region Unit
        Task<Return> NewUnit(NewUnitDTO Unit);
        Task<Return> UpdateUnit(ByUnitDTO Unit);
        Task<Return> DeleteUnit(int id);
        Task<List<UnitDTO>> UnitList();
        Task<ByUnitDTO> UnitByID(int id);
        #endregion

        #region Departament
        Task<Return> NewDepartament(NewDepartamentDTO Departament);
        Task<Return> UpdateDepartament(EditDepartamentDTO Departament);
        Task<Return> DeleteDepartament(int id);
        Task<List<DepartamentDTO>> DepartamentList();
        Task<ByDepartamentDTO> DepartamentByID(int id);
        #endregion

        #region Permissions
        Task<ByPermissionsDTO> PermissionByID(int id);
        Task<Return> NewPermissionAll(List<NewPermissionsDTO> Permission);
        Task<Return> NewPermission(NewPermissionsDTO Permission);
        Task<Return> UpdatePermission(EditPermissionsDTO Permission);
        Task<Return> DeletePermissionsAll(List<int> id);
        Task<Return> DeletePermissions(int id);
        Task<List<PermissionsDTO>> PermissionList();
        Task<List<PermissionsDTO>> PermissionByRole(int id);
        #endregion


    }
}

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using NexuSys.Data;
using NexuSys.DTOs.Departament;
using NexuSys.DTOs.Permissions;
using NexuSys.DTOs.Role;
using NexuSys.DTOs.Seller;
using NexuSys.DTOs.Unit;
using NexuSys.DTOs.Users;
using NexuSys.Entities;
using NexuSys.Helper;
using NexuSys.Interfaces;
using System;
using System.Data;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;

namespace NexuSys.Services
{
    public class UsersServices : IUsers
    {
        public readonly ApplicationDbContext _context;
        public readonly IMapper _mapper;

        public UsersServices(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Departament

        public async Task<ByDepartamentDTO> DepartamentByID(int id) =>
            await _context.Department
                .AsNoTracking()
                .ProjectTo<ByDepartamentDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.ID == id);

        public async Task<List<DepartamentDTO>> DepartamentList() =>
            await _context.Department
                .AsNoTracking()
                .ProjectTo<DepartamentDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<Return> NewDepartament(NewDepartamentDTO Departament)
        {
            var result = new Return();
            try
            {
                var departament = _mapper.Map<Department>(Departament);
                var history = departament.historyFK;
                await _context.History.AddAsync(history);
                await _context.SaveChangesAsync();
                departament.History = history.ID;
                departament.historyFK = null;
                await _context.Department.AddAsync(departament);
                await _context.SaveChangesAsync();
                _context.Entry(departament).State = EntityState.Detached;
                _context.Entry(history).State = EntityState.Detached;
                result.Result = true;
                result.Message = "Departamento cadastrado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> UpdateDepartament(EditDepartamentDTO Departament)
        {
            var result = new Return();
            try
            {
                var departament = _mapper.Map<Department>(Departament);
                var history = departament.historyFK;

                var existing = await _context.History.FindAsync(history.ID);
                if (existing == null)
                {
                    result.Result = false;
                    result.Message = "Historico não encontrado.";
                    return result;
                }
                _context.Entry(existing).CurrentValues.SetValues(history);
                await _context.SaveChangesAsync();
                departament.historyFK = null;
                _context.Department.Update(departament);
                await _context.SaveChangesAsync();
                _context.Entry(departament).State = EntityState.Detached;
                result.Result = true;
                result.Message = "Departamento atualizado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> DeleteDepartament(int id)
        {
            var result = new Return();
            try
            {
                var departament = await _context.Department.FirstOrDefaultAsync(e => e.ID == id);
                _context.Department.Remove(departament);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = $"Departamento ID:{id} removido";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        #endregion

        #region Role

        public async Task<ByRoleDTO> RoleByID(int id) =>
            await _context.Role
                .AsNoTracking()
                .ProjectTo<ByRoleDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.ID == id);

        public async Task<List<RoleDTO>> RoleList() =>
            await _context.Role
                .AsNoTracking()
                .ProjectTo<RoleDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<Return> NewRole(NewRoleDTO Role)
        {
            var result = new Return();

            try
            {
                var role = _mapper.Map<Role>(Role);
                var history = role.historyFK;


                role.historyFK = null;
                await _context.History.AddAsync(history);
                await _context.SaveChangesAsync();
                _context.Entry(history).State = EntityState.Detached;

                role.History = history.ID;

                await _context.Role.AddAsync(role);
                await _context.SaveChangesAsync();
                _context.Entry(role).State = EntityState.Detached;
                result.Result = true;
                result.Message = "Função cadastrada com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }

        public async Task<Return> UpdateRole(ByRoleDTO Role)
        {
            var result = new Return();

            try
            {
                var role = _mapper.Map<Role>(Role);
                var history = role.historyFK;

                _context.History.Update(history);
                await _context.SaveChangesAsync();

                role.historyFK = null;
                _context.Role.Update(role);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = "Função atualizada com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
                _context.Entry(Role).State = EntityState.Detached;
            }

            return result;
        }

        public async Task<Return> DeleteRole(int id)
        {
            var result = new Return();

            try
            {
                var role = await _context.Role
                    .FirstOrDefaultAsync(e => e.ID == id);

                if (role != null)
                {
                    _context.Role.Remove(role);
                    await _context.SaveChangesAsync();
                }

                result.Result = true;
                result.Message = $"Função ID:{id} removida";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }

        #endregion

        
        #region Permissions

        public async Task<Return> NewPermissionAll(List<NewPermissionsDTO> Permission)
        {
            var result = new Return();

            try
            {

                foreach (var item in Permission)
                {

                    var permission = _mapper.Map<Permissions>(Permission);

                    await _context.Permissions.AddAsync(permission);
                    await _context.SaveChangesAsync();
                    _context.Entry(permission).State = EntityState.Detached;

                }
                result.Result = true;
                result.Message = "Permissões cadastrada com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }

        public async Task<Return> DeletePermissionsAll(List<int> id)
        {
            var result = new Return();
            try
            {
                foreach (var item in id)
                {
                    var permission = await _context.Permissions.FirstOrDefaultAsync(e => e.ID == item);

                    _context.Permissions.Remove(permission);
                    await _context.SaveChangesAsync();
                    _context.Entry(permission).State = EntityState.Detached;

                }
                result.Result = true;
                result.Message = "Permissões Removidas com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<ByPermissionsDTO> PermissionByID(int id) =>
    await _context.Permissions
        .AsNoTracking()
        .ProjectTo<ByPermissionsDTO>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync(e => e.ID == id);

        public async Task<List<PermissionsDTO>> PermissionByRole(int id) =>
   await _context.Permissions
       .AsNoTracking()
       .Where(e => e.Role == id)
       .ProjectTo<PermissionsDTO>(_mapper.ConfigurationProvider)
       .ToListAsync();

        public async Task<List<PermissionsDTO>> PermissionList() =>
            await _context.Permissions
        .AsNoTracking()
                .ProjectTo<PermissionsDTO>(_mapper.ConfigurationProvider)
        .ToListAsync();

        public async Task<Return> NewPermission(NewPermissionsDTO Permission)
        {
            var result = new Return();

            try
            {
                var permission = _mapper.Map<Permissions>(Permission);




                _context.Entry(permission).State = EntityState.Detached;



                await _context.Permissions.AddAsync(permission);
                await _context.SaveChangesAsync();
                _context.Entry(permission).State = EntityState.Detached;

                result.Result = true;
                result.Message = "Permissão cadastrada com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }

        public async Task<Return> UpdatePermission(EditPermissionsDTO Permission)
        {
            var result = new Return();

            try
            {
                var permission = _mapper.Map<Permissions>(Permission);



                await _context.SaveChangesAsync();



                _context.Permissions.Update(permission);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = "Permissão atualizada com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
                _context.Entry(Permission).State = EntityState.Detached;
            }

            return result;
        }

        public async Task<Return> DeletePermissions(int id)
        {
            var result = new Return();

            try
            {
                var permission = await _context.Permissions
                    .FirstOrDefaultAsync(e => e.ID == id);

                if (permission != null)
                {
                    _context.Permissions.Remove(permission);
                    await _context.SaveChangesAsync();
                }

                result.Result = true;
                result.Message = $"Permissão ID:{id} removida";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }
        #endregion

        #region Seller

        public async Task<BySellerDTO> SellerByID(int id) =>
            await _context.Seller
                .AsNoTracking()
                .ProjectTo<BySellerDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.ID == id);


        public async Task<List<SellerDTO>> SellerList() =>
            await _context.Seller
                .AsNoTracking()
                .ProjectTo<SellerDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();


        public async Task<Return> NewSeller(NewSellerDTO Seller)
        {
            var result = new Return();

            try
            {
                var seller = _mapper.Map<Seller>(Seller);
                var history = seller.historyFK;

                await _context.History.AddAsync(history);
                await _context.SaveChangesAsync();

                seller.History = history.ID;
                seller.historyFK = null;

                await _context.Seller.AddAsync(seller);
                await _context.SaveChangesAsync();
                _context.Entry(seller).State = EntityState.Detached;
                result.Result = true;
                result.Message = "Vendedor cadastrado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }


        public async Task<Return> UpdateSeller(BySellerDTO Seller)
        {
            var result = new Return();

            try
            {
                var seller = _mapper.Map<Seller>(Seller);
                var history = seller.historyFK;

                _context.History.Update(history);
                await _context.SaveChangesAsync();

                seller.historyFK = null;
                _context.Seller.Update(seller);
                await _context.SaveChangesAsync();
                _context.Entry(seller).State = EntityState.Detached;
                result.Result = true;
                result.Message = "Vendedor atualizado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }


        public async Task<Return> DeleteSeller(int id)
        {
            var result = new Return();

            try
            {
                var seller = await _context.Seller
                    .FirstOrDefaultAsync(e => e.ID == id);

                if (seller != null)
                {
                    _context.Seller.Remove(seller);
                    await _context.SaveChangesAsync();
                }

                result.Result = true;
                result.Message = $"Vendedor ID:{id} removido";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }

        #endregion

        #region Unit

        public async Task<ByUnitDTO> UnitByID(int id) =>
            await _context.Unit
                .AsNoTracking()
                .ProjectTo<ByUnitDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.ID == id);

        public async Task<List<UnitDTO>> UnitList() =>
            await _context.Unit
                .AsNoTracking()
                .ProjectTo<UnitDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<Return> NewUnit(NewUnitDTO Unit)
        {
            var result = new Return();
            try
            {
                var unit = _mapper.Map<Unit>(Unit);

                var history = unit.historyFK;

                await _context.History.AddAsync(history);
                await _context.SaveChangesAsync();

                unit.History = history.ID;
                unit.historyFK = null;

                await _context.Unit.AddAsync(unit);
                await _context.SaveChangesAsync();
                _context.Entry(unit).State = EntityState.Detached;

                result.Result = true;
                result.Message = "Unidade cadastrada com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }

        public async Task<Return> UpdateUnit(ByUnitDTO Unit)
        {
            var result = new Return();

            try
            {
                var unit = _mapper.Map<Unit>(Unit);

                _context.Unit.Update(unit);
                await _context.SaveChangesAsync();
                _context.Entry(unit).State = EntityState.Detached;


                result.Result = true;
                result.Message = "Unidade atualizada com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }

        public async Task<Return> DeleteUnit(int id)
        {
            var result = new Return();

            try
            {
                var unit = await _context.Unit
                    .FirstOrDefaultAsync(e => e.ID == id);

                if (unit == null)
                {
                    result.Result = false;
                    result.Message = "Unidade não encontrada";
                    return result;
                }

                _context.Unit.Remove(unit);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = "Unidade removida com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }

        #endregion

        #region Users
        public async Task<Return> Log_In(Log_In user)
        {
            var result = new Return();
            try
            {
                var hash = new Password_Hash(SHA512.Create());

                var currentuser = await _context.Users.FirstOrDefaultAsync(e => e.ID == user.ID);

                result.Result = hash.CompareHash(user.Password, currentuser.Password);
                if (result.Result == true)
                {
                    result.Data = currentuser;
                    result.Message = $"Bem Vindo {currentuser.Name}";
                }
                else
                {
                    result.Message = "Senha ou ID Errado.";
                }

            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException.Message;
            }
            return result;
        }

        public async Task<ByUserDTO> UserByID(int id) =>
            await _context.Users
                .AsNoTracking()
                .ProjectTo<ByUserDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.ID == id);

        public async Task<List<UserDTO>> UserList() =>
            await _context.Users
                .AsNoTracking()
                .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<Return> NewUser(NewUserDTO user)
        {
            var result = new Return();
            try
            {
                var hash = new Password_Hash(SHA512.Create());
                var SHashPass = user.Password;

                user.Password = hash.PasswordEncrypt(user.Password);

                var newUser = _mapper.Map<Users>(user);
                var history = newUser.historyFK;
                await _context.History.AddAsync(history);
                await _context.SaveChangesAsync();
                newUser.History = history.ID;
                newUser.historyFK = null;
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
                _context.Entry(newUser).State = EntityState.Detached;
                result.Result = true;
                result.Message = "Usuário cadastrado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> UpdateUser(EditUserDTO user)
        {
            var result = new Return();
            try
            {
                var updateUser = _mapper.Map<Users>(user);
                _context.Users.Update(updateUser);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = "Usuário atualizado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> DeleteUser(int id)
        {
            var result = new Return();
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(e => e.ID == id);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = $"Usuário ID:{id} removido";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public Task<List<PermissionsDTO>> ListarPermissoes(int roleId)
        {
            throw new NotImplementedException();
        }

        public async Task SalvarPermissoes(int roleId, List<PermissionsDTO> permissoes)
        {
           
            var atuais = _context.Permissions
                .Where(r => r.Role == roleId);

            _context.Permissions.RemoveRange(atuais);

      
            var novasPermissoes = permissoes.Select(p => new Permissions
            {
                Role= roleId,
                Page = p.Page
            }).ToList();

            await _context.Permissions.AddRangeAsync(novasPermissoes);

            await _context.SaveChangesAsync();
        }

        #endregion
    }

}

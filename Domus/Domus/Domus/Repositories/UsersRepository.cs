using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.DataBase;
using Domus.DTO.Users;
using Domus.Models;
using Domus.Models.DB;
using Domus.Repositories.Interfaces;
using Domus.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domus.Repositories
{
    public class UsersRepository : IUsers
    {
        #region Property
        private readonly ApplicationDbContext _context;
        private readonly ITenantDbContextFactory _tenantDbContext;
        private readonly IConfiguration _configuration;
        private readonly TenantProvider _tenantProvider;
        private readonly IMapper _mapper;
        #endregion

        public UsersRepository(IMapper mapper, ApplicationDbContext context, ITenantDbContextFactory tenantcontext, IConfiguration configuration, TenantProvider tentantprovider)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            _tenantDbContext = tenantcontext;
            _tenantProvider = tentantprovider;
            _configuration = configuration;
            _mapper = mapper;
        }

        #region Department
        public async Task<Return> NewDepartment(Department department)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    await context.Department.AddAsync(department);
                    await context.SaveChangesAsync();

                }
                result.Result = true;
                result.Message = $"Departmento Cadastrado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> DeleteDepartment(int departmentId)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    var DpRemove = await context.Department.FirstOrDefaultAsync(e => e.ID == departmentId);
                    context.Department.Remove(DpRemove);
                    await context.SaveChangesAsync();
                }
                result.Result = true;
                result.Message = $"Departmento Deletado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Department> DepartmentById(int departmentId) => await _context.Department.AsNoTracking().FirstOrDefaultAsync(e => e.ID == departmentId);
        public async Task<List<Department>> ListDepartments() => await _context.Department.AsNoTracking().ToListAsync();
        public async Task<Return> UpdateDepartment(Department department)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    context.Department.Update(department);
                    await context.SaveChangesAsync();

                }
                result.Result = true;
                result.Message = $"Departmento Atualizado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        #endregion

        #region Permissions
        public async Task<Return> NewPermission(Permissions permission)
        {
            var result = new Return();
            try
            {
                await _context.Permissions.AddAsync(permission);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = $"Permissão adicionada ao Usuario.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> DeletePermission(int permissionId)
        {
            var result = new Return();
            try
            {
                var DeletePermission = _context.Permissions.FirstOrDefault(x => x.ID == permissionId);
                _context.Permissions.Remove(DeletePermission);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = $"Permissão Deletada Deletado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<List<Permissions>> ListPermissionsByUser(int Permissionid) => await _context.Permissions.AsNoTracking().Where(e => e.User == Permissionid).ToListAsync();
        public async Task<Return> MultiNewPermission(List<Permissions> permissions)
        {
            var result = new Return();
            try
            {
                foreach (var permission in permissions)
                {
                    await _context.Permissions.AddAsync(permission);
                    await _context.SaveChangesAsync();
                }
                result.Result = true;
                result.Message = $"Permissões adicionada ao Usuario.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message + ex.InnerException;
            }
            return result;
        }
        public async Task<Return> MultiDeletePermission(List<Permissions> permissions)
        {
            var result = new Return();
            try
            {
                foreach (var permission in permissions)
                {
                    _context.Permissions.Remove(permission);
                    await _context.SaveChangesAsync();
                }
                result.Result = true;
                result.Message = $"Permissões adicionada ao Usuario.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        #endregion

        #region Roles
        public async Task<Return> NewRole(Roles role)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    await context.Roles.AddAsync(role);
                    await context.SaveChangesAsync();

                }
                result.Result = true;
                result.Message = $"Função criada com Sucesso.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> DeleteRole(int RoleId)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    var RoleDelete = await context.Roles.FirstOrDefaultAsync(e => e.ID == RoleId);
                    context.Roles.Remove(RoleDelete);
                    await context.SaveChangesAsync();
                }
                result.Result = true;
                result.Message = $"Função Deletado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<List<Roles>> ListRoles() => await _context.Roles.AsNoTracking().ToListAsync();
        public async Task<Roles> RoleById(int Roleid) => await _context.Roles.AsNoTracking().FirstOrDefaultAsync(e => e.ID == Roleid);
        public async Task<Return> UpdateRole(Roles role)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    context.Roles.Update(role);
                    await context.SaveChangesAsync();

                }
                result.Result = true;
                result.Message = $"Função Atualizado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        #endregion

        #region Users
        public async Task<Byte[]> Photo(int Id) => await _context.Users.Where(e => e.ID == Id)
            .AsNoTracking()
            .Select(e => e.photo)
            .FirstOrDefaultAsync();
        public async Task<Byte[]> Signature(int Id) => await _context.Users.Where(e => e.ID == Id)
            .AsNoTracking()
            .Select(e => e.Signature)
            .FirstOrDefaultAsync();
        public async Task<ThemeIdDTO> ThemeByUser(int id) => await _context.Theme.Where(e => e.User == id)
            .AsNoTracking()
            .ProjectTo<ThemeIdDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        public async Task<Return> SwitchTheme(ThemeDTO theme)
        {
            var result = new Return();
            try
            {

                var tema = await _context.Theme.FirstOrDefaultAsync(t => t.ID == theme.ID);
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                if (tema == null)
                {
                    // Criar um novo registro
                    foreach (var connectionString in tenantConnections)
                    {
                        var con = connectionString.Value;
                        await using var context = _tenantDbContext.Create(con);

                        var newTheme = _mapper.Map<Theme>(theme);
                        await context.Theme.AddAsync(newTheme);
                        await context.SaveChangesAsync();
                    }

                }
                else
                {
                    foreach (var connectionString in tenantConnections)
                    {
                        var con = connectionString.Value;
                        await using var context = _tenantDbContext.Create(con);
                        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                        var t = await _context.Theme.FirstOrDefaultAsync(t => t.ID == theme.ID);
                        _mapper.Map(theme, t);
                        context.Theme.Update(t);
                        await context.SaveChangesAsync();
                    }
                }

                result.Result = true;
                result.Message = $"Tema Atualizado com Sucesso.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> NewPhoto(NewPhotoDTO photo)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);
                    var user = await context.Users.FirstOrDefaultAsync(e => e.ID == photo.ID);
                    _mapper.Map(photo, user);
                    await _context.SaveChangesAsync();
                }
                result.Result = true;
                result.Message = $"Foto Atualizada com Sucesso.";

            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<IEnumerable<UserDTO>> ListUsers() => await _context.Users
            .AsNoTracking()
            .Include(e => e.RoleFK)
            .Include(e => e.DepartmentFK)
            .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();

        public async Task<Return> Log_In(Log_In user)
        {
            var result = new Return();
            try
            {
                var hash = new Password_Hash(SHA512.Create());

                var currentuser = await _context.Users.FirstOrDefaultAsync(e => e.ID == user.Id);

                result.Result = hash.CompareHash(user.Password, currentuser.Password);
                if (result.Result == true)
                {
                    result.Message = $"Bem Vindo {currentuser.Name}";
                    result.UserToken = BuildToken(user);
                }
                else
                {
                    result.Message = "Senha ou ID Errado.";
                }

            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> NewUser(CreateUserDTO user)
        {
            var result = new Return();
            try
            {
                var hash = new Password_Hash(SHA512.Create());
                var SHashPass = user.Password;

                user.Password = hash.PasswordEncrypt(user.Password);

                var newuser = _mapper.Map<Users>(user);

                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    await context.Users.AddAsync(newuser);
                    await context.SaveChangesAsync();

                }

                var send = new Sender_Email(new SmtpClient(), new MailMessage());

                send.SendNewUser(newuser, SHashPass);

                result.Result = true;
                result.Message = $"Usuario {user.Name} Cadastrado, ID: {newuser.ID}";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message + ex.InnerException;
            }
            return result;
        }
        public async Task<Return> ResetPassword(int Id)
        {
            var result = new Return();
            try
            {
                var Currentuser = await _context.Users.FirstOrDefaultAsync(e => e.ID == Id);
                var rand = new Random();
                var hash = new Password_Hash(SHA512.Create());
                string NewPassword = rand.Next().ToString();

                Currentuser.Password = hash.PasswordEncrypt(NewPassword);

                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    context.Users.Update(Currentuser);
                    await context.SaveChangesAsync();

                }

                var send = new Sender_Email(new SmtpClient(), new MailMessage());

                send.SendReset(Currentuser.Email, NewPassword);

                result.Result = true;
                result.Message = $"Senha Resetada com Sucesso, Senha Nova Enviada para o E-mail Cadastrado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> SwitchPassword(SwitchPass switchPass)
        {
            var result = new Return();
            try
            {
                var hash = new Password_Hash(SHA512.Create());
                var currentuser = await _context.Users.FirstOrDefaultAsync(e => e.ID == switchPass.Id);


                if (!hash.CompareHash(switchPass.CurrentPassword, currentuser.Password))
                    return new Return() { Result = false, Message = "Senha Atual Digitada está errada." };

                currentuser.Password = hash.PasswordEncrypt(switchPass.NewPassword);

                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    context.Users.Update(currentuser);
                    await context.SaveChangesAsync();

                }
                result.Result = true;
                result.Message = $"Senha Alterada com Sucesso.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> UpdateUser(Users user)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    context.Users.Update(user);
                    await context.SaveChangesAsync();

                }

                result.Result = true;
                result.Message = $"Usuario Alterado com Sucesso.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Users> UserByID(int Userid) => await _context.Users
            .AsNoTracking()
            .Include(e => e.RoleFK)
            .Include(e => e.DepartmentFK)
            .FirstOrDefaultAsync(e => e.ID == Userid);
        private UserToken BuildToken(Log_In userInfo)
        {

            var user = _context.Users.Include(e => e.PermissionsOneFK).FirstOrDefaultAsync(e => e.ID == userInfo.Id).Result;
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId,Convert.ToString(user.ID)),
                new Claim(JwtRegisteredClaimNames.Name, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // tempo de expiração do token: 1 hora
            var expiration = DateTime.UtcNow.AddHours(3);
            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);
            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,

            };
        }
    }
    #endregion
}


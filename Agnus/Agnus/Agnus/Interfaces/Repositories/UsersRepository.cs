using Agnus.Models;
using Agnus.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Permissions = Agnus.Models.DB.Permissions;
using Agnus.DTO.Users;

namespace Agnus.Interfaces.Repositories
{
    public class UsersRepository : IUsers
    {
        public readonly HttpClient _httpClient;

        public UsersRepository(IHttpClientFactory http)
        {
            _httpClient = http.CreateClient("API");
        }

        public void SetHeader(string tenantId, string token)
        {
            if (_httpClient.DefaultRequestHeaders.Contains("X-TenantId"))
            {
                _httpClient.DefaultRequestHeaders.Remove("X-TenantId");
            }
            _httpClient.DefaultRequestHeaders.Add("X-TenantId", tenantId);

            if (_httpClient.DefaultRequestHeaders.Contains(Setting.Auth))
            {
                _httpClient.DefaultRequestHeaders.Remove(Setting.Auth);
            }
            _httpClient.DefaultRequestHeaders.Add(Setting.Auth, $"{Setting.Bearer}{token}");
        }

        public void SetTenantHeader(string tenantId)
        {
            if (_httpClient.DefaultRequestHeaders.Contains("X-TenantId"))
            {
                _httpClient.DefaultRequestHeaders.Remove("X-TenantId");
            }
            _httpClient.DefaultRequestHeaders.Add("X-TenantId", tenantId);
        }

        #region Users

        public async Task<byte[]> Photo(int Id)
        {
            var response = await _httpClient.GetAsync($"Users/User/{Id}/Photo");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<Return> NewPhoto(NewPhotoDTO photo)
        {
            try
            {
                var json = JsonConvert.SerializeObject(photo);
                var response = await _httpClient.PatchAsync("Users/User/Photo/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Return>(result);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro de requisição HTTP: {ex.Message}");
                throw;
            }
        }

        public async Task<byte[]> Signature(int Id)
        {
            var response = await _httpClient.GetAsync($"Users/User/{Id}/Signature");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<Return> SwitchTheme(ThemeDTO theme)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Users/User/Theme", theme);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Return>(result);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro de requisição HTTP: {ex.Message}");
                throw;
            }
        }
        public async Task<ThemeIdDTO> ThemeByUser(int id)
        {
            var response = await _httpClient.GetAsync($"Users/User/{id}/Theme");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ThemeIdDTO>(result);
        }
        public async Task<IEnumerable<UserDTO>> ListUsers()
        {
            var response = await _httpClient.GetAsync($"Users/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<UserDTO>>(result);
        }

        public async Task<Return> Log_In(Log_In user)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Users/Log_In", user);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Return>(result);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro de requisição HTTP: {ex.Message}");
                throw;
            }
        }

        public async Task<Return> NewUser(CreateUserDTO user)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Users/NewUser", user);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Return>(result);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro de requisição HTTP: {ex.Message}");
                throw;
            }
        }

        public async Task<Return> ResetPassword(int Id)
        {
            var response = await _httpClient.GetAsync($"Users/ResetPassword/{Id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Return> SwitchPassword(SwitchPass switchPass)
        {
            var response = await _httpClient.PostAsJsonAsync("Users/SwitchPassword", switchPass);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Return> UpdateUser(Users user)
        {
            var json = JsonConvert.SerializeObject(user);
            var response = await _httpClient.PutAsync("Users/User/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Users> UserByID(int Userid)
        {
            var response = await _httpClient.GetAsync($"Users/User/{Userid}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Users>(result);
        }
        #endregion

        #region Roles
        public async Task<Return> DeleteRole(int RoleId)
        {
            var response = await _httpClient.DeleteAsync($"Users/Roles/Delete/{RoleId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<List<Roles>> ListRoles()
        {
            var response = await _httpClient.GetAsync("Users/Roles/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Roles>>(result);
        }

        public async Task<Return> NewRole(Roles role)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Users/Roles/NewRole", role);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Return>(result);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro de requisição HTTP: {ex.Message}");
                throw;
            }
        }

        public async Task<Roles> RoleById(int Roleid)
        {
            var response = await _httpClient.GetAsync($"Users/Roles/{Roleid}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Roles>(result);
        }

        public async Task<Return> UpdateRole(Roles role)
        {
            var json = JsonConvert.SerializeObject(role);
            var response = await _httpClient.PutAsync("Users/Roles/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion

        #region Permissions
        public async Task<Return> DeletePermission(int permissionId)
        {
            var response = await _httpClient.DeleteAsync($"Users/Permissions/Delete/{permissionId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<List<Permissions>> ListPermissionsByUser(int Permissionid)
        {
            var response = await _httpClient.GetAsync($"Users/Permissions/List/{Permissionid}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Permissions>>(result);
        }

        public async Task<Return> MultiDeletePermission(List<Permissions> permissions)
        {
            var response = await _httpClient.DeleteAsync("Users/Permissions/MultiDelete");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Return> MultiNewPermission(List<Permissions> permissions)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Users/Permissions/MultiNewPermission", permissions);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Return>(result);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro de requisição HTTP: {ex.Message}");
                throw;
            }
        }

        public async Task<Return> NewPermission(Permissions permission)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Users/Permissions/NewPermission", permission);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Return>(result);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro de requisição HTTP: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region Department
        public async Task<Return> DeleteDepartment(int departmentId)
        {
            var response = await _httpClient.DeleteAsync($"Users/Department/Delete/{departmentId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Department> DepartmentById(int departmentId)
        {
            var response = await _httpClient.GetAsync($"Users/Department/{departmentId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Department>(result);
        }

        public async Task<List<Department>> ListDepartments()
        {
            var response = await _httpClient.GetAsync("Users/Department/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Department>>(result);
        }

        public async Task<Return> NewDepartment(Department department)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Users/Department/NewDepartment", department);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Return>(result);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro de requisição HTTP: {ex.Message}");
                throw;
            }
        }

        public async Task<Return> UpdateDepartment(Department department)
        {
            var json = JsonConvert.SerializeObject(department);
            var response = await _httpClient.PutAsync("Users/Department/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion
    }
}

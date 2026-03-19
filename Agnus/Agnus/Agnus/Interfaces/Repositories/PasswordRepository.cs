using Agnus.DTO.Passwords;
using Agnus.Models;
using Agnus.Models.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Interfaces.Repositories
{
    public class PasswordRepository : IPassword
    {
        public readonly HttpClient _httpClient;

        public PasswordRepository(IHttpClientFactory http)
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

        #region Password
        public async Task<Return> DeletePassoword(int Delete)
        {
            var response = await _httpClient.DeleteAsync($"Password/Delete/{Delete}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Return> NewPassoword(NewPasswordDTO New)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Password/New", New);
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

        public async Task<List<PasswordDTO>> PasswordsList()
        {
            var response = await _httpClient.GetAsync("Password/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<PasswordDTO>>(result);
        }

        public async Task<PasswordByDTO> Password_SearchBy(int id)
        {
            var response = await _httpClient.GetAsync($"Password/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PasswordByDTO>(result);
        }

        public async Task<Return> UpdatePassoword(EditPasswordDTO Update)
        {
            var json = JsonConvert.SerializeObject(Update);
            var response = await _httpClient.PutAsync("Password/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion

        #region Type Password
        public async Task<Return> DeleteType(int Delete)
        {
            var response = await _httpClient.DeleteAsync($"Password/Type/Delete/{Delete}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Return> NewType(Type_Passwords New)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Password/Type/New", New);
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

        public async Task<List<Type_Passwords>> TypeList()
        {
            var response = await _httpClient.GetAsync("Password/Type/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Type_Passwords>>(result);
        }

        public async Task<Type_Passwords> Type_SearchBy(int id)
        {
            var response = await _httpClient.GetAsync($"Password/Type/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Type_Passwords>(result);
        }

        public async Task<Return> UpdateType(Type_Passwords Update)
        {
            var json = JsonConvert.SerializeObject(Update);
            var response = await _httpClient.PutAsync("Password/Type/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion
    }
}

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
    public class ManualsRepository : IManuals
    {
        public readonly HttpClient _httpClient;

        public ManualsRepository(IHttpClientFactory http)
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

        // adicionado a função delete ...
        public async Task<Return> DeleteManuals(int Delete)
        {
            var response = await _httpClient.DeleteAsync($"Manuals/Delete/{Delete}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        // ... nas linhas acima ^^

        public async Task<List<Manuals>> ListManuals()
        {
            var response = await _httpClient.GetAsync("Manuals/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Manuals>>(result);
        }

        public async Task<Manuals> Manuals_SearchBy(int id)
        {
            var response = await _httpClient.GetAsync($"Manuals/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Manuals>(result);
        }

        public async Task<Return> NewManuals(Manuals New)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Manuals/New", New);
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

        public async Task<Return> UpdateManuals(Manuals Update)
        {
            var json = JsonConvert.SerializeObject(Update);
            var response = await _httpClient.PutAsync("Manuals/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
    }
}

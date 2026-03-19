using Agnus.DTO.Computer;
using Agnus.DTO.Patrimony;
using Agnus.Models;
using Agnus.Models.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Agnus.Interfaces.Repositories
{
    public class PatrimonyRepository : IPatrimony
    {
        public readonly HttpClient _httpClient;

        public PatrimonyRepository(IHttpClientFactory http)
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

        #region Patrimony
        public async Task<Return> DeletePatrimony(int Delete)
        {
            var response = await _httpClient.DeleteAsync($"Patrimony/Delete/{Delete}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Return> NewPatrimony(Patrimony New)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Patrimony/New", New);
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

        public async Task<IEnumerable<PatrimonyDTO>> PatrimonyList()
        {
            var response = await _httpClient.GetAsync($"Patrimony/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<PatrimonyDTO>>(result);
        }

        public async Task<Patrimony> Patrimony_SearchBy(int id)
        {
            var response = await _httpClient.GetAsync($"Patrimony/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Patrimony>(result);
        }

        public async Task<Return> UpdatePatrimony(Patrimony Update)
        {
            var json = JsonConvert.SerializeObject(Update);
            var response = await _httpClient.PutAsync("Patrimony/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion

        #region Category
        public async Task<List<Patrimony_Category>> CategoryList()
        {
            var response = await _httpClient.GetAsync("Patrimony/Category/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Patrimony_Category>>(result);
        }

        public async Task<Patrimony_Category> Category_SearchBy(int id)
        {
            var response = await _httpClient.GetAsync($"Patrimony/Category/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Patrimony_Category>(result);
        }

        public async Task<Return> DeleteCategory(int Delete)
        {
            var response = await _httpClient.DeleteAsync($"Patrimony/Category/Delete/{Delete}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Return> NewCategory(Patrimony_Category New)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Patrimony/Category/New", New);
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

        public async Task<Return> UpdateCategory(Patrimony_Category Update)
        {
            var json = JsonConvert.SerializeObject(Update);
            var response = await _httpClient.PutAsync("Patrimony/Category/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion

        #region Computer
        public async Task<List<Computer>> ComputerList()
        {
            var response = await _httpClient.GetAsync($"Patrimony/Computer/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Computer>>(result);
        }

        public async Task<Computer> Computer_SearchByPc(int id)
        {
            var response = await _httpClient.GetAsync($"Patrimony/Computer/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Computer>(result);
        }
        public async Task<Return> DeleteComputer(int Delete)
        {
            var response = await _httpClient.DeleteAsync($"Patrimony/Computer/Delete/{Delete}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Return> NewComputer(Computer New)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Patrimony/Computer/New", New);
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

        public async Task<Return> UpdateComputer(Computer Update)
        {
            var json = JsonConvert.SerializeObject(Update);
            var response = await _httpClient.PutAsync("Patrimony/Computer/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<IEnumerable<PatrimonyDTO>> PCRegister()
        {
            var response = await _httpClient.GetAsync($"Patrimony/PCRegister");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<PatrimonyDTO>>(result);
        }

        public async  Task<IEnumerable<PCDTO>> ComputerListByOwner(int owner)
        {
            var response = await _httpClient.GetAsync($"Patrimony/Computer/ListByOwner/{owner}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<PCDTO>>(result);
        }
        #endregion
    }
}

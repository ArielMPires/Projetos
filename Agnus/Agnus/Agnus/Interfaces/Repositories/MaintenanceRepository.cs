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

namespace Agnus.Interfaces.Repositories
{
    public class MaintenanceRepository : IMaintenance
    {
        public readonly HttpClient _httpClient;

        public MaintenanceRepository(IHttpClientFactory http)
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

        #region Maintenance
        public async Task<Return> DeleteMaintenance(int MaintenanceDelete)
        {
            var response = await _httpClient.DeleteAsync($"Maintenance/Delete/{MaintenanceDelete}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<List<Maintenance>> MaintenanceList()
        {
            var response = await _httpClient.GetAsync("Maintenance/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Maintenance>>(result);
        }

        public async Task<Maintenance> Maintenance_SearchByPc(int id)
        {
            var response = await _httpClient.GetAsync($"Maintenance/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Maintenance>(result);
        }

        public async Task<Return> NewMaintenance(Maintenance New)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Maintenance/New", New);
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

        public async Task<Return> UpdateMaintenance(Maintenance Update)
        {
            var json = JsonConvert.SerializeObject(Update);
            var response = await _httpClient.PutAsync("Maintenance/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion

        #region Scheduled_Maintenance
        public async Task<Return> DeleteScheduled(int Scheduled_MaintenanceDelete)
        {
            var response = await _httpClient.DeleteAsync($"Maintenance/Scheduled/Delete/{Scheduled_MaintenanceDelete}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Return> NewScheduled(Scheduled_Maintenance New)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Maintenance/Scheduled/New", New);
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

        public async Task<List<Scheduled_Maintenance>> ScheduledList()
        {
            var response = await _httpClient.GetAsync("Maintenance/Scheduled/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Scheduled_Maintenance>>(result);
        }

        public async Task<Scheduled_Maintenance> Scheduled_SearchByPc(int id)
        {
            var response = await _httpClient.GetAsync($"Maintenance/Scheduled/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Scheduled_Maintenance>(result);
        }

        public async Task<Return> UpdateScheduled(Scheduled_Maintenance Update)
        {
            var json = JsonConvert.SerializeObject(Update);
            var response = await _httpClient.PutAsync("Maintenance/Scheduled/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion

        #region Maintenance_CheckList
        public async Task<List<Maintenance_CheckList>> Checklist_List()
        {
            var response = await _httpClient.GetAsync("Maintenance/CheckList/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Maintenance_CheckList>>(result);
        }

        public async Task<Maintenance_CheckList> CheckList_SearchByPc(int id)
        {
            var response = await _httpClient.GetAsync($"Maintenance/CheckList/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Maintenance_CheckList>(result);
        }

        public async Task<Return> Delete_CheckList(int Maintenance_CheckListDelete)
        {
            var response = await _httpClient.DeleteAsync($"Maintenance/CheckList/Delete/{Maintenance_CheckListDelete}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Return> New_CheckList(Maintenance_CheckList NewCheck)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Maintenance/CheckList/New", NewCheck); 
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

        public async Task<Return> Update_CheckList(Maintenance_CheckList Update)
        {
            var json = JsonConvert.SerializeObject(Update);
            var response = await _httpClient.PutAsync("Maintenance/CheckList/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion
    }
}

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
    public class ProjectRepository : IProject
    {
        public readonly HttpClient _httpClient;

        public ProjectRepository(IHttpClientFactory http)
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

        #region Project
        public async Task<Return> DeleteProject(int Delete)
        {
            var response = await _httpClient.DeleteAsync($"Project/Delete/{Delete}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<List<Project>> ListProject()
        {
            var response = await _httpClient.GetAsync("Project/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Project>>(result);
        }

        public async Task<Return> NewProject(Project New)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Project/New", New);
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

        public async Task<Project> Project_SearchById(int id)
        {
            var response = await _httpClient.GetAsync($"Project/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Project>(result);
        }

        public async Task<Return> UpdateProject(Project Update)
        {
            var json = JsonConvert.SerializeObject(Update);
            var response = await _httpClient.PutAsync("Project/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion

        #region Project Products
        public async Task<Return> DeleteProducts(int Delete)
        {
            var response = await _httpClient.DeleteAsync($"Project/Products/Delete/{Delete}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<List<Project_Products>> ListProducts()
        {
            var response = await _httpClient.GetAsync("Project/Products/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Project_Products>>(result);
        }

        public async Task<Return> NewProducts(Project_Products New)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Project/Products/New", New);
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

        public async Task<Project_Products> Products_SearchById(int id)
        {
            var response = await _httpClient.GetAsync($"Project/Products/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Project_Products>(result);
        }

        public async Task<Return> UpdateProducts(Project_Products Update)
        {
            var json = JsonConvert.SerializeObject(Update);
            var response = await _httpClient.PutAsync("Project/Products/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion

        #region Tasks
        public async Task<Return> DeleteTask(int Delete)
        {
            var response = await _httpClient.DeleteAsync($"Project/Tasks/Delete/{Delete}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<List<Tasks>> ListTasks()
        {
            var response = await _httpClient.GetAsync("Project/Tasks/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Tasks>>(result);
        }

        public async Task<Return> NewTask(Tasks New)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Project/Tasks/New", New);
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

        public async Task<Tasks> Task_SearchById(int id)
        {
            var response = await _httpClient.GetAsync($"Project/Tasks/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Tasks>(result);
        }

        public async Task<Return> UpdateTask(Tasks Update)
        {
            var json = JsonConvert.SerializeObject(Update);
            var response = await _httpClient.PutAsync("Project/Tasks/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion
    }
}

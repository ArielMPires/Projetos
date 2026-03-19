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
    public class FilesRepository : IFiles
    {
        public readonly HttpClient _httpClient;

        public FilesRepository(IHttpClientFactory http)
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

        #region Files
        public async Task<Return> DeleteFiles(int DeleteFiles)
        {
            var response = await _httpClient.DeleteAsync($"Files/Delete/{DeleteFiles}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<List<Files>> FilesList()
        {
            var response = await _httpClient.GetAsync("Files/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Files>>(result);
        }

        public async Task<Files> Files_SearchByPc(int id)
        {
            var response = await _httpClient.GetAsync($"Files/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Files>(result);
        }

        public async Task<Return> NewFiles(Files New)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Files/New", New);
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

        public async Task<Return> UpdateFiles(Files UpdateFiles)
        {
            var json = JsonConvert.SerializeObject(UpdateFiles);
            var response = await _httpClient.PutAsync("Files/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Return> NewFilesAllTenant(Files New)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Files/NewAll", New);
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

        public async Task<Return> UpdateFilesAllTenant(Files UpdateFiles)
        {
            var json = JsonConvert.SerializeObject(UpdateFiles);
            var response = await _httpClient.PutAsync("Files/UpdateAll", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion

        #region FileFolder
        public async Task<Return> DeleteFolder(int DeleteFolder)
        {
            var response = await _httpClient.DeleteAsync($"Files/Folder/Delete/{DeleteFolder}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<List<FileFolder>> FolderList()
        {
            var response = await _httpClient.GetAsync("Files/Folder/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<FileFolder>>(result);
        }

        public async Task<FileFolder> Folder_SearchByPc(int id)
        {
            var response = await _httpClient.GetAsync($"Files/Folder/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FileFolder>(result);
        }

        public async Task<Return> NewFolder(FileFolder NewFolder)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Files/Folder/New", NewFolder);
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

        public async Task<Return> UpdateFolder(FileFolder Update)
        {
            var json = JsonConvert.SerializeObject(Update);
            var response = await _httpClient.PutAsync("Files/Folder/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Return> NewFolderAllTenant(FileFolder NewFolder)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Files/Folder/NewAll", NewFolder);
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

        public async Task<Return> UpdateFolderAllTenant(FileFolder Update)
        {
            var json = JsonConvert.SerializeObject(Update);
            var response = await _httpClient.PutAsync("Files/Folder/UpdateAll", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion
    }
}

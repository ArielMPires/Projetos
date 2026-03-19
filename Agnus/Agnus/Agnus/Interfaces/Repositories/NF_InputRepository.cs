using Agnus.DTO.NF;
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
    public class NF_InputRepository : INF_Input
    {
        public readonly HttpClient _httpClient;

        public NF_InputRepository(IHttpClientFactory http)
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

        #region NF_Input
        public async Task<IEnumerable<InputDTO>> InputList()
        {
            var response = await _httpClient.GetAsync("NF_Input/List"); // caminho api ok
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<InputDTO>>(result);
        }

        public async Task<Return> InputNew(NF_Input New)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("NF_Input/New", New); // caminho api ok
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

        public async Task<NF_Input> Input_SearchByNf(int id)
        {
            var response = await _httpClient.GetAsync($"NF_Input/{id}"); // caminho api ok
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<NF_Input>(result);
        }
        #endregion

        #region NF_Output
        public async Task<IEnumerable<OutputDTO>> OutputList()
        {
            var response = await _httpClient.GetAsync("NF_Input/Output/List"); // caminho api ok
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<OutputDTO>>(result);
        }

        public async Task<Return> OutputNew(NF_Output New)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("NF_Input/Output/New", New); // caminho api ok
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

        public async Task<NF_Output> OutputSearchByNf(int id)
        {
            var response = await _httpClient.GetAsync($"NF_Input/Output/{id}"); // caminho api ok
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<NF_Output>(result);
        }
        #endregion
    }
}

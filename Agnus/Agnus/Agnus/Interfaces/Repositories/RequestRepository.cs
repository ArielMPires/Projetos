using Agnus.DTO.Request;
using Agnus.Models;
using Agnus.Models.DB;
using Domus.DTO.Purchase_Order;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Interfaces.Repositories
{
    public class RequestRepository : IRequest
    {

        public readonly HttpClient _httpClient;

        public RequestRepository(IHttpClientFactory http)
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

        #region Request
        public async Task<Return> DeleteRequest(int Delete)
        {
            var response = await _httpClient.DeleteAsync($"Request/Delete/{Delete}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Return> NewRequest(Request New)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Request/New", New);
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

        public async Task<IEnumerable<RequestDTO>> RequestList()
        {
            var response = await _httpClient.GetAsync("Request/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<RequestDTO>>(result);
        }
        public async Task<IEnumerable<RequestDTO>> RequestListPending()
        {
            var response = await _httpClient.GetAsync("Request/ListPending");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<RequestDTO>>(result);
        }

        public async Task<Request> Request_SearchByPc(int id)
        {
            var response = await _httpClient.GetAsync($"Request/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Request>(result);
        }

        public async Task<Return> UpdateRequest(Request Update)
        {
            var json = JsonConvert.SerializeObject(Update);
            var response = await _httpClient.PutAsync("Request/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion

        #region Approval
        public async Task<Request_Approval> Approval_SearchByPc(int id)
        {
            var response = await _httpClient.GetAsync($"Request/Aprroval/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Request_Approval>(result);
        }

        public async Task<List<Request_Approval>> AprrovalList()
        {
            var response = await _httpClient.GetAsync("Request/Approval/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Request_Approval>>(result);
        }

        public async Task<Return> DeleteAprroval(int Delete)
        {
            var response = await _httpClient.DeleteAsync($"Request/Approval/Delete/{Delete}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Return> NewAprroval(Request New)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Request/Approval/New", New);
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

        public async Task<Return> UpdateAprroval(Request_Approval Update)
        {
            var json = JsonConvert.SerializeObject(Update);
            var response = await _httpClient.PutAsync("Request/Approval/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion

        #region Items
        public async Task<Return> DeleteItems(int Delete)
        {
            var response = await _httpClient.DeleteAsync($"Request/Items/Delete/{Delete}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<List<Request_Items>> ItemsList()
        {
            var response = await _httpClient.GetAsync("Request/Items/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Request_Items>>(result);
        }

        public async Task<Request_Items> Items_SearchByPc(int id)
        {
            var response = await _httpClient.GetAsync($"Request/Items/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Request_Items>(result);
        }

        public async Task<Return> New_Items(Request_Items New)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Request/Items/New", New);
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

        public async Task<Return> UpdateItems(Request_Items Update)
        {
            var json = JsonConvert.SerializeObject(Update);
            var response = await _httpClient.PutAsync("Request/Items/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion

        #region Usage
        public async Task<Return> DeleteUsage(int Delete)
        {
            var response = await _httpClient.DeleteAsync($"Request/Usage/Delete/{Delete}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Return> New_Usage(Request_Usage New)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Request/Usage/New", New);
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

        public async Task<Return> UpdateUsage(Request_Usage Update)
        {
            var json = JsonConvert.SerializeObject(Update);
            var response = await _httpClient.PutAsync("Request/Usage/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<List<Request_Usage>> UsageList()
        {
            var response = await _httpClient.GetAsync("Request/Usage/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Request_Usage>>(result);
        }

        public async Task<Request_Usage> Usage_SearchByPc(int id)
        {
            var response = await _httpClient.GetAsync($"Request/Usage/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Request_Usage>(result);
        }
        #endregion

        #region Order
        public async Task<Return> DeleteOrder(int Delete)
        {
            var response = await _httpClient.DeleteAsync($"Request/Order/Delete/{Delete}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Return> New_Order(Purchase_Order New)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Request/Order/New", New);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Return>(result);
            }
            catch (HttpRequestException ex) 
            {
                Console.WriteLine($"Erro de requisição Http: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Purchase_OrderDTO>> PurchaseList()
        {
            var response = await _httpClient.GetAsync("Request/Order/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Purchase_OrderDTO>>(result);
        }
        public async Task<IEnumerable<Purchase_OrderDTO>> PurchaseListDelivered()
        {
            var response = await _httpClient.GetAsync("Request/Order/ListPending");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Purchase_OrderDTO>>(result);
        }

        public async Task<Purchase_Order> Purchase_SearchByPc(int id)
        {
            var response = await _httpClient.GetAsync($"Request/Order/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Purchase_Order>(result);
        }

        public async Task<Return> UpdateOrder(Purchase_Order Update)
        {
            var json = JsonConvert.SerializeObject(Update);
            var response = await _httpClient.PutAsync("Request/Order/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion
    }
}


using Agnus.DTO.Service_Order;
using Agnus.Models;
using Agnus.Models.DB;
using Domus.DTO.Service_Order;
using Domus.DTO.Service_Type;
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
    public class ServiceOrderRepository : IService_Order
    {
        public HttpClient _httpClient; 
        public ServiceOrderRepository(IHttpClientFactory http)
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
            _httpClient.DefaultRequestHeaders.Add(Setting.Auth,$"{Setting.Bearer}{token}");
        }

        #region Service Order
        public async Task<Return> EndOrder(int id, EndOrderDTO order)
        {
            var json = JsonConvert.SerializeObject(order);
            var response = await _httpClient.PatchAsync($"ServiceOrder/EndOrder/{id}", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        public async Task<Return> CatchOrder(int id,CatchOrderDTO order)
        {
            var json = JsonConvert.SerializeObject(order);
            var response = await _httpClient.PatchAsync($"ServiceOrder/Order/{id}", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        public async Task<Return> ContactOrder(int id)
        {
            var response = await _httpClient.PatchAsJsonAsync($"ServiceOrder/Contact/{id}", new { });
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<IEnumerable<ServiceOrderDTO>> ListAllOrder()
        {
            var response = await _httpClient.GetAsync("ServiceOrder/ListAll");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ServiceOrderDTO>>(result);
        }

        public async Task<IEnumerable<ServiceOrderDTO>> ListPendingOrder()
        {
            var response = await _httpClient.GetAsync("ServiceOrder/ListPending");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ServiceOrderDTO>>(result);
        }

        public async Task<IEnumerable<ServiceOrderDTO>> ListTechnicalOrder(int id)
        {
            var response = await _httpClient.GetAsync($"ServiceOrder/ListByTechnical/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ServiceOrderDTO>>(result);
        }

        public async Task<Return> NewOrder(Service_Order order)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("ServiceOrder/New", order);
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

        public async Task<Service_Order> Order(int id)
        {
            var response = await _httpClient.GetAsync($"ServiceOrder/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Service_Order>(result);
        }

        public async Task<Return> UpdateOrder(Service_Order order)
        {
            var json = JsonConvert.SerializeObject(order);
            var response = await _httpClient.PutAsync("ServiceOrder/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion

        #region Service Items
        public async Task<Service_Items> ItemById(int id)
        {
            var response = await _httpClient.GetAsync($"ServiceOrder/Items/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Service_Items>(result);
        }

        public async Task<List<Service_Items>> ListAllItemsByOrder(int order)
        {
            var response = await _httpClient.GetAsync($"ServiceOrder/Items/ListByOrder/{order}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Service_Items>>(result);
        }

        public async Task<Return> NewItemOrder(Service_Items item)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("ServiceOrder/Items/New", item);
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

        public async Task<Return> UpdateItem(Service_Items item)
        {
            var json = JsonConvert.SerializeObject(item);
            var response = await _httpClient.PutAsync("ServiceOrder/Items/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Return> DeleteItem(int id)
        {
            var response = await _httpClient.DeleteAsync($"ServiceOrder/Items/Delete/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion

        #region Service Type
        public async Task<IEnumerable<TypeDTO>> ListAllType()
        {
            var response = await _httpClient.GetAsync("ServiceOrder/Type/list");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<TypeDTO>>(result);
        }
        public async Task<IEnumerable<TypeDTO>> ListAllTypeByCategory(int id)
        {
            var response = await _httpClient.GetAsync($"ServiceOrder/Type/list/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<TypeDTO>>(result);
        }

        public async Task<Return> NewType(Service_Type type)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("ServiceOrder/Type/New", type);
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

        public async Task<Service_Type> TypeById(int typyId)
        {
            var response = await _httpClient.GetAsync($"ServiceOrder/Type/{typyId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Service_Type>(result);
        }

        public async Task<Return> UpdateType(Service_Type type)
        {
            var json = JsonConvert.SerializeObject(type);
            var response = await _httpClient.PutAsync("ServiceOrder/Type/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Return> DeleteType(int typyId)
        {
            var response = await _httpClient.DeleteAsync($"ServiceOrder/Type/Delete/{typyId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion

        #region Service Category
        public async Task<Service_Category> CategoryById(int categoryId)
        {
            var response = await _httpClient.GetAsync($"ServiceOrder/Category/{categoryId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Service_Category>(result);
        }

        public async Task<List<Service_Category>> ListAllCategory()
        {
            var response = await _httpClient.GetAsync("ServiceOrder/Category/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Service_Category>>(result);
        }

        public async Task<Return> NewCategory(Service_Category category)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("ServiceOrder/Category/New", category);
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

        public async Task<Return> UpdateCategory(Service_Category category)
        {
            var json = JsonConvert.SerializeObject(category);
            var response = await _httpClient.PutAsync("ServiceOrder/Category/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Return> DeleteCategory(int categoryId)
        {
            var response = await _httpClient.DeleteAsync($"ServiceOrder/Category/Delete/{categoryId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion

        #region Service Execute
        public async Task<Service_Execute> ExecuteById(int executeId)
        {
            var response = await _httpClient.GetAsync($"ServiceOrder/Execute/{executeId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Service_Execute>(result);
        }

        public async Task<List<Service_Execute>> ListAllExecute()
        {
            var response = await _httpClient.GetAsync("ServiceOrder/Execute/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Service_Execute>>(result);
        }

        public async Task<Return> NewExecute(Service_Execute execute)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("ServiceOrder/Execute/New", execute);
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

        public async Task<Return> UpdateExecute(Service_Execute execute)
        {
            var json = JsonConvert.SerializeObject(execute);
            var response = await _httpClient.PutAsync("ServiceOrder/Execute/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Return> DeleteExecute(int executeId)
        {
            var response = await _httpClient.DeleteAsync($"ServiceOrder/Execute/Delete/{executeId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<List<Service_Execute>> ListAllServiceByOrder(int order)
        {
            var response = await _httpClient.GetAsync($"ServiceOrder/Execute/ListByOrder/{order}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Service_Execute>>(result);
        }
        #endregion

        #region Services
        public async Task<List<Services>> ListAllService()
        {
            var response = await _httpClient.GetAsync("ServiceOrder/Services/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Services>>(result);
        }

        public async Task<Return> NewService(Services service)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("ServiceOrder/Services/New", service);
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

        public async Task<Services> ServiceById(int serviceId)
        {
            var response = await _httpClient.GetAsync($"ServiceOrder/Services/{serviceId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Services>(result);
        }

        public async Task<Return> UpdateService(Services service)
        {
            var json = JsonConvert.SerializeObject(service);
            var response = await _httpClient.PutAsync("ServiceOrder/Services/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Return> DeleteService(int serviceId)
        {
            var response = await _httpClient.DeleteAsync($"ServiceOrder/Services/Delete/{serviceId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion

        #region Service Checklist
        public async Task<Service_CheckList> ChecklistById(int checklistId)
        {
            var response = await _httpClient.GetAsync($"ServiceOrder/ExecuChecklistte/{checklistId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Service_CheckList>(result);
        }

        public async Task<List<Service_CheckList>> ListAllChecklist()
        {
            var response = await _httpClient.GetAsync("ServiceOrder/Checklist/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Service_CheckList>>(result);
        }

        public async Task<Return> NewChecklist(Service_CheckList checkList)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("ServiceOrder/Checklist/New", checkList);
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

        public async Task<Return> UpdateChecklist(Service_CheckList checkList)
        {
            var json = JsonConvert.SerializeObject(checkList);
            var response = await _httpClient.PutAsync("ServiceOrder/Checklist/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Return> DeleteChecklist(int checklistId)
        {
            var response = await _httpClient.DeleteAsync($"ServiceOrder/Checklist/Delete/{checklistId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<List<Service_CheckList>> ListAllCheckByOrder(int order)
        {
            var response = await _httpClient.GetAsync($"ServiceOrder/Checklist/ListByOrder/{order}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Service_CheckList>>(result);
        }
        #endregion
    }
}

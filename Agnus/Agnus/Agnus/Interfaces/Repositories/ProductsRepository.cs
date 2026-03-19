using Agnus.DTO.Products;
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
    public class ProductsRepository : IProducts
    {
        public readonly HttpClient _httpClient;

        public ProductsRepository(IHttpClientFactory http)
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

        #region Products
        public async Task<Return> DeleteProduct(int productId)
        {
            var response = await _httpClient.DeleteAsync($"Products/Delete/{productId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<IEnumerable<ProductsDTO>> ListProduct()
        {
            var response = await _httpClient.GetAsync($"Products/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ProductsDTO>>(result);
        }

        public async Task<Return> NewProduct(Products product)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Products/New", product);
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

        public async Task<Products> ProductById(int Id)
        {
            var response = await _httpClient.GetAsync($"Products/Product/{Id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Products>(result);
        }

        public async Task<Return> UpdateProduct(Products product)
        {
            var json = JsonConvert.SerializeObject(product);
            var response = await _httpClient.PutAsync("Products/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<List<Products>> ReplacementList()
        {
            var response = await _httpClient.GetAsync("Products/Stock/Replacement");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Products>>(result);
        }
        #endregion

        #region Suppliers
        public async Task<Return> DeleteSupplier(int supplierId)
        {
            var response = await _httpClient.DeleteAsync($"Products/Suppliers/Delete/{supplierId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<List<Suppliers>> ListSupplier()
        {
            var response = await _httpClient.GetAsync("Products/Suppliers/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Suppliers>>(result);
        }

        public async Task<Return> NewSupplier(Suppliers supplier)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Products/Suppliers/New", supplier);
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

        public async Task<Suppliers> SupplierById(int Id)
        {
            var response = await _httpClient.GetAsync($"Products/Suppliers/{Id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Suppliers>(result);
        }

        public async Task<Return> UpdateSupplier(Suppliers supplier)
        {
            var json = JsonConvert.SerializeObject(supplier);
            var response = await _httpClient.PutAsync("Products/Suppliers/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion

        #region Product Supplier
        public async Task<Return> DeleteProductSupplier(int productsupplierId)
        {
            var response = await _httpClient.DeleteAsync($"Products/ProductSupplier/Delete/{productsupplierId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<List<Product_Supplier>> ListProductSupplier()
        {
            var response = await _httpClient.GetAsync("Products/ProductSupplier/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Product_Supplier>>(result);
        }

        public async Task<List<Product_Supplier>> ListProductSupplierByProduct(int productsupplier)
        {
            var response = await _httpClient.GetAsync($"Products/ProductSupplier/List/{productsupplier}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Product_Supplier>>(result);
        }

        public async Task<Return> NewProductSupplier(Product_Supplier productsupplier)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Products/ProductSupplier/New", productsupplier);
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

        public async Task<Product_Supplier> ProductSupplierById(int Id)
        {
            var response = await _httpClient.GetAsync($"Products/ProductSupplier/{Id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Product_Supplier>(result);
        }

        public async Task<Return> UpdateProductSupplier(Product_Supplier productsupplier)
        {
            var json = JsonConvert.SerializeObject(productsupplier);
            var response = await _httpClient.PutAsync("Products/ProductSupplier/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion

        #region Product Category
        public async Task<Return> DeleteProductCategory(int productCategoryId)
        {
            var response = await _httpClient.DeleteAsync($"Products/ProductCategory/Delete/{productCategoryId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<List<Product_Category>> ListProductCategory()
        {
            var response = await _httpClient.GetAsync("Products/ProductCategory/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Product_Category>>(result);
        }

        public async Task<Return> NewProductCategory(Product_Category product_Category)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Products/ProductCategory/New", product_Category);
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

        public async Task<Product_Category> ProductCategoryById(int Id)
        {
            var response = await _httpClient.GetAsync($"Products/ProductCategory/{Id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Product_Category>(result);
        }

        public async Task<Return> UpdateProductCategory(Product_Category product_Category)
        {
            var json = JsonConvert.SerializeObject(product_Category);
            var response = await _httpClient.PutAsync("Products/ProductCategory/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion

        #region Brands
        public async Task<Brands> BrandsById(int Id)
        {
            var response = await _httpClient.GetAsync($"Products/Brands/{Id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Brands>(result);
        }

        public async Task<Return> DeleteBrands(int brandsId)
        {
            var response = await _httpClient.DeleteAsync($"Products/Brands/Delete/{brandsId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<List<Brands>> ListBrands()
        {
            var response = await _httpClient.GetAsync("Products/Brands/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Brands>>(result);
        }

        public async Task<Return> NewBrands(Brands brands)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Products/Brands/New", brands);
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

        public async Task<Return> UpdateBrands(Brands brands)
        {
            var json = JsonConvert.SerializeObject(brands);
            var response = await _httpClient.PutAsync("Products/Brands/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion

        #region Stock
        public async Task<List<Stock>> ListStock()
        {
            var response = await _httpClient.GetAsync("Products/Stock/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Stock>>(result);
        }

        public async Task<Return> NewStock(Stock stock)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Products/Stock/New", stock);
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

        public async Task<Stock> StockById(int Id)
        {
            var response = await _httpClient.GetAsync($"Products/Stock/{Id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Stock>(result);
        }

        public async Task<Return> UpdateStock(Stock stock)
        {
            var json = JsonConvert.SerializeObject(stock);
            var response = await _httpClient.PutAsync("Products/Stock/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }
        #endregion

        #region Virtual Stock
        public async Task<Return> DeleteVirtualStock(int virtualStockId)
        {
            var response = await _httpClient.DeleteAsync($"Products/VirtualStock/Delete/{virtualStockId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<List<Virtual_Stock>> ListVirtualStock()
        {
            var response = await _httpClient.GetAsync("Products/VirtualStock/List");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Virtual_Stock>>(result);
        }

        public async Task<List<Virtual_Stock>> ListVirtualStockByOrder(int Id)
        {
            var response = await _httpClient.GetAsync($"Products/VirtualStock/Order/{Id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Virtual_Stock>>(result);
        }

        public async Task<Return> NewVirtualStock(Virtual_Stock virtual_Stock)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Products/VirtualStock/New", virtual_Stock);
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

        public async Task<Return> UpdateVirtualStock(Virtual_Stock virtual_Stock)
        {
            var json = JsonConvert.SerializeObject(virtual_Stock);
            var response = await _httpClient.PutAsync("Products/VirtualStock/Update", new StringContent(json, Encoding.UTF8, Setting.Header));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Return>(result);
        }

        public async Task<Virtual_Stock> VirtualStockById(int Id)
        {
            var response = await _httpClient.GetAsync($"Products/VirtualStock/{Id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Virtual_Stock>(result);
        }
        #endregion
    }
}

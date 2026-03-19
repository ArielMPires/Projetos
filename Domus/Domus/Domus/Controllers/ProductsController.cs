using Domus.DTO.Products;
using Domus.Models;
using Domus.Models.DB;
using Domus.Repositories.Interfaces;
using Domus.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Domus.Controllers
{
    [Route("Domus/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        #region Property
        public readonly IProducts _products;
        private readonly IHubContext<NotificationHub> _hubContext;
        #endregion

        public ProductsController(IProducts products, IHubContext<NotificationHub> hubContext)
        {
            _products = products;
            _hubContext = hubContext;
        }

        #region Products
        [Authorize]
        [HttpPost("New")]
        public async Task<ActionResult<Return>> NewProduct([FromBody] Products product)
        {
            var result = await _products.NewProduct(product);
            await _hubContext.Clients.All.SendAsync("UpdateProducts");
            return result;
        }
        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<Return>> DeleteProduct(int id) => await _products.DeleteProduct(id);
        [Authorize]
        [HttpPut("Update")]
        public async Task<ActionResult<Return>> UpdateProduct([FromBody] Products product)
        {
            var result = await _products.UpdateProduct(product);
            await _hubContext.Clients.All.SendAsync("UpdateProducts");
            return result;
        }
        [Authorize]
        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<ProductsDTO>>> ProductList() => Ok(await _products.ListProduct());
        [Authorize]
        [HttpGet("Stock/Replacement")]
        public async Task<ActionResult<List<Products>>> ReplacementList() => await _products.ReplacementList();
        [Authorize]
        [HttpGet("Product/{id}")]
        public async Task<ActionResult<Products>> ProductById(int id) => await _products.ProductById(id);
        #endregion

        #region Suppliers
        [Authorize]
        [HttpPost("Suppliers/New")]
        public async Task<ActionResult<Return>> NewSupplier([FromBody] Suppliers supplier)
        {
            var result = await _products.NewSupplier(supplier);
            await _hubContext.Clients.All.SendAsync("UpdateSupplier");
            return result;
        }
        [Authorize]
        [HttpDelete("Suppliers/Delete/{id}")]
        public async Task<ActionResult<Return>> DeleteSupplier(int id) => await _products.DeleteSupplier(id);
        [Authorize]
        [HttpPut("Suppliers/Update")]
        public async Task<ActionResult<Return>> UpdateSupplier([FromBody] Suppliers supplier)
        {
            var result = await _products.UpdateSupplier(supplier);
            await _hubContext.Clients.All.SendAsync("UpdateSupplier");
            return result;
        }
        [Authorize]
        [HttpGet("Suppliers/List")]
        public async Task<ActionResult<List<Suppliers>>> SupplierList() => await _products.ListSupplier();
        [Authorize]
        [HttpGet("Suppliers/{id}")]
        public async Task<ActionResult<Suppliers>> SupplierById(int id) => await _products.SupplierById(id);
        #endregion

        #region Product Supplier
        [Authorize]
        [HttpPost("ProductSupplier/New")]
        public async Task<ActionResult<Return>> NewProductSupplier([FromBody] Product_Supplier product)
        {
            var result = await _products.NewProductSupplier(product);
            await _hubContext.Clients.All.SendAsync("UpdatePS");
            return result;
        }
        [Authorize]
        [HttpDelete("ProductSupplier/Delete/{id}")]
        public async Task<ActionResult<Return>> DeleteProductSupplier(int id) => await _products.DeleteProductSupplier(id);
        [Authorize]
        [HttpPut("ProductSupplier/Update")]
        public async Task<ActionResult<Return>> UpdateProductSupplier([FromBody] Product_Supplier product)
        {
            var result = await _products.UpdateProductSupplier(product);
            await _hubContext.Clients.All.SendAsync("UpdatePS");
            return result;
        }
        [Authorize]
        [HttpGet("ProductSupplier/List")]
        public async Task<ActionResult<List<Product_Supplier>>> ProductSupplierList() => await _products.ListProductSupplier();
        [Authorize]
        [HttpGet("ProductSupplier/List/{id}")]
        public async Task<ActionResult<List<Product_Supplier>>> ProductSupplierListBy(int id) => await _products.ListProductSupplierByProduct(id);
        [Authorize]
        [HttpGet("ProductSupplier/{id}")]
        public async Task<ActionResult<Product_Supplier>> ProductSupplierById(int id) => await _products.ProductSupplierById(id);
        #endregion

        #region Product Category
        [Authorize]
        [HttpPost("ProductCategory/New")]
        public async Task<ActionResult<Return>> NewProductCategory([FromBody] Product_Category product)
        {
            var result = await _products.NewProductCategory(product);
            await _hubContext.Clients.All.SendAsync("UpdatePCategory");
            return result;
        }
        [Authorize]
        [HttpDelete("ProductCategory/Delete/{id}")]
        public async Task<ActionResult<Return>> DeleteProductCategory(int id) => await _products.DeleteProductCategory(id);
        [Authorize]
        [HttpPut("ProductCategory/Update")]
        public async Task<ActionResult<Return>> UpdateProductCategory([FromBody] Product_Category product)
        {
            var result = await _products.UpdateProductCategory(product);
            await _hubContext.Clients.All.SendAsync("UpdatePCategory");
            return result;
        }
        [Authorize]
        [HttpGet("ProductCategory/List")]
        public async Task<ActionResult<List<Product_Category>>> ProductCategoryList() => await _products.ListProductCategory();
        [Authorize]
        [HttpGet("ProductCategory/{id}")]
        public async Task<ActionResult<Product_Category>> ProductCategoryById(int id) => await _products.ProductCategoryById(id);
        #endregion

        #region Brands
        [Authorize]
        [HttpPost("Brands/New")]
        public async Task<ActionResult<Return>> NewBrands([FromBody] Brands brands)
        {
            var result = await _products.NewBrands(brands);
            await _hubContext.Clients.All.SendAsync("UpdateBrands");
            return result;
        }
        [Authorize]
        [HttpDelete("Brands/Delete/{id}")]
        public async Task<ActionResult<Return>> DeleteBrands(int id) => await _products.DeleteBrands(id);
        [Authorize]
        [HttpPut("Brands/Update")]
        public async Task<ActionResult<Return>> UpdateBrands([FromBody] Brands brands)
        {
            var result = await _products.UpdateBrands(brands);
            await _hubContext.Clients.All.SendAsync("UpdateBrands");
            return result;
        }
        [Authorize]
        [HttpGet("Brands/List")]
        public async Task<ActionResult<List<Brands>>> BrandsList() => await _products.ListBrands();
        [Authorize]
        [HttpGet("Brands/{id}")]
        public async Task<ActionResult<Brands>> BrandsById(int id) => await _products.BrandsById(id);
        #endregion

        #region Stock
        [Authorize]
        [HttpPost("Stock/New")]
        public async Task<ActionResult<Return>> NewStock([FromBody] Stock stock) => await _products.NewStock(stock);
        [Authorize]
        [HttpPut("Stock/Update")]
        public async Task<ActionResult<Return>> UpdateStock([FromBody] Stock stock) => await _products.UpdateStock(stock);
        [Authorize]
        [HttpGet("Stock/List")]
        public async Task<ActionResult<List<Stock>>> StockList() => await _products.ListStock();
        [Authorize]
        [HttpGet("Stock/{id}")]
        public async Task<ActionResult<Stock>> StockById(int id) => await _products.StockById(id);
        #endregion

        #region Virtual Stock
        [Authorize]
        [HttpPost("VirtualStock/New")]
        public async Task<ActionResult<Return>> NewVirtualStock([FromBody] Virtual_Stock vStock) => await _products.NewVirtualStock(vStock);
        [Authorize]
        [HttpDelete("VirtualStock/Delete/{id}")]
        public async Task<ActionResult<Return>> DeleteVirtualStock(int id) => await _products.DeleteVirtualStock(id);
        [Authorize]
        [HttpPut("VirtualStock/Update")]
        public async Task<ActionResult<Return>> UpdateVirtualStock([FromBody] Virtual_Stock vStock) => await _products.UpdateVirtualStock(vStock);
        [Authorize]
        [HttpGet("VirtualStock/List")]
        public async Task<ActionResult<List<Virtual_Stock>>> VirtualStockList() => await _products.ListVirtualStock();
        [Authorize]
        [HttpGet("VirtualStock/{id}")]
        public async Task<ActionResult<Virtual_Stock>> VirtualStockById(int id) => await _products.VirtualStockById(id);

        #endregion
    }
}

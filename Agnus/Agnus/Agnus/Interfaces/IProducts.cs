using Agnus.DTO.Products;
using Agnus.Models;
using Agnus.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Interfaces
{
    public interface IProducts
    {
       public void SetHeader(string tenantId, string token);
        public void SetTenantHeader(string tenantId);

        #region Products
        public Task<Return> NewProduct(Products product);
        public Task<Return> UpdateProduct(Products product);
        public Task<Return> DeleteProduct(int productId);
        public Task<IEnumerable<ProductsDTO>> ListProduct();
        public Task<Products> ProductById(int Id);
        public Task<List<Products>> ReplacementList();
        #endregion

        #region Suppliers
        public Task<Return> NewSupplier(Suppliers supplier);
        public Task<Return> UpdateSupplier(Suppliers supplier);
        public Task<Return> DeleteSupplier(int supplierId);
        public Task<List<Suppliers>> ListSupplier();
        public Task<Suppliers> SupplierById(int Id);
        #endregion

        #region Product Supplier
        public Task<Return> NewProductSupplier(Product_Supplier productsupplier);
        public Task<Return> UpdateProductSupplier(Product_Supplier productsupplier);
        public Task<Return> DeleteProductSupplier(int productsupplierId);
        public Task<List<Product_Supplier>> ListProductSupplier();
        public Task<List<Product_Supplier>> ListProductSupplierByProduct(int productsupplier);
        public Task<Product_Supplier> ProductSupplierById(int Id);
        #endregion

        #region Product Category
        public Task<Return> NewProductCategory(Product_Category product_Category);
        public Task<Return> UpdateProductCategory(Product_Category product_Category);
        public Task<Return> DeleteProductCategory(int productCategoryId);
        public Task<List<Product_Category>> ListProductCategory();
        public Task<Product_Category> ProductCategoryById(int Id);
        #endregion

        #region Brands
        public Task<Return> NewBrands(Brands brands);
        public Task<Return> UpdateBrands(Brands brands);
        public Task<Return> DeleteBrands(int brandsId);
        public Task<List<Brands>> ListBrands();
        public Task<Brands> BrandsById(int Id);
        #endregion

        #region Stock
        public Task<Return> NewStock(Stock stock);
        public Task<Return> UpdateStock(Stock stock);
        public Task<List<Stock>> ListStock();
        public Task<Stock> StockById(int Id);
        #endregion

        #region Virtual Stock
        public Task<Return> NewVirtualStock(Virtual_Stock virtual_Stock);
        public Task<Return> UpdateVirtualStock(Virtual_Stock virtual_Stock);
        public Task<Return> DeleteVirtualStock(int virtualStockId);
        public Task<List<Virtual_Stock>> ListVirtualStock();
        public Task<List<Virtual_Stock>> ListVirtualStockByOrder(int Id);
        public Task<Virtual_Stock> VirtualStockById(int Id);
        #endregion
    }
}

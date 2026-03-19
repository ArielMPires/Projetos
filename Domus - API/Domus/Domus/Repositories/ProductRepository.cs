using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.DataBase;
using Domus.DTO.Products;
using Domus.Models;
using Domus.Models.DB;
using Domus.Repositories.Interfaces;
using Domus.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security;

namespace Domus.Repositories
{
    public class ProductRepository : IProducts
    {
        #region Property
        public readonly ApplicationDbContext Context;
        private readonly ITenantDbContextFactory _tenantDbContext;
        private readonly TenantProvider _tenantProvider;
        private readonly IMapper _mapper;
        #endregion

        public ProductRepository(ApplicationDbContext context, ITenantDbContextFactory tenantcontext, TenantProvider tentantprovider,IMapper mapper)
        {
            Context = context;
            _tenantDbContext = tenantcontext;
            _tenantProvider = tentantprovider;
            _mapper = mapper;
        }

        #region Products
        public async Task<List<Products>> ReplacementList() => await Context.Products.Where(e => e.Minimum_Stock >= e.ProductsFK.Amount)
             .Include(e => e.ProductsFK)
             .Include(e => e.CategoryFK)
             .Include(e => e.MarkFK)
             .Include(e => e.ProductSupplierFK)
             .ToListAsync();
        public async Task<Return> DeleteProduct(int productId)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    var productRemove = await context.Products.FirstOrDefaultAsync(e => e.ID == productId);
                    context.Products.Remove(productRemove);
                    await context.SaveChangesAsync();
                }
                result.Result = true;
                result.Message = $"Produto Deletado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<IEnumerable<ProductsDTO>> ListProduct() => await Context.Products
            .AsNoTracking()
            .Include(e => e.ProductsFK)
            .Include(e => e.CategoryFK)
            .Include(e => e.MarkFK)
            .ProjectTo<ProductsDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
        public async Task<Return> NewProduct(Products product)
        {
            var result = new Return();
            try
            {
                var rand = new Random();
                var idstock = Convert.ToInt32(rand.Next(0000000,9999999));
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    await context.Products.AddAsync(product);
                    await context.SaveChangesAsync();

                    context.Entry(product).State = EntityState.Detached;

                    var newstock = new Stock() {ID = idstock ,Product = product.ID, Amount = 0, Measure = "Un" };

                    await context.Stock.AddAsync(newstock);
                    await context.SaveChangesAsync();

                }
                result.Result = true;
                result.Message = $"Produto Cadastrado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message + ex.InnerException.Message;
            }
            return result;
        }
        public async Task<Products> ProductById(int Id) => await Context.Products
            .Include(e => e.CategoryFK)
            .Include(e => e.MarkFK)
            .Include(e => e.ProductSupplierFK)
            .Include(e => e.VirtualFK)
            .Include(e => e.ProductsFK)
            .Include(e => e.CreateByFK)
            .Include(e => e.ChangedByFK)
            .FirstOrDefaultAsync(e => e.ID == Id);
        public async Task<Return> UpdateProduct(Products product)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    var existing = await context.Products.FindAsync(product.ID);
                    if (existing == null)
                    {
                        result.Result = false;
                        result.Message = "Produto não encontrado.";
                        return result;
                    }

                    // Atualiza apenas os campos necessários
                    context.Entry(existing).CurrentValues.SetValues(product);
                    await context.SaveChangesAsync();

                }
                result.Result = true;
                result.Message = $"Produto Atualizado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        #endregion

        #region Suppliers
        public async Task<Return> DeleteSupplier(int supplierId)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    var DpRemove = await context.Suppliers.FirstOrDefaultAsync(e => e.ID == supplierId);
                    context.Suppliers.Remove(DpRemove);
                    await context.SaveChangesAsync();
                }
                result.Result = true;
                result.Message = $"Fornecedor Deletado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<List<Suppliers>> ListSupplier() => await Context.Suppliers.ToListAsync();
        public async Task<Return> NewSupplier(Suppliers supplier)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    await context.Suppliers.AddAsync(supplier);
                    await context.SaveChangesAsync();

                }
                result.Result = true;
                result.Message = $"Fornecedor Cadastrado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.InnerException.Message;
            }
            return result;
        }
        public async Task<Suppliers> SupplierById(int Id) => await Context.Suppliers.FirstOrDefaultAsync(e => e.ID == Id);
        public async Task<Return> UpdateSupplier(Suppliers supplier)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    context.Suppliers.Update(supplier);
                    await context.SaveChangesAsync();

                }
                result.Result = true;
                result.Message = $"Fornecedor Atualizado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        #endregion

        #region Product Supplier
        public async Task<Return> DeleteProductSupplier(int productsupplierId)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    var DpRemove = await context.Product_Supplier.FirstOrDefaultAsync(e => e.ID == productsupplierId);
                    context.Product_Supplier.Remove(DpRemove);
                    await context.SaveChangesAsync();
                }
                result.Result = true;
                result.Message = $"Departmento Deletado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<List<Product_Supplier>> ListProductSupplier() => await Context.Product_Supplier
            .Include(e => e.ProductFK)
            .Include(e => e.SupplierFK)
            .ToListAsync();
        public async Task<List<Product_Supplier>> ListProductSupplierByProduct(int productsupplier) => await Context.Product_Supplier.Where(e => e.Product == productsupplier)
            .Include(e => e.ProductFK)
            .Include(e => e.SupplierFK)
            .ToListAsync();
        public async Task<Return> NewProductSupplier(Product_Supplier productsupplier)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    await context.Product_Supplier.AddAsync(productsupplier);
                    await context.SaveChangesAsync();

                }
                result.Result = true;
                result.Message = $"Produto do Fornecedor Cadastrado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Product_Supplier> ProductSupplierById(int Id) => await Context.Product_Supplier.FirstOrDefaultAsync(e => e.ID == Id);
        public async Task<Return> UpdateProductSupplier(Product_Supplier productsupplier)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    context.Product_Supplier.Update(productsupplier);
                    await context.SaveChangesAsync();

                }
                result.Result = true;
                result.Message = $"Departmento Atualizado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        #endregion

        #region Product Category
        public async Task<Return> DeleteProductCategory(int productCategoryId)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    var DpRemove = await context.Product_Category.FirstOrDefaultAsync(e => e.ID == productCategoryId);
                    context.Product_Category.Remove(DpRemove);
                    await context.SaveChangesAsync();
                }
                result.Result = true;
                result.Message = $"Categoria de produto Deletado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<List<Product_Category>> ListProductCategory() => await Context.Product_Category.ToListAsync();
        public async Task<Return> NewProductCategory(Product_Category product_Category)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    await context.Product_Category.AddAsync(product_Category);
                    await context.SaveChangesAsync();

                }
                result.Result = true;
                result.Message = $"Produto de Categoria Cadastrado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Product_Category> ProductCategoryById(int Id) => await Context.Product_Category.FirstOrDefaultAsync(e => e.ID == Id);
        public async Task<Return> UpdateProductCategory(Product_Category product_Category)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    context.Product_Category.Update(product_Category);
                    await context.SaveChangesAsync();

                }
                result.Result = true;
                result.Message = $"Categoria de produto Atualizado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        #endregion

        #region Brands
        public async Task<Brands> BrandsById(int Id) => await Context.Brands.FirstOrDefaultAsync(e => e.ID == Id);
        public async Task<List<Brands>> ListBrands() => await Context.Brands.ToListAsync();
        public async Task<Return> DeleteBrands(int brandsId)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    var DpRemove = await context.Brands.FirstOrDefaultAsync(e => e.ID == brandsId);
                    context.Brands.Remove(DpRemove);
                    await context.SaveChangesAsync();
                }
                result.Result = true;
                result.Message = $"Marca Deletado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> NewBrands(Brands brands)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    await context.Brands.AddAsync(brands);
                    await context.SaveChangesAsync();

                }
                result.Result = true;
                result.Message = $"Marca Cadastrado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> UpdateBrands(Brands brands)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    context.Brands.Update(brands);
                    await context.SaveChangesAsync();

                }
                result.Result = true;
                result.Message = $"Marca Atualizado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        #endregion

        #region Stock
        public async Task<List<Stock>> ListStock() => await Context.Stock.ToListAsync();

        public async Task<Return> NewStock(Stock stock)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    await context.Stock.AddAsync(stock);
                    await context.SaveChangesAsync();

                }
                result.Result = true;
                result.Message = $"Estoque Cadastrado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Stock> StockById(int Id) => await Context.Stock.FirstOrDefaultAsync(e => e.ID == Id);
        public async Task<Return> UpdateStock(Stock stock)
        {
            var result = new Return();
            try
            {
                Context.Stock.Update(stock);
                await Context.SaveChangesAsync();
                result.Result = true;
                result.Message = $"Estoque Atualizado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        #endregion

        #region Virtual Stock
        public async Task<Return> DeleteVirtualStock(int virtualStockId)
        {
            var result = new Return();
            try
            {
                var del = await Context.Virtual_Stock.FirstOrDefaultAsync(e => e.ID == virtualStockId);
                Context.Virtual_Stock.Remove(del);
                await Context.SaveChangesAsync();

                result.Result = true;
                result.Message = $"Material Adicionado ao estoque virtual.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<List<Virtual_Stock>> ListVirtualStock() => await Context.Virtual_Stock.ToListAsync();
        public async Task<Return> NewVirtualStock(Virtual_Stock virtual_Stock)
        {
            var result = new Return();
            try
            {
                await Context.Virtual_Stock.AddAsync(virtual_Stock);
                await Context.SaveChangesAsync();

                var min = await Context.Stock.FirstOrDefaultAsync(e => e.Product == virtual_Stock.Product);
                min.Amount = min.Amount - virtual_Stock.Amount;
                Context.Stock.Update(min);
                await Context.SaveChangesAsync();

                result.Result = true;
                result.Message = $"Material Adicionado ao estoque virtual.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Virtual_Stock> VirtualStockById(int Id) => await Context.Virtual_Stock.FirstOrDefaultAsync(e => e.ID == Id);
        public async Task<Return> UpdateVirtualStock(Virtual_Stock virtual_Stock)
        {
            var result = new Return();
            try
            {
                Context.Virtual_Stock.Update(virtual_Stock);
                await Context.SaveChangesAsync();
                result.Result = true;
                result.Message = $"Material alterado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<List<Virtual_Stock>> ListVirtualStockByOrder(int Id) => await Context.Virtual_Stock.Where(e => e.Order == Id).ToListAsync();
        #endregion
    }
}

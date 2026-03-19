using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NexuSys.Data;
using NexuSys.DTOs.Purchase;
using NexuSys.DTOs.Purchase_Items;
using NexuSys.DTOs.Suppliers;
using NexuSys.Entities;
using NexuSys.Interfaces;
using NexuSys.Helper;

namespace NexuSys.Services
{
    public class PurchaseService : IPurchase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PurchaseService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region CREATE

        public async Task<int> NewPurchase(NewPurchaseDTO dto)
        {
            var entity = _mapper.Map<Purchase>(dto);

            await _context.Purchase.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.ID;
        }

        public async Task<Return> NewPurchase_Items(NewPurchase_ItemsDTO dto)
        {
            var result = new Return();

            try
            {
                var entity = _mapper.Map<Purchase_Items>(dto);

                await _context.Purchase_Items.AddAsync(entity);
                await _context.SaveChangesAsync();

                await _context.Stock.Where(e => e.Product == entity.Products)
                    .ExecuteUpdateAsync(e => e.SetProperty(e => e.Amount, a => a.Amount + entity.Amount));
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = "Item cadastrado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }

        public async Task<Return> NewSuppliers(NewSuppliersDTO dto)
        {
            var result = new Return();

            try
            {
                var supplier = _mapper.Map<Suppliers>(dto);

                await _context.Suppliers.AddAsync(supplier);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = "Fornecedor cadastrado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }

        #endregion

        #region READ

        public async Task<List<PurchaseDTO>> PurchaseList() =>
            await _context.Purchase
                .AsNoTracking()
                .ProjectTo<PurchaseDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<ByPurchaseDTO> PurchaseByID(int id) =>
            await _context.Purchase
                .AsNoTracking()
                .ProjectTo<ByPurchaseDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.ID == id);

        public async Task<List<Purchase_ItemsDTO>> Purchase_ItemsList() =>
            await _context.Purchase_Items
                .AsNoTracking()
                .ProjectTo<Purchase_ItemsDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<ByPurchase_ItemsDTO> Purchase_ItemsByID(int id) =>
            await _context.Purchase_Items
                .AsNoTracking()
                .ProjectTo<ByPurchase_ItemsDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.ID == id);

        public async Task<List<SuppliersDTO>> SuppliersList() =>
            await _context.Suppliers
                .AsNoTracking()
                .ProjectTo<SuppliersDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<BySuppliersDTO> SuppliersByID(int id) =>
            await _context.Suppliers
                .AsNoTracking()
                .ProjectTo<BySuppliersDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.ID == id);

        #endregion

        #region UPDATE

        public async Task<Return> UpdatePurchase(EditPurchaseDTO dto)
        {
            var result = new Return();

            try
            {
                var entity = _mapper.Map<Purchase>(dto);

                _context.Purchase.Update(entity);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = "Compra atualizada com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }

        public async Task<Return> UpdatePurchase_Items(EditPurchase_ItemsDTO dto)
        {
            var result = new Return();

            try
            {
                var entity = _mapper.Map<Purchase_Items>(dto);

                _context.Purchase_Items.Update(entity);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = "Item atualizado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }

        public async Task<Return> UpdateSuppliers(BySuppliersDTO dto)
        {
            var result = new Return();

            try
            {
                var supplier = _mapper.Map<Suppliers>(dto);

                _context.Suppliers.Update(supplier);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = "Fornecedor atualizado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }

        #endregion

        #region DELETE

        public async Task<Return> DeletePurchase(int id)
        {
            var result = new Return();

            try
            {
                var entity = await _context.Purchase.FindAsync(id);

                if (entity != null)
                {
                    _context.Purchase.Remove(entity);
                    await _context.SaveChangesAsync();
                }

                result.Result = true;
                result.Message = "Compra removida com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }

        public async Task<Return> DeletePurchase_Items(int id)
        {
            var result = new Return();

            try
            {
                var entity = await _context.Purchase_Items.FindAsync(id);

                if (entity != null)
                {
                    _context.Purchase_Items.Remove(entity);
                    await _context.SaveChangesAsync();
                }

                result.Result = true;
                result.Message = "Item removido com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }

        public async Task<Return> DeleteSuppliers(int id)
        {
            var result = new Return();

            try
            {
                var supplier = await _context.Suppliers.FindAsync(id);

                if (supplier != null)
                {
                    _context.Suppliers.Remove(supplier);
                    await _context.SaveChangesAsync();
                }

                result.Result = true;
                result.Message = "Fornecedor removido com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }

        #endregion
    }
}
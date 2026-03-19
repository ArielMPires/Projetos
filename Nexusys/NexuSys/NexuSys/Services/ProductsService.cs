using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NexuSys.Data;
using NexuSys.DTOs.Customers;
using NexuSys.DTOs.Equipment;
using NexuSys.DTOs.Products;
using NexuSys.DTOs.Stock;
using NexuSys.Entities;
using NexuSys.Interfaces;

namespace NexuSys.Services
{
    public class Productservice : IProducts
    {
        public readonly ApplicationDbContext _context;
        public readonly IMapper _mapper;

        public Productservice(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Customers

        public async Task<ByCustomersDTO> CustomersByID(int id) =>
            await _context.Customers
                .AsNoTracking()
                .ProjectTo<ByCustomersDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.ID == id);

        public async Task<List<CustomersDTO>> CustomersList() =>
            await _context.Customers
                .AsNoTracking()
                .ProjectTo<CustomersDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<Return> NewCustomers(NewCustomersDTO Customers)
        {
            var result = new Return();
            try
            {
                var customer = _mapper.Map<Customers>(Customers);
                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = "Cliente cadastrado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> UpdateCustomers(ByCustomersDTO Customers)
        {
            var result = new Return();
            try
            {
                var customer = _mapper.Map<Customers>(Customers);
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = "Cliente atualizado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> DeleteCustomers(int id)
        {
            var result = new Return();
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(e => e.ID == id);
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = $"Cliente ID:{id} removido";
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

        #region STOCK

        public async Task<ByStockDTO> StockByID(int id) =>
            await _context.Stock
                .AsNoTracking()
                .ProjectTo<ByStockDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.ID == id);

        public async Task<List<StockDTO>> StockList() =>
            await _context.Stock
                .AsNoTracking()
                .ProjectTo<StockDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<Return> NewStock(NewStockDTO Stock)
        {
            var result = new Return();

            try
            {
                var stock = _mapper.Map<Stock>(Stock);

                _context.Entry(stock).State = EntityState.Detached;

                await _context.Stock.AddAsync(stock);
                await _context.SaveChangesAsync();
                _context.Entry(stock).State = EntityState.Detached;

                result.Result = true;
                result.Message = "Estoque cadastrado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }

        public async Task<Return> UpdateStock(EditStockDTO Stock)
        {
            var result = new Return();

            try
            {
                var stock = _mapper.Map<Stock>(Stock);

                _context.Stock.Update(stock);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = "Estoque atualizado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
                _context.Entry(Stock).State = EntityState.Detached;
            }

            return result;
        }

        public async Task<Return> DeleteStock(int id)
        {
            var result = new Return();

            try
            {
                var stock = await _context.Stock
                    .FirstOrDefaultAsync(e => e.ID == id);

                if (stock != null)
                {
                    _context.Stock.Remove(stock);
                    await _context.SaveChangesAsync();
                }

                result.Result = true;
                result.Message = $"Estoque ID:{id} removido";
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

        #region Equipment

        public async Task<List<EquipmentDTO>> EquipmentListByClient(int id) => await _context.Equipment
            .AsNoTracking()
            .Where(e => e.Customer == id)
            .ProjectTo<EquipmentDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
        public async Task<ByEquipmentDTO> EquipmentByID(int id) =>
      await _context.Equipment
          .AsNoTracking()
          .ProjectTo<ByEquipmentDTO>(_mapper.ConfigurationProvider)
          .FirstOrDefaultAsync(e => e.ID == id);

        public async Task<List<EquipmentDTO>> EquipmentList() =>
            await _context.Equipment
                .AsNoTracking()
                .Include(e => e.customersFK)
                .Include(e => e.productsFK)
                .ProjectTo<EquipmentDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<Return> NewEquipment(NewEquipmentDTO Equipment)
        {
            var result = new Return();

            try
            {
                var equipment = _mapper.Map<Equipment>(Equipment);



                await _context.Equipment.AddAsync(equipment);
                await _context.SaveChangesAsync();

                _context.Entry(equipment).State = EntityState.Detached;

                result.Result = true;
                result.Message = "Equipamento cadastrado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }

        public async Task<Return> UpdateEquipment(EditEquipmentDTO Equipment)
        {
            var result = new Return();

            try
            {
                var equipment = _mapper.Map<Equipment>(Equipment);
                var history = equipment.historyFK;


                _context.History.Update(history);
                await _context.SaveChangesAsync();

                equipment.historyFK = null;


                _context.Equipment.Update(equipment);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = "Equipamento atualizado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }

        public async Task<Return> DeleteEquipment(int id)
        {
            var result = new Return();

            try
            {
                var equipment = await _context.Equipment
                    .FirstOrDefaultAsync(e => e.ID == id);

                if (equipment != null)
                {
                    _context.Equipment.Remove(equipment);
                    await _context.SaveChangesAsync();
                }

                result.Result = true;
                result.Message = $"Equipamento ID:{id} removido";
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

        #region Products

        public async Task<ByProductsDTO> ProductsByID(int id) =>
            await _context.Products
            .Include(e => e.StockFK)
                .AsNoTracking()
                .ProjectTo<ByProductsDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.ID == id);

        public async Task<List<ProductsDTO>> ProductsList() =>
            await _context.Products
            .Include(e => e.StockFK)
                .AsNoTracking()
                .ProjectTo<ProductsDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<Return> NewProducts(NewProductsDTO Products)
        {
            var result = new Return();
            try
            {
                var product = _mapper.Map<Products>(Products);
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                _context.Entry(product).State = EntityState.Detached;
                result.Result = true;
                result.Message = "Produto cadastrado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> UpdateProducts(ByProductsDTO Products)
        {
            var result = new Return();
            try
            {
                var product = _mapper.Map<Products>(Products);
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = "Produto atualizado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> DeleteProducts(int id)
        {
            var result = new Return();
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(e => e.ID == id);
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = $"Produto ID:{id} removido";
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

using NexuSys.DTOs.Products;
using NexuSys.DTOs.Equipment;
using NexuSys.DTOs.Customers;
using NexuSys.Entities;

namespace NexuSys.Interfaces
{
    public interface IProducts
    {
        #region Products
        Task<Return> NewProducts(NewProductsDTO Products);
        Task<Return> UpdateProducts(ByProductsDTO Products);
        Task<Return> DeleteProducts(int id);
        Task<List<ProductsDTO>> ProductsList();
        Task<ByProductsDTO> ProductsByID(int id);
        #endregion

        #region Equipment
        Task<Return> NewEquipment(NewEquipmentDTO Equipment);
        Task<Return> UpdateEquipment(EditEquipmentDTO Equipment);
        Task<Return> DeleteEquipment(int id);
        Task<List<EquipmentDTO>> EquipmentList();
        Task<List<EquipmentDTO>> EquipmentListByClient(int id);
        Task<ByEquipmentDTO> EquipmentByID(int id);
        #endregion

        #region Customers
        Task<Return> NewCustomers(NewCustomersDTO Customers);
        Task<Return> UpdateCustomers(ByCustomersDTO Customers);
        Task<Return> DeleteCustomers(int id);
        Task<List<CustomersDTO>> CustomersList();
        Task<ByCustomersDTO> CustomersByID(int id);
        #endregion
    }
}


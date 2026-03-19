using NexuSys.DTOs.Purchase;
using NexuSys.DTOs.Purchase_Items;
using NexuSys.DTOs.Role;
using NexuSys.DTOs.Seller;
using NexuSys.DTOs.Suppliers;
using NexuSys.DTOs.Users;
using NexuSys.Entities;

namespace NexuSys.Interfaces
{
    public interface IPurchase
    {
        #region Purchase
        Task<int> NewPurchase(NewPurchaseDTO Purchase);
        Task<Return> UpdatePurchase(EditPurchaseDTO Purchase);
        Task<Return> DeletePurchase(int id);
        Task<List<PurchaseDTO>> PurchaseList();
        Task<ByPurchaseDTO> PurchaseByID(int id);
        #endregion

        #region Purchase Items
        Task<Return> NewPurchase_Items(NewPurchase_ItemsDTO Purchase_Items);
        Task<Return> UpdatePurchase_Items(EditPurchase_ItemsDTO Purchase_Items);
        Task<Return> DeletePurchase_Items(int id);
        Task<List<Purchase_ItemsDTO>> Purchase_ItemsList();
        Task<ByPurchase_ItemsDTO> Purchase_ItemsByID(int id);

        #endregion

        #region Suppliers
        Task<Return> NewSuppliers(NewSuppliersDTO Suppliers);
        Task<Return> UpdateSuppliers(BySuppliersDTO Suppliers);
        Task<Return> DeleteSuppliers(int id);
        Task<List<SuppliersDTO>> SuppliersList();
        Task<BySuppliersDTO> SuppliersByID(int id);
        #endregion
    }
}

using System.ComponentModel.DataAnnotations;
using Domus.Models.DB;

namespace Domus.Models.DB {
    
    public class Product_Supplier {

        #region Property
        [Key]
        public int ID {get; set;}
        public int? Supplier {get; set;}
        public int? Product {get; set;}
        public decimal Purchase_Value {get; set;}
        #endregion

        #region Navigation
        public Suppliers? SupplierFK {get; set;}
        public Products? ProductFK {get; set;}
        #endregion
    }
}
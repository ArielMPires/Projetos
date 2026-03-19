using System.ComponentModel.DataAnnotations;
using Agnus.Models.DB;

namespace Agnus.Models.DB {

    public class Purchase_Order {

        #region Property
        [Key]
        public int ID {get; set;}
        public int Request {get; set;}
        public int Supplier {get; set;}
        public string Situation {get; set;}
        public bool Delivered {get; set;}
        public DateTime? Delivery_Time {get; set;}
        public int? CreateBy {get; set;}
        public DateTime? DateCreate {get; set;}
        public int? ChangedBy {get; set;}
        public DateTime? DateChanged {get; set;}
        #endregion

        #region Navigation
        public Request? RequestFK {get; set;}
        public Suppliers? SupplierFK {get; set;}
        public Users? CreateByFK {get; set;}
        public Users? ChangedByFK {get; set;}
        #endregion
    }
}
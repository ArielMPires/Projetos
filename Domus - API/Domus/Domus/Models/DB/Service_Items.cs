using System.ComponentModel.DataAnnotations;
using Domus.Models.DB;

namespace Domus.Models.DB {
    
    public class Service_Items {

        #region Property
        [Key]
        public int ID {get; set;}
        public int Order {get; set;}
        public int Product {get; set;}
        public float Amount {get; set;}
        #endregion

        #region Navigation
        public Service_Order? OrderFK {get; set;}
        public Products? ProductFK {get; set;}
        #endregion
    }
}
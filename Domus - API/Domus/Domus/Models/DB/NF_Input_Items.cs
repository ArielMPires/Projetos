using System.ComponentModel.DataAnnotations;
using Domus.Models.DB;

namespace Domus.Models.DB {
    
    public class NF_Input_Items {

        #region Property
        [Key]
        public int ID {get; set;}
        public int NF {get; set;}
        public int Product {get; set;}
        public float Amount {get; set;}
        public decimal Purchase_Value {get; set;}
        #endregion

        #region Navigation
        public NF_Input? NFFK {get; set;}
        public Products? ProductFK {get; set;}
        #endregion
    }
}
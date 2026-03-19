using System.ComponentModel.DataAnnotations;
using Agnus.Models.DB;

namespace Agnus.Models.DB {
    
    public class NF_Output_Items {

        #region Property
        [Key]
        public int ID {get; set;}
        public int NF {get; set;}
        public int Product {get; set;}
        public float Amount {get; set;}
        public decimal Usage_Value {get; set;}
        #endregion

        #region Navigation
        public NF_Output? NFFK {get; set;}
        public Products? ProductFK {get; set;}
        #endregion
    }
}
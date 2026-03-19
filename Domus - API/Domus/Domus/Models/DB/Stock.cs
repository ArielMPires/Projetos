    using System.ComponentModel.DataAnnotations;
using Domus.Models.DB;

namespace Domus.Models.DB {
    
    public class Stock {

        #region Property
        [Key]
        public int ID {get; set;}
        public int Product {get; set;}
        public float Amount {get; set;}
        public string Measure{get; set;}
        #endregion

        #region Navigation
        public Products? ProductFK {get; set;}
        #endregion
    }
}
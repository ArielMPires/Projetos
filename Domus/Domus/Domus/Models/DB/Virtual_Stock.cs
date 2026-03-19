using System.ComponentModel.DataAnnotations;
using Domus.Models.DB;

namespace Domus.Models.DB {
    
    public class Virtual_Stock {

        #region Property
        [Key]
        public int ID {get; set;}
        public int Product {get; set;}
        public float Amount {get; set;}
        public string Measure {get; set;}
        public string label {get; set;}
        public int Order {get; set;}
        #endregion

        #region Navigation
        public Products? ProductFK { get; set;}
        #endregion
    }
}
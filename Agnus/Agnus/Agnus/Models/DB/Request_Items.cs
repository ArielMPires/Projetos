using System.ComponentModel.DataAnnotations;
using Agnus.Models.DB;

namespace Agnus.Models.DB{

    public class Request_Items{

        #region Property
        [Key]
        public int ID {get; set;}
        public int Request {get; set;}
        public int Product {get; set;}
        public float Amount {get; set;}
        public decimal Unit_Value {get; set;}
        public bool purchase {get; set;}
        #endregion

        #region Navigation
        public Request? RequestFK {get; set;}
        public Products? ProductFK {get; set;}
        #endregion
    }
}
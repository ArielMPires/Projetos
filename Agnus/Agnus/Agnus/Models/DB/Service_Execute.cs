using System.ComponentModel.DataAnnotations;
using Agnus.Models.DB;

namespace Agnus.Models.DB {
    
    public class Service_Execute {

        #region Property
        [Key]
        public int ID {get; set;}
        public int? Order {get; set;}
        public int? Service {get; set;}
        #endregion

        #region Navigation
        public Services? ServiceFK {get; set;}
        public Service_Order? OrderFK {get; set;}
        #endregion
    }
}
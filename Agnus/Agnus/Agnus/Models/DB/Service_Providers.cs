using System.ComponentModel.DataAnnotations;
using Agnus.Models.DB;

namespace Agnus.Models.DB {
    
    public class Service_Providers {

        #region Property
        [Key]
        public int ID {get; set;}
        public string Name {get; set;}
        public int CNPJ {get; set;}
        public string Email {get; set;}
        public string Phone {get; set;}
        #endregion

        #region Navigation 
        public ICollection<Provider_Order>? Service_ProvidersFK { get; set; }
        #endregion
    }
}
using System.ComponentModel.DataAnnotations;
using Domus.Models.DB;

namespace Domus.Models.DB {
    
    public class End_Call {

        #region Property
        [Key]
        public int ID {get; set;}
        public int Order {get; set;}
        public int Technical {get; set;}
        public DateTime Date {get; set;}
        public string Reason{get; set;}
        #endregion

        #region Navigation
        public Users? TechnicalFK {get; set;}
        public Service_Order? Service_OrderFK {get; set;}
        #endregion
    }
}
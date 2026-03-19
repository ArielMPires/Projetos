using System.ComponentModel.DataAnnotations;
using Domus.Models.DB;

namespace Domus.Models.DB{

    public class Provider_Order{

        #region Property
        [Key]
        public int ID {get; set;}
        public int? Service_Order {get; set;}
        public int Provider {get; set;}
        public string Problem {get; set;}
        public string Protocol {get; set;}
        public string Technical {get; set;}
        public DateTime Resquested_Date {get; set;}
        public int Responsible {get; set;}
        public string Note {get; set;}
        public DateTime Term {get; set;}
        public DateTime End_Date {get; set;}
        #endregion

        #region Navigation
        public Service_Order? Service_OrderFK { get; set;}
        public Service_Providers? ProviderFK {get; set;}
        public Users? ResponsibleFK {get; set;}
        #endregion
    }

}
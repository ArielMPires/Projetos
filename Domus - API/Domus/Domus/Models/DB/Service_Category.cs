using System.ComponentModel.DataAnnotations;
using Domus.Models.DB;

namespace Domus.Models.DB {
    
    public class Service_Category {

        #region Property
        [Key]
        public int ID {get; set;}
        public string Name {get; set;}
        public int? CreateBy {get; set;}
        public DateTime DateCreate {get; set;}
        public int? ChangedBy {get; set;}
        public DateTime DateChanged {get; set;}
        #endregion

        #region Navigation
        public ICollection<Service_Type>? Service_TypeFK {get; set;}
        public Users? CreateByFK {get; set;}
        public Users? ChangedByFK {get; set;}
        #endregion
    }
}
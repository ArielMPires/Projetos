using System.ComponentModel.DataAnnotations;
using Agnus.Models.DB;

namespace Agnus.Models.DB {
    
    public class Scheduled_Maintenance {

        #region Property
        [Key]
        public int ID {get; set;}
        public int Computer {get; set;}
        public bool Status {get; set;}
        public DateTime Date {get; set;}
        public int? CreateBy {get; set;}
        public DateTime DateCreate {get; set;}
        public int? ChangedBy {get; set;} 
        public DateTime DateChanged {get; set;}
        #endregion

        #region Navigation
        public Users? CreateByFK {get; set;}
        public Users? ChangedByFK {get; set;}
        public Computer? ComputerFK {get; set;}
        public ICollection<Maintenance> MaintenanceFK {get; set;}
        #endregion

    }
}
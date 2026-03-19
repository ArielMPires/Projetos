using System.ComponentModel.DataAnnotations;
using Domus.Models.DB;

namespace Domus.Models.DB {
    
    public class Maintenance{

        #region Property
        [Key]
        public int ID {get; set;}
        public int Scheduling {get; set;}
        public int Technical {get; set;}
        public DateTime Date_Performed {get; set;}
        public int? CreateBy {get; set;}
        public DateTime DateCreate {get; set;}
        public int? ChangedBy {get; set;}
        public DateTime DateChanged {get; set;}
        #endregion

        #region Navigation
        public Users? CreateByFK {get; set;}
        public Users? ChangedByFK {get; set;}
        public Scheduled_Maintenance? SchedulingFK {get; set;}
        public Users? TechnicalFK {get; set;}
        public ICollection<Maintenance_CheckList>? Maintenance_CheckListFK {get; set;}
        #endregion
    }
}
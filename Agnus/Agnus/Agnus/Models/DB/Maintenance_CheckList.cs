using System.ComponentModel.DataAnnotations;
using Agnus.Models.DB;

namespace Agnus.Models.DB {
    
    public class Maintenance_CheckList {

        #region Property
        [Key]
        public int ID {get; set;}
        public int Maintenance {get; set;}
        public int Checklist {get; set;}
        public bool Checked {get; set;}
        public string Note {get; set;}
        public int? CreateBy {get; set;}
        public DateTime DateCreate {get; set;}
        public int? ChangedBy {get; set;}
        public DateTime DateChanged {get; set;}
        #endregion

        #region Navigation
        public Users? CreateByFK {get; set;}
        public Users? ChangedByFK {get; set;}
        public Checklist? CheckListFK {get; set;}
        public Maintenance? MaintenanceFK {get; set;}
        #endregion
    }
}
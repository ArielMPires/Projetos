using System.ComponentModel.DataAnnotations;
using Agnus.Models.DB;

namespace Agnus.Models.DB {
    
    public class Tasks {

        #region Property
        [Key]
        public int ID {get; set;}
        public int Project {get; set;}
        public int? Technical {get; set;}
        public string Service {get; set;}
        public DateTime Start_Date {get; set;}
        public DateTime End_Date {get; set;}
        public DateTime Term {get; set;}
        public bool Status {get; set;}
        public int? CreateBy {get; set;}
        public DateTime DateCreate {get; set;}
        public int? ChangedBy {get; set;}
        public DateTime DateChanged {get; set;}
        #endregion

        #region Navigation
        public Users? CreateByFK { get; set;}
        public Users? ChangedByFK {get; set;}
        public Users? TechnicalFK {get; set;}
        public Project? ProjectFK {get; set;}
        #endregion
    }
}
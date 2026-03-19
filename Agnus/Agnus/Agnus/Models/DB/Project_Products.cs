using System.ComponentModel.DataAnnotations;
using Agnus.Models.DB;

namespace Agnus.Models.DB {
    
    public class Project_Products {

        #region Property
        [Key]
        public int ID {get; set;}
        public int Project {get; set;}
        public int Product {get; set;}
        public float Amount {get; set;}
        public decimal Unit_Value {get; set;}
        public bool Status {get; set;}
        public string Note{get; set;}
        public int? CreateBy {get; set;}
        public DateTime DateCreate {get; set;}
        public int? ChangedBy {get; set;}
        public DateTime DateChanged {get; set;}
        #endregion

        #region Navigation
        public Users? CreateByFK {get; set;}
        public Users? ChangedByFK {get; set;}
        public Products? ProductFK {get; set;}
        public Project? ProjectFK {get; set;}
        #endregion
    }
}
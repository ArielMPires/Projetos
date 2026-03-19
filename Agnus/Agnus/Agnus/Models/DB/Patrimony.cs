using System.ComponentModel.DataAnnotations;
using Agnus.Models.DB;

namespace Agnus.Models.DB {

    public class Patrimony {

        #region Property
        [Key]   
        public int ID {get; set;}
        public string Description {get; set;}
        public int Category {get; set;}
        public string Serial {get; set;}
        public string? SerialSecondary {get; set;}
        public decimal Value {get; set;}
        public int? Department{get; set;}
        public int? Current_Owner { get; set; }
        public int FileFolder {get; set;}
        public int? CreateBy {get; set;}
        public DateTime? DateCreate {get; set;}
        public int? ChangedBy {get; set;}
        public DateTime? DateChanged {get; set;}
        #endregion

        #region Navigation
        public Patrimony_Category? CategoryFK {get; set;}
        public Department? DepartmentFK {get; set;}
        public Users? Current_OwnerFK {get; set;}
        public FileFolder? FileFolderFK {get; set;}
        public Users? CreateByFK {get; set;}
        public Users? ChangedByFK {get; set;}
        public Computer? ComputerFK {get; set;}  
        #endregion
    }
}   
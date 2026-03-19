using System.ComponentModel.DataAnnotations;
using Agnus.Models.DB;

namespace Agnus.Models.DB {
    
    public class NF_Output {

        #region Property
        [Key]
        public int ID {get; set;}
        public int Unit {get; set;}
        public decimal Total_Value {get; set;}
        public DateTime DateOut {get; set;}
        public int CreateBy {get; set;}
        public DateTime DateCreate {get; set;}
        public int? FileFolder {get; set;}
        public string? Label {get; set;}
        #endregion

        #region Navigation
        public Unit? UnitFK {get; set;}
        public Users? CreateByFK {get; set;}
        public FileFolder? FileFolderFK {get; set;}
        public ICollection<NF_Output_Items>? NF_Output_ItemsFK {get; set;}
        #endregion
    }
}
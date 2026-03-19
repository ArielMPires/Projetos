using System.ComponentModel.DataAnnotations;
using Agnus.Models.DB;

namespace Agnus.Models.DB {
    
    public class NF_Input {

        #region Property
        [Key]
        public int ID {get; set;}
        public int Numero {get; set;}
        public int Supplier {get; set;}
        public decimal Total_Value {get; set;}
        public DateTime DateIn {get; set;}
        public int? CreateBy {get; set;}
        public DateTime DateCreate {get; set;}
        public int? FileFolder {get; set;}
        public string? Label {get; set;}
        #endregion

        #region Navigation 
        public Suppliers? SupplierFK {get; set;}
        public Users? CreateByFK {get; set;}
        public FileFolder? FileFolderFK {get; set;}
        public ICollection<NF_Input_Items> NF_Input_ItemsFK {get; set;}
        #endregion
    }
}
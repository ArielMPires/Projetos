using System.ComponentModel.DataAnnotations;
using Domus.Models.DB;

namespace Domus.Models.DB {

    public class FileFolder{
        #region Property
        [Key]
        public int ID {get; set;}
        public string Name {get; set;}
        public int? CreateBy {get; set;}
        public DateTime DateCreate {get; set;}
        public int? ChangedBy {get; set;}
        public DateTime DateChanged{get; set;}
        #endregion

        #region Navigation
        public Users? CreateByFK {get; set;}
        public Users? ChangedByFK {get; set;}
        public ICollection<NF_Input>? NF_InputFK {get; set;}
        public ICollection<NF_Output>? NF_OutputFK {get; set;}
        public ICollection<Products>? ProductsFK {get; set;}
        public ICollection<Files>? FilesFK {get; set;}
        public ICollection<Manuals>? ManualsFK {get; set;}
        public ICollection<Patrimony>? PatrimonyFK {get; set;}
        public ICollection<Service_Order>? Service_OrderFK {get; set;}
        #endregion
    }
}
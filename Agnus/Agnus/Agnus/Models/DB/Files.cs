using System.ComponentModel.DataAnnotations;
using Agnus.Models.DB;

namespace Agnus.Models.DB {
    
    public class Files {

        #region Property
        [Key]
        public int ID {get; set;}
        public string Name {get; set;}
        public byte[] File {get; set;}
        public string Extension {get; set;}
        public int Folder {get; set;}
        public int? CreateBy {get; set;}
        public DateTime? DateCreate {get; set;}
        public int? ChangedBy {get; set;}
        public DateTime DateChanged {get; set;}
        #endregion

        #region Navigation
        public Users? CreateByFK {get; set;}
        public Users? ChangedByFK {get; set;}
        public FileFolder? FolderFK {get; set;}
        #endregion
    }
}
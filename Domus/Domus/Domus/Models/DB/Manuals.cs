using System.ComponentModel.DataAnnotations;
using Domus.Models.DB;

namespace Domus.Models.DB
{
    public class Manuals
    {
        #region Property
        [Key]
     public int ID {get; set;}
     public string Topic {get; set;}
     public string Description {get; set;}
     public int? FileFolder {get; set;}
     public int? CreateBy {get; set;}
     public DateTime? DateCreate {get; set;}
     public int? ChangedBy {get; set;}
     public DateTime? DateChanged{get;set;}
        #endregion

        #region Navigation
        public Users? CreateByFK {get; set;}
     public Users? ChangedByFK {get; set;}
     public FileFolder? fileFolderFK {get; set;}
        #endregion
    }
}
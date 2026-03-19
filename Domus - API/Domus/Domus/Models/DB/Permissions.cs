using System.ComponentModel.DataAnnotations;
using Domus.Models.DB;

namespace Domus.Models.DB {

    public class Permissions {

        #region Property
        [Key]
        public int ID {get; set;}
        public int User {get; set;}
        public string Page {get; set;}
        public int? CreateBy {get; set;}
        public int? ChangedBy{get; set;}
        public DateTime? DateCreate {get; set;}
        public DateTime? DateChanged {get; set;}
        #endregion

        #region Navigation
        public Users? UserFK {get; set;}
        public Users? CreateByFK {get; set;}
        public Users? ChangedByFK {get; set;}
        #endregion
    }
}
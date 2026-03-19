using System.ComponentModel.DataAnnotations;
using Agnus.Models.DB;

namespace Agnus.Models.DB {

    public class Department {

        #region Property
        [Key]
        public int ID {get; set;}
        public string Name {get; set;}
        public int? CreateBy {get; set;}
        public DateTime? DateCreate {get; set;}
        public int? ChangedBy {get; set;}
        public DateTime? DateChanged {get; set;}
        #endregion

        #region Navigation
        public Users? CreateByFK {get; set;}
        public Users? ChangedByFK {get; set;}
        public ICollection<Users>? UserFK {get; set;}
        public ICollection<Patrimony>? PatrimonyFK {get; set;}
        public ICollection<Request>? RequestFK {get; set;}
        #endregion
    }
}
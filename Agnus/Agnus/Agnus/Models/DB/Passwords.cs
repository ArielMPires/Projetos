using System.ComponentModel.DataAnnotations;
using Agnus.Models.DB;

namespace Agnus.Models.DB{

    public class Passwords{

        #region Property
        [Key]
        public int ID {get; set;}
        public int Type {get; set;}
        public int Owner {get; set;}
        public string User {get; set;}
        public string Password {get; set;}
        public int? CreateBy {get; set;}
        public DateTime DateCreateBy {get; set;}
        public int? ChangedBy {get; set;}
        public DateTime DateChanged {get; set;}
        #endregion

        #region Navigation
        public Type_Passwords? TypeFK {get; set;}
        public Users? OwnerFK {get; set;}
        public Users? CreateByFK {get; set;}
        public Users? ChangedByFK {get; set;}
        #endregion
    }
}    
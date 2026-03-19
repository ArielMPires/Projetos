using System.ComponentModel.DataAnnotations;
using Agnus.Models.DB;

namespace Agnus.Models.DB {
    
    public class Type_Passwords {

        #region Property
        [Key]
        public int ID {get; set;}
        public string Name {get; set;}
        #endregion
        #region Navigation
        public ICollection<Passwords>? PasswordsFK {get; set;}
        #endregion
    }
}
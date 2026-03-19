using System.ComponentModel.DataAnnotations;
using Domus.Models.DB;

namespace Domus.Models.DB {
    
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
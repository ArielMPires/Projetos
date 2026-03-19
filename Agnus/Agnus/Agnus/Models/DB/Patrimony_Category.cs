using System.ComponentModel.DataAnnotations;
using Agnus.Models.DB;

namespace Agnus.Models.DB {
    
    public class Patrimony_Category {

        #region Property
        [Key]
        public int ID {get; set;}
        public string Name {get; set;}
        public int? CreateBy { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? ChangedBy { get; set; }
        public DateTime? DateChanged { get; set; }
        #endregion

        #region Navigation
        public ICollection<Patrimony>? PatrimoniesFK { get; set; }
        public Users? CreateByFK { get; set; }
        public Users? ChangedByFK { get; set; }
        #endregion
    }
}
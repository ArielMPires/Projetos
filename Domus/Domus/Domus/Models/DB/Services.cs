using System.ComponentModel.DataAnnotations;
using Domus.Models.DB;

namespace Domus.Models.DB {
    
    public class Services {

        #region Property
        [Key]
        public int ID {get; set;}
        public string Name {get; set;}
        #endregion

        #region Navigation
        public ICollection<Service_Execute>? Service_ExecuteFK { get; set;}
        #endregion
    }
}
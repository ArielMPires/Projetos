using System.ComponentModel.DataAnnotations;
using Domus.Models.DB;

namespace Domus.Models.DB{
    
    public class Request_Usage{

        #region Property
        [Key]
        public int ID {get; set;}
        public string Name {get; set;}
        #endregion

        #region Navigation
        public ICollection<Request>? Request_UsageFK { get; set; }
        #endregion

    }
}
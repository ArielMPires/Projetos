using System.ComponentModel.DataAnnotations;
using Agnus.Models.DB;

namespace Agnus.Models.DB{

    public class Request_Approval{

        #region Property
        [Key]
        public int ID {get; set;}
        public bool Situation { get; set; }
        public int ApprovalBy {get; set;}
        public string Reason {get; set;}
        #endregion

        #region Navigation
        public Users? ApprovalByFk {get; set;}
        public Request? RequestFK {get; set;}
        #endregion
    }
}
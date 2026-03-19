using System.ComponentModel.DataAnnotations;
using Domus.Models.DB;

namespace Domus.Models.DB {

    public class Request {

        #region Property
        [Key]
        public int ID {get; set;}
        public int Department {get; set;}
        public int Requester {get; set;}
        public int Use {get; set;}
        public bool Status {get; set;}
        public decimal Total_Value {get; set;}
        public string? Note {get; set;}
        public int? Authorization {get; set;}
        public int? CreateBy {get; set;}
        public DateTime? DateCreate {get; set;}
        public int? ChangedBy {get; set;}
        public DateTime? DateChanged {get; set;}
        #endregion

        #region Navigation
        public Department? DepartmentFK {get; set;}
        public Users? RequesterFK {get; set;}
        public Request_Usage? UseFK {get; set;}
        public Request_Approval? AuthorizationFK {get; set;}
        public Users? CreateByFK {get; set;}
        public Users? ChangedByFK {get; set;}
        public ICollection<Request_Items>? ItemsFK {get; set;}
        public ICollection<Project>? ProjectFK {get; set;}
        public Purchase_Order? RequestFK {get; set;}
        #endregion
    }
}
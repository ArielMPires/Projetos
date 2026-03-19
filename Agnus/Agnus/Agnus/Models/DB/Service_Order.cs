using System.ComponentModel.DataAnnotations;
using Agnus.Models.DB;

namespace Agnus.Models.DB {
    
    public class Service_Order {

        #region Property
        [Key]
        public int ID {get; set;}
        public string Problem {get; set;}
        public int Request {get; set;}
        public int Computer {get; set;}
        public DateTime Requested_Date {get; set;}
        public int? Technical {get; set;}
        public bool Status {get; set;}
        public int Type {get; set;}
        public DateTime? Contact_Date {get; set;}
        public string? Note {get; set;}
        public string? Service {get; set;}
        public int? FileFolder {get; set;}
        public int? CreateBy {get; set;}
        public DateTime? DateCreate {get; set;}
        public int? ChangedBy {get; set;}
        public DateTime? DateChanged {get; set;}
        #endregion

        #region Navigation
        public Service_Rate? RateFK { get; set; }
        public Users? CreateByFK {get; set;}
        public Users? ChangedByFK {get; set;}
        public Users? TechnicalFK {get; set;}
        public Users? RequestFK {get; set;}
        public Computer? ComputerFK {get; set;}
        public Service_Type? TypeFK {get; set;}
        public End_Call? ConcludeFK {get; set;}
        public FileFolder? FileFolderFK {get; set;}
        public ICollection<Service_CheckList>? Service_CheckListFK {get; set;}
        public ICollection<Service_Items>? Service_ItemsFK {get; set;}
        public ICollection<Service_Execute>? Service_ExecuteFK {get; set;}
        public ICollection<Provider_Order>? Provider_OrderFK {get; set;}
        #endregion
    }
}
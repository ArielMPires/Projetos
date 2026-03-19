using System.ComponentModel.DataAnnotations;
using Agnus.Models.DB;

namespace Agnus.Models.DB{

    public class Computer{

        #region Property
        [Key]
        public int ID {get; set;}
        public string MotherBoard {get; set;}
        public string Processor {get; set;}
        public string Memory {get; set;}
        public string Cooler {get; set;}
        public string Storage {get; set;}
        public string Power_Supply {get; set;}
        public string? DVD {get; set;}
        public string Cabinet {get; set;}
        public string SO {get; set;}
        public string? PCI {get; set;}
        public string? PCI_EX {get; set;}
        public string? PCI_X16 {get; set;}
        public int? CreateBy {get; set;}
        public DateTime? DateCreate {get; set;}
        public int? ChangedBy {get; set;}
        public DateTime? DateChanged {get; set;}
        #endregion

        #region Navigation
        public Users? CreateByFK {get; set;}
        public Users? ChangedByFK {get; set;}
        public ICollection<Scheduled_Maintenance> MaintenanceFK {get; set;}
        public ICollection<Service_Order> Service_OrderFK {get; set;}
        public Patrimony PatrimonyFK {get; set;}
        #endregion

    }
}
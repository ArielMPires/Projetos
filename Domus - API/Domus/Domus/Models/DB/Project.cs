using System.ComponentModel.DataAnnotations;
using Domus.Models.DB;

namespace Domus.Models.DB {
    
    public class Project {

        #region Property
        [Key]
        public int ID {get; set;}
        public string Topic {get; set;}
        public string Description {get; set;}
        public int Applicant {get; set;}
        public bool Status {get; set;}
        public DateTime Start_Date {get; set;}
        public DateTime End_Date {get; set;}
        public DateTime Term {get; set;}
        public int? Request {get; set;}
        public decimal Purchase_Cost {get; set;}
        public decimal Total_Cost {get; set;}
        public int Responsible {get; set;}
        public int? CreateBy {get; set;}
        public DateTime DateCreate {get; set;}
        public int? ChangedBy {get; set;}
        public DateTime DateChanged {get; set;}
        #endregion

        #region Navigation
        public Users? CreateByFK {get; set;}
        public Users? ChangedByFK {get; set;}
        public Users? ResponsibleFK {get; set;}
        public Users? ApplicantFK {get; set;}
        public Request? RequestFK {get; set;}
        public ICollection<Project_Products>? Project_ProductsFK {get; set;}
        public ICollection<Tasks>? TasksFK {get; set;}
        #endregion
    }
}
using System.ComponentModel.DataAnnotations;
using Agnus.Models.DB;

namespace Agnus.Models.DB {
    public class Products {

        #region Property
        [Key]
        public int ID {get; set;}
        public string Description {get; set;}
        public int? Category {get; set;}
        public int? Mark {get; set;}
        public float Minimum_Stock {get; set;}
        public decimal Usage_Value{get; set;}
        public int? CreateBy {get; set;}
        public DateTime? DateCreate {get; set;}
        public int? ChangedBy {get; set;}
        public DateTime? DateChanged {get; set;}
        public int? FolderPicture  {get; set;}
        #endregion

        #region Navigation
        public Product_Category? CategoryFK {get; set;}
        public Brands? MarkFK {get; set;}
        public Users? CreateByFK {get; set;}
        public Users? ChangedByFK {get; set;}
        public FileFolder? FolderPictureFK {get; set;}
        public ICollection<Virtual_Stock>? VirtualFK {get; set;}
        public Stock? ProductsFK {get; set;}
        public ICollection<NF_Output_Items>? NF_Output_ItemsFK {get; set;}
        public ICollection<NF_Input_Items>? NF_Input_ItemsFK {get; set;}
        public ICollection<Project_Products>? ProductProjectFK {get; set;}
        public ICollection<Product_Supplier>? ProductSupplierFK {get; set;}
        public ICollection<Service_Items>? Service_ItemsFK {get; set;}
        public ICollection<Request_Items>? Request_ItemsFK {get; set;}
        #endregion
    }
}
using System.ComponentModel.DataAnnotations;
using Agnus.Models.DB;


namespace Agnus.Models.DB{
    public class Users {
        #region Property
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int? Role { get; set; }
        public int? Department { get; set; }
        public byte[]? photo { get; set; }
        public byte[]? Signature { get; set; }
        #endregion

        #region Navigation
        public Roles? RoleFK { get; set; }
        public Department? DepartmentFK { get; set; }
        public ICollection<Department>? DepartmentOneFK { get; set; }
        public ICollection<Department>? DepartmentTwoFK { get; set; }
        public ICollection<Patrimony>? PatrimonyOneFK { get; set; }
        public ICollection<Patrimony>? PatrimonyTwoFK { get; set; }
        public ICollection<Patrimony>? PatrimonyThreeFK { get; set; }
        public ICollection<Patrimony_Category>? PatrimonyCatOneFK { get; set; }
        public ICollection<Patrimony_Category>? PatrimonyCatFK { get; set; }
        public ICollection<Manuals>? ManualsOneFK { get; set; }
        public ICollection<Manuals>? ManualsTwoFK { get; set; }
        public ICollection<Computer>? ComputerOneFK { get; set; }
        public ICollection<Computer>? ComputerTwoFK { get; set; }
        public ICollection<Permissions>? PermissionsOneFK { get; set; }
        public ICollection<Permissions>? PermissionsTwoFK { get; set; }
        public ICollection<Permissions>? PermissionsThreeFK { get; set; }
        public ICollection<Request>? RequestOneFK { get; set; }
        public ICollection<Request>? RequestTwoFK { get; set; }
        public ICollection<Request>? RequestThreeFk {get; set;}
        public ICollection<Passwords>? PasswordsOneFK {get; set;}
        public ICollection<Passwords>? PasswordsTwoFK {get; set;}
        public ICollection<Passwords>? PasswordsThreeFK {get; set;}
        public ICollection<End_Call>? End_CallOneFK {get; set;}
        public ICollection<Provider_Order>? Provider_OrderOneFK {get; set;}
        public ICollection<Request_Approval>? Request_ApprovalOneFK {get; set;}
        public ICollection<Purchase_Order>? Purchase_OrdeOneFK {get; set;}
        public ICollection<Purchase_Order>? Purchase_OrdeTwoFK {get; set;}
        public ICollection<Service_Category>? Service_CategoryOneFK {get; set;}
        public ICollection<Service_Category>? Service_CategoryTwoFK {get; set;}
        public ICollection<Suppliers>? SuppliersOneFK {get; set;}
        public ICollection<Suppliers>? SuppliersTwoFK {get; set;}
        public ICollection<Service_Order>? Service_OrderOneFK {get; set;} 
        public ICollection<Service_Order>? Service_OrderTwoFK {get; set;}
        public ICollection<Service_Order>? Service_OrderThreeFK {get; set;}
        public ICollection<Service_Order>? Service_OrderFourFK {get; set;}
        public ICollection<Products>? ProductsOneFK {get; set;}
        public ICollection<Products>? ProductsTwoFK {get; set;}
        public ICollection<Product_Category>? ProductCatOneFK { get; set; }
        public ICollection<Product_Category>? ProductCatTwoFK { get; set; }
        public ICollection<Files>? FilesOneFK {get; set;}
        public ICollection<Files>? FilesTwoFK {get; set;}
        public ICollection<Scheduled_Maintenance>? Scheduled_MaintenanceOneFK {get; set;} 
        public ICollection<Scheduled_Maintenance>? Scheduled_MaintenanceTwoFK {get; set;}
        public ICollection<Maintenance>? MaintenanceOneFK {get; set;}
        public ICollection<Maintenance>? MaintenanceTwoFK {get; set;}
        public ICollection<Maintenance>? MaintenanceThreeFK {get; set;}
        public ICollection<Service_Type>? Service_TypesOneFK {get; set;}
        public ICollection<Service_Type>? Service_TypesTwoFK {get; set;}
        public ICollection<Project>? ProjectOneFK {get; set;}
        public ICollection<Project>? ProjectTwoFK {get; set;}
        public ICollection<Project>? ProjectThreeFK {get; set;}
        public ICollection<Project>? ProjectFourFK {get; set;}
        public ICollection<Service_CheckList>? Service_CheckListOneFK {get; set;}
        public ICollection<Service_CheckList>? Service_CheckListTwoFK {get; set;}
        public ICollection<Maintenance_CheckList>? Maintenance_CheckListOneFK {get; set;}
        public ICollection<Maintenance_CheckList>? Maintenance_CheckListTwoFK {get; set;}
        public ICollection<Checklist>? CheckListOneFK {get; set;}
        public ICollection<Checklist>? CheckListTwoFK {get; set;}
        public ICollection<Project_Products>? Project_ProductsOneFK {get; set;}
        public ICollection<Project_Products>? Project_ProductsTwoFK {get; set;}
        public ICollection<NF_Output>? NF_OutputOneFK {get; set;}
        public ICollection<NF_Input>? NF_InputOneFK {get; set;}
        public ICollection<FileFolder>? FileFolderOneFK {get; set;}
        public ICollection<FileFolder>? FileFolderTwoFK {get; set;}
        public ICollection<Tasks>? TasksOneFK {get; set;}
        public ICollection<Tasks>? TasksTwoFK {get; set;}
        public ICollection<Tasks>? TasksThreeeFK {get; set;}
        public ICollection<Brands>? BrandsOneFK {get; set;}
        public ICollection<Brands>? BrandsTwoFK {get; set;}
        #endregion
    }
}
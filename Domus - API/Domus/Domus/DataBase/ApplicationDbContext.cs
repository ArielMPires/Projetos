using Domus.Models.DB;
using Domus.Services;
using Microsoft.EntityFrameworkCore;

namespace Domus.DataBase
{
    public class ApplicationDbContext : DbContext
    {
        #region Property
        private readonly TenantProvider _tenantProvider;
        private readonly int _tenantId;
        #endregion

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        #region DBSet
        public DbSet<Brands> Brands { get; set; }
        public DbSet<Checklist> CheckList { get; set; }
        public DbSet<Computer> Computer { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<End_Call> End_Call { get; set; }
        public DbSet<FileFolder> FileFolder { get; set; }
        public DbSet<Files> Files { get; set; }
        public DbSet<Maintenance> Maintenance { get; set; }
        public DbSet<Maintenance_CheckList> Maintenance_CheckList {  get; set; }
        public DbSet<Manuals> Manuals { get; set; }
        public DbSet<NF_Input> NF_Input { get; set; }
        public DbSet<NF_Input_Items> NF_Input_Items { get; set; }
        public DbSet<NF_Output> NF_Output { get; set; }
        public DbSet<NF_Output_Items> NF_Output_Items { get; set; }
        public DbSet<Passwords> Passwords { get; set; }
        public DbSet<Patrimony> Patrimony { get; set; }
        public DbSet<Patrimony_Category> Patrimony_Category { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<Product_Category> Product_Category { get; set; }
        public DbSet<Product_Supplier> Product_Supplier { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Project_Products> Project_Products { get; set; }
        public DbSet<Provider_Order> Provider_Order { get; set; }
        public DbSet<Purchase_Order> Purchase_Order { get; set; }
        public DbSet<Request> Request { get; set; }
        public DbSet<Request_Approval> Request_Approval { get; set; }
        public DbSet<Request_Items> Request_Items { get; set; }
        public DbSet<Request_Usage> Request_Usage { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Scheduled_Maintenance> Scheduled_Maintenance { get; set; }
        public DbSet<Service_Category> Service_Category { get; set; }
        public DbSet<Service_CheckList> Service_CheckList { get; set; }
        public DbSet<Service_Execute> Service_Execute { get; set; }
        public DbSet<Service_Items> Service_Items { get; set; }
        public DbSet<Service_Order> Service_Order { get; set; }
        public DbSet<Service_Providers> Service_Providers { get; set; }
        public DbSet<Service_Type> Service_Type { get; set; }
        public DbSet<Service_Rate> Service_Rate { get; set; }
        public DbSet<Domus.Models.DB.Services> Services { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<Suppliers> Suppliers { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Theme> Theme { get; set; }
        public DbSet<Type_Passwords> Type_Passwords { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Virtual_Stock> Virtual_Stock { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Brands>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.BrandsOneFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Brands>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.BrandsTwoFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Checklist>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.CheckListOneFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Checklist>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.CheckListTwoFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Computer>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.ComputerOneFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Computer>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.ComputerTwoFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Department>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.DepartmentOneFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Department>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.DepartmentTwoFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<End_Call>()
                .HasOne(e => e.TechnicalFK)
                .WithMany(e => e.End_CallOneFK)
                .HasForeignKey(e => e.Technical)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<FileFolder>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.FileFolderOneFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<FileFolder>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.FileFolderTwoFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Files>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.FilesOneFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(deleteBehavior: DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Files>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.FilesTwoFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Files>()
                .HasOne(e => e.FolderFK)
                .WithMany(e => e.FilesFK)
                .HasForeignKey(e => e.Folder)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Maintenance>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.MaintenanceOneFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(deleteBehavior: DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Maintenance>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.MaintenanceTwoFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Maintenance>()
                .HasOne(e => e.SchedulingFK)
                .WithMany(e => e.MaintenanceFK)
                .HasForeignKey(e => e.Scheduling)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Maintenance>()
                .HasOne(e => e.TechnicalFK)
                .WithMany(e => e.MaintenanceThreeFK)
                .HasForeignKey(e => e.Technical)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Maintenance_CheckList>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.Maintenance_CheckListOneFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(deleteBehavior: DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Maintenance_CheckList>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.Maintenance_CheckListTwoFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Maintenance_CheckList>()
                .HasOne(e => e.CheckListFK)
                .WithMany(e => e.Maintenance_CheckListFK)
                .HasForeignKey(e => e.Checklist)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Maintenance_CheckList>()
                .HasOne(e => e.MaintenanceFK)
                .WithMany(e => e.Maintenance_CheckListFK)
                .HasForeignKey(e => e.Maintenance)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Manuals>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.ManualsOneFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(deleteBehavior: DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Manuals>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.ManualsTwoFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Manuals>()
                .HasOne(e => e.fileFolderFK)
                .WithMany(e => e.ManualsFK)
                .HasForeignKey(e => e.FileFolder)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<NF_Input>()
                .HasOne(e => e.SupplierFK)
                .WithMany(e => e.InputFK)
                .HasForeignKey(e => e.Supplier)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<NF_Input>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.NF_InputOneFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<NF_Input>()
                .HasOne(e => e.FileFolderFK)
                .WithMany(e => e.NF_InputFK)
                .HasForeignKey(e => e.FileFolder)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<NF_Input_Items>()
                .HasOne(e => e.NFFK)
                .WithMany(e => e.NF_Input_ItemsFK)
                .HasForeignKey(e => e.NF)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<NF_Input_Items>()
                .HasOne(e => e.ProductFK)
                .WithMany(e => e.NF_Input_ItemsFK)
                .HasForeignKey(e => e.Product)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<NF_Output>()
                .HasOne(e => e.UnitFK)
                .WithMany(e => e.NF_OutputFK)
                .HasForeignKey(e => e.Unit)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<NF_Output>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.NF_OutputOneFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<NF_Output>()
                .HasOne(e => e.FileFolderFK)
                .WithMany(e => e.NF_OutputFK)
                .HasForeignKey(e => e.FileFolder)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<NF_Output_Items>()
                .HasOne(e => e.NFFK)
                .WithMany(e => e.NF_Output_ItemsFK)
                .HasForeignKey(e => e.NF)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<NF_Output_Items>()
                .HasOne(e => e.ProductFK)
                .WithMany(e => e.NF_Output_ItemsFK)
                .HasForeignKey(e => e.Product)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Passwords>()
                .HasOne(e => e.TypeFK)
                .WithMany(e => e.PasswordsFK)
                .HasForeignKey(e => e.Type)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Passwords>()
                .Property(e => e.Password)
                .HasConversion(new EncryptedStringConverter());

            builder.Entity<Passwords>()
                .HasOne(e => e.OwnerFK)
                .WithMany(e => e.PasswordsOneFK)
                .HasForeignKey(e => e.Owner)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Passwords>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.PasswordsTwoFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Passwords>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.PasswordsThreeFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Patrimony>()
                .HasOne(e => e.CategoryFK)
                .WithMany(e => e.PatrimoniesFK)
                .HasForeignKey(e => e.Category)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Patrimony>()
                .HasOne(e => e.DepartmentFK)
                .WithMany(e => e.PatrimonyFK)
                .HasForeignKey(e => e.Department)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Patrimony>()
                .HasOne(e => e.Current_OwnerFK)
                .WithMany(e => e.PatrimonyOneFK)
                .HasForeignKey(e => e.Current_Owner)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Patrimony>()
                .HasOne(e => e.FileFolderFK)
                .WithMany(e => e.PatrimonyFK)
                .HasForeignKey(e => e.FileFolder)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Patrimony>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.PatrimonyTwoFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Patrimony>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.PatrimonyThreeFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Patrimony>()
                .HasOne(e => e.ComputerFK)
                .WithOne(e => e.PatrimonyFK)
                .HasForeignKey<Patrimony>(e => e.ID)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Patrimony_Category>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.PatrimonyCatFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Patrimony_Category>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.PatrimonyCatOneFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Permissions>()
                .HasOne(e => e.UserFK)
                .WithMany(e => e.PermissionsOneFK)
                .HasForeignKey(e => e.User)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Permissions>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.PermissionsTwoFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Permissions>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.PermissionsThreeFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Product_Category>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.ProductCatOneFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Product_Category>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.ProductCatTwoFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Product_Supplier>()
                .HasOne(e => e.SupplierFK)
                .WithMany(e => e.SupplierFK)
                .HasForeignKey(e => e.Supplier)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Product_Supplier>()
                .HasOne(e => e.ProductFK)
                .WithMany(e => e.ProductSupplierFK)
                .HasForeignKey(e => e.Product)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Products>()
                .HasOne(e => e.CategoryFK)
                .WithMany(e => e.Product_CategoryFK)
                .HasForeignKey(e => e.Category)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Products>()
                .HasOne(e => e.MarkFK)
                .WithMany(e => e.BrandsFK)
                .HasForeignKey(e => e.Mark)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Products>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.ProductsOneFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Products>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.ProductsTwoFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Products>()
                .HasOne(e => e.FolderPictureFK)
                .WithMany(e => e.ProductsFK)
                .HasForeignKey(e => e.FolderPicture)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Stock>()
                .HasOne(e => e.ProductFK)
                .WithOne(e => e.ProductsFK)
                .HasForeignKey<Stock>(e => e.Product)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Project>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.ProjectOneFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Project>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.ProjectTwoFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Project>()
                .HasOne(e => e.ResponsibleFK)
                .WithMany(e => e.ProjectThreeFK)
                .HasForeignKey(e => e.Responsible)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Project>()
                .HasOne(e => e.ApplicantFK)
                .WithMany(e => e.ProjectFourFK)
                .HasForeignKey(e => e.Applicant)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Project>()
                .HasOne(e => e.RequestFK)
                .WithMany(e => e.ProjectFK)
                .HasForeignKey(e => e.Request)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Project_Products>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.Project_ProductsOneFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Project_Products>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.Project_ProductsTwoFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Project_Products>()
                .HasOne(e => e.ProductFK)
                .WithMany(e => e.ProductProjectFK)
                .HasForeignKey(e => e.Product)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Project_Products>()
                .HasOne(e => e.ProjectFK)
                .WithMany(e => e.Project_ProductsFK)
                .HasForeignKey(e => e.Project)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Provider_Order>()
                .HasOne(e => e.Service_OrderFK)
                .WithMany(e => e.Provider_OrderFK)
                .HasForeignKey(e => e.Service_Order)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Provider_Order>()
                .HasOne(e => e.ProviderFK)
                .WithMany(e => e.Service_ProvidersFK)
                .HasForeignKey(e => e.Provider)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Provider_Order>()
                .HasOne(e => e.ResponsibleFK)
                .WithMany(e => e.Provider_OrderOneFK)
                .HasForeignKey(e => e.Responsible)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Purchase_Order>()
                .HasOne(e => e.SupplierFK)
                .WithMany(e => e.PurchaseFK)
                .HasForeignKey(e => e.Supplier)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Purchase_Order>()
                .HasOne(e => e.RequestFK)
                .WithOne(e => e.RequestFK)
                .HasForeignKey<Purchase_Order>(e => e.Request)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Purchase_Order>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.Purchase_OrdeOneFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Purchase_Order>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.Purchase_OrdeTwoFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Request>()
                .HasOne(e => e.DepartmentFK)
                .WithMany(e => e.RequestFK)
                .HasForeignKey(e => e.Department)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Request>()
                .HasOne(e => e.RequesterFK)
                .WithMany(e => e.RequestOneFK)
                .HasForeignKey(e => e.Requester)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Request>()
                .HasOne(e => e.UseFK)
                .WithMany(e => e.Request_UsageFK)
                .HasForeignKey(e => e.Use)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Request>()
                .HasOne(e => e.AuthorizationFK)
                .WithOne(e => e.RequestFK)
                .HasForeignKey<Request>(e => e.Authorization)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Request>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.RequestTwoFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Request>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.RequestThreeFk)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Request_Approval>()
                .HasOne(e => e.ApprovalByFk)
                .WithMany(e => e.Request_ApprovalOneFK)
                .HasForeignKey(e => e.ApprovalBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Request_Items>()
                .HasOne(e => e.RequestFK)
                .WithMany(e => e.ItemsFK)
                .HasForeignKey(e => e.Request)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Request_Items>()
                .HasOne(e => e.ProductFK)
                .WithMany(e => e.Request_ItemsFK)
                .HasForeignKey(e => e.Product)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Scheduled_Maintenance>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.Scheduled_MaintenanceOneFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Scheduled_Maintenance>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.Scheduled_MaintenanceTwoFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Scheduled_Maintenance>()
                .HasOne(e => e.ComputerFK)
                .WithMany(e => e.MaintenanceFK)
                .HasForeignKey(e => e.Computer)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Service_Category>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.Service_CategoryOneFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Service_Category>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.Service_CategoryTwoFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Service_CheckList>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.Service_CheckListOneFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Service_CheckList>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.Service_CheckListTwoFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Service_CheckList>()
                .HasOne(e => e.CheckListFK)
                .WithMany(e => e.Service_CheckListFK)
                .HasForeignKey(e => e.Checklist)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Service_CheckList>()
                .HasOne(e => e.OrderFK)
                .WithMany(e => e.Service_CheckListFK)
                .HasForeignKey(e => e.Order)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Service_Execute>()
                .HasOne(e => e.ServiceFK)
                .WithMany(e => e.Service_ExecuteFK)
                .HasForeignKey(e => e.Service)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Service_Execute>()
                .HasOne(e => e.OrderFK)
                .WithMany(e => e.Service_ExecuteFK)
                .HasForeignKey(e => e.Order)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Service_Items>()
                .HasOne(e => e.OrderFK)
                .WithMany(e => e.Service_ItemsFK)
                .HasForeignKey(e => e.Order)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Service_Items>()
                .HasOne(e => e.ProductFK)
                .WithMany(e => e.Service_ItemsFK)
                .HasForeignKey(e => e.Product)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Service_Order>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.Service_OrderOneFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Service_Order>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.Service_OrderTwoFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Service_Order>()
                .HasOne(e => e.TechnicalFK)
                .WithMany(e => e.Service_OrderThreeFK)
                .HasForeignKey(e => e.Technical)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Service_Order>()
                .HasOne(e => e.RequestFK)
                .WithMany(e => e.Service_OrderFourFK)
                .HasForeignKey(e => e.Request)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Service_Order>()
                .HasOne(e => e.ComputerFK)
                .WithMany(e => e.Service_OrderFK)
                .HasForeignKey(e => e.Computer)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Service_Order>()
                .HasOne(e => e.TypeFK)
                .WithMany(e => e.Service_OrderFK)
                .HasForeignKey(e => e.Type)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Service_Order>()
                .HasOne(e => e.ConcludeFK)
                .WithOne(e => e.Service_OrderFK)
                .HasForeignKey<End_Call>(e => e.Order)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Service_Order>()
                .HasOne(e => e.FileFolderFK)
                .WithMany(e => e.Service_OrderFK)
                .HasForeignKey(e => e.FileFolder)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Service_Type>()
                .HasOne(e => e.CategoryFK)
                .WithMany(e => e.Service_TypeFK)
                .HasForeignKey(e => e.Category)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Service_Type>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.Service_TypesOneFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Service_Type>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.Service_TypesTwoFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Service_Rate>()
                .HasOne(e => e.OrderFK)
                .WithOne(e => e.RateFK)
                .HasForeignKey<Service_Rate>(e => e.Order)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Suppliers>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.SuppliersOneFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Suppliers>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.SuppliersTwoFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Tasks>()
                .HasOne(e => e.CreateByFK)
                .WithMany(e => e.TasksOneFK)
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Tasks>()
                .HasOne(e => e.ChangedByFK)
                .WithMany(e => e.TasksTwoFK)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Tasks>()
                .HasOne(e => e.TechnicalFK)
                .WithMany(e => e.TasksThreeeFK)
                .HasForeignKey(e => e.Technical)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Tasks>()
                .HasOne(e => e.ProjectFK)
                .WithMany(e => e.TasksFK)
                .HasForeignKey(e => e.Project)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Theme>()
                .HasOne(e => e.UserFK)
                .WithOne(e => e.ThemeFK)
                .HasForeignKey<Theme>(e => e.User)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Users>()
                .HasOne(e => e.RoleFK)
                .WithMany(e => e.RolesFK)
                .HasForeignKey(e => e.Role)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Entity<Users>()
                .HasOne(e => e.DepartmentFK)
                .WithMany(e => e.UserFK)
                .HasForeignKey(e => e.Department)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
            
            builder.Entity<Virtual_Stock>()
                .HasOne(e => e.ProductFK)
                .WithMany(e => e.VirtualFK)
                .HasForeignKey(e => e.Product)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

        }
    }
}

using Microsoft.EntityFrameworkCore;
using NexuSys.Entities;

namespace NexuSys.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        #region DBSet
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<FileFolder> FileFolder { get; set; }
        public DbSet<Files> Files { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<NFs> NFs { get; set; }
        public DbSet<Possible_Defects> Possible_Defects { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Repair_Activities> Repair_Activities { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<Review_Activities> Review_Activities { get; set; }
        public DbSet<Review_Defects> Review_Defects { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Seller> Seller { get; set; }
        public DbSet<Service_Equipment> Service_Equipment { get; set; }
        public DbSet<Service_Items> Service_Items { get; set; }
        public DbSet<Service_Order> Service_Order { get; set; }
        public DbSet<Situation> Situation { get; set; }
        public DbSet<Type_Service> Type_Service { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Purchase> Purchase { get; set; }
        public DbSet<Purchase_Items> Purchase_Items { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<Suppliers> Suppliers { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // =========================
            // CUSTOMERS
            // =========================

            modelBuilder.Entity<Customers>()
                .HasOne(x => x.SellerFK)
                .WithMany(c => c.customersFK)
                .HasForeignKey(x => x.Seller)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Customers>()
                .HasOne(x => x.historyFK)
                .WithMany()
                .HasForeignKey(x => x.History)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // SERVICE ORDER (CORE DO SISTEMA)
            // =========================

            modelBuilder.Entity<Service_Order>()
                .HasOne(x => x.CustomersFK)
                .WithMany()
                .HasForeignKey(x => x.Customer)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Service_Order>()
                .HasOne(x => x.departmentFK)
                .WithMany()
                .HasForeignKey(x => x.Department)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Service_Order>()
                .HasOne(x => x.TechnicalFK)
                .WithMany()
                .HasForeignKey(x => x.Technical)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Service_Order>()
                .HasOne(x => x.SituationFK)
                .WithMany()
                .HasForeignKey(x => x.Situation)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Service_Order>()
                .HasOne(x => x.Type_ServiceFK)
                .WithMany()
                .HasForeignKey(x => x.Type_Service)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Service_Order>()
                .HasOne(x => x.unitFK)
                .WithMany()
                .HasForeignKey(x => x.Unit)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Service_Order>()
                .HasOne(x => x.filefolderFK)
                .WithMany()
                .HasForeignKey(x => x.FileFolder)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Service_Order>()
                .HasOne(x => x.DeliveryNoteFK)
                .WithMany()
                .HasForeignKey(x => x.Delivery_Note)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Service_Order>()
                .HasOne(x => x.DepartureNoteFK)
                .WithMany()
                .HasForeignKey(x => x.Departure_Note)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Service_Order>()
                .HasOne(x => x.ServiceNoteFK)
                .WithMany()
                .HasForeignKey(x => x.Service_Note)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Service_Order>()
                .HasOne(x => x.historyFK)
                .WithMany()
                .HasForeignKey(x => x.History)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // SERVICE ITEMS
            // =========================

            modelBuilder.Entity<Service_Items>()
                .HasOne(x => x.service_orderFK)
                .WithMany(x => x.ItemsFK)
                .HasForeignKey(x => x.OS)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Service_Items>()
                .HasOne(x => x.productsFK)
                .WithMany()
                .HasForeignKey(x => x.Product)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Service_Items>()
                .HasOne(x => x.historyFK)
                .WithMany()
                .HasForeignKey(x => x.History)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // SERVICE EQUIPMENT
            // =========================

            modelBuilder.Entity<Service_Equipment>()
                .HasOne(x => x.service_orderFK)
                .WithOne(x => x.service_equipmentFK)
                .HasForeignKey<Service_Equipment>(x => x.OS)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Service_Equipment>()
                .HasOne(x => x.equipmentFK)
                .WithMany()
                .HasForeignKey(x => x.Equipment)
                .OnDelete(DeleteBehavior.Restrict);


            // =========================
            // EQUIPMENT
            // =========================

            modelBuilder.Entity<Equipment>()
                .HasOne(x => x.customersFK)
                .WithMany()
                .HasForeignKey(x => x.Customer)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Equipment>()
                .HasOne(x => x.productsFK)
                .WithMany()
                .HasForeignKey(x => x.Product)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Equipment>()
                .HasOne(x => x.historyFK)
                .WithMany()
                .HasForeignKey(x => x.History)
                .OnDelete(DeleteBehavior.Restrict);
            // =========================
            // BUDGET
            // =========================

            modelBuilder.Entity<Budget>()
                .HasOne(x => x.service_orderFK)
                .WithOne(x => x.BudgetFK)
                .HasForeignKey<Budget>(x => x.OS)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Budget>()
                .HasOne(x => x.FileFolderFK)
                .WithMany()
                .HasForeignKey(x => x.FileFolder)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Budget>()
                .HasOne(x => x.historyFK)
                .WithMany()
                .HasForeignKey(x => x.History)
                .OnDelete(DeleteBehavior.Restrict);


            // =========================
            // HISTORY
            // =========================

            modelBuilder.Entity<History>()
                .HasOne(x => x.CreateByFK)
                .WithMany()
                .HasForeignKey(x => x.CreateBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<History>()
                .HasOne(x => x.ChangeByFK)
                .WithMany()
                .HasForeignKey(x => x.ChangedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Unit>()
                .HasOne(x => x.historyFK)
                .WithMany()
                .HasForeignKey(x => x.History)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Department>()
                .HasOne(x => x.historyFK)
                .WithMany()
                .HasForeignKey(x => x.History)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Situation>()
                .HasOne(x => x.historyFK)
                .WithMany()
                .HasForeignKey(x => x.History)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Possible_Defects>()
                .HasOne(x => x.historyFK)
                .WithMany()
                .HasForeignKey(x => x.History)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Role>()
                .HasOne(x => x.historyFK)
                .WithMany()
                .HasForeignKey(x => x.History)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Type_Service>()
                .HasOne(x => x.historyFK)
                .WithMany()
                .HasForeignKey(x => x.History)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Seller>()
                .HasOne(x => x.historyFK)
                .WithMany()
                .HasForeignKey(x => x.History)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Repair_Activities>()
                .HasOne(x => x.historyFK)
                .WithMany()
                .HasForeignKey(x => x.History)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FileFolder>()
                .HasOne(x => x.historyFK)
                .WithMany()
                .HasForeignKey(x => x.History)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // REVIEW
            // =========================

            modelBuilder.Entity<Review>()
                .HasOne(x => x.budgetFK)
                .WithOne(x => x.ReviewFK)
                .HasForeignKey<Review>(x => x.Budget_number)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review_Activities>()
                .HasOne(x => x.ReviewFK)
                .WithMany(x => x.ActiviesFK)
                .HasForeignKey(x => x.Review)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review_Activities>()
                .HasOne(x => x.ActivitiesFK)
                .WithMany()
                .HasForeignKey(x => x.Activities)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review_Activities>()
                 .HasOne(x => x.historyFK)
                 .WithMany()
                 .HasForeignKey(x => x.History)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review_Defects>()
                .HasOne(x => x.ReviewFK)
                .WithMany(x => x.DefectsFK)
                .HasForeignKey(x => x.Review)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review_Defects>()
                .HasOne(x => x.DefectsFK)
                .WithMany()
                .HasForeignKey(x => x.Defects)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review_Defects>()
                .HasOne(x => x.historyFK)
                .WithMany()
                .HasForeignKey(x => x.History)
                .OnDelete(DeleteBehavior.Restrict);

            //=====================================
            // USERS
            //=====================================

            modelBuilder.Entity<Users>()
                .HasOne(e => e.DepartmentFK)
                .WithMany()
                .HasForeignKey(e => e.Department)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Users>()
                .HasOne(x => x.historyFK)
                .WithMany()
                .HasForeignKey(x => x.History)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Users>()
                .HasOne(x => x.RoleFK)
                .WithMany()
                .HasForeignKey(e => e.Role)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // PRODUCTS
            // =========================

            modelBuilder.Entity<Products>()
                .HasOne(x => x.historyFK)
                .WithMany()
                .HasForeignKey(x => x.History)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // NFS
            // =========================

            modelBuilder.Entity<NFs>()
                .HasOne(x => x.customersFK)
                .WithMany()
                .HasForeignKey(x => x.Customers)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NFs>()
                .HasOne(x => x.folderFK)
                .WithMany()
                .HasForeignKey(x => x.Folder)
                .OnDelete(DeleteBehavior.Restrict);
            // =========================
            // FILES
            // =========================

            modelBuilder.Entity<Files>()
                .HasOne(x => x.folderFK)
                .WithMany(x => x.Files)
                .HasForeignKey(x => x.Folder)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Files>()
                .HasOne(x => x.historyFK)
                .WithMany()
                .HasForeignKey(x => x.History)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // NOVOS
            // =========================

            modelBuilder.Entity<Permissions>()
                .HasOne(x => x.rolefk)
                .WithMany()
                .HasForeignKey(x => x.Role)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Stock>()
                .HasOne(e => e.productsFK)
                .WithOne(e => e.StockFK)
                .HasForeignKey<Stock>(e => e.Product)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Purchase>()
                .HasOne(e => e.suppliersFK)
                .WithMany()
                .HasForeignKey(e => e.Supplier)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Purchase>()
                .HasOne(e => e.NfsFK)
                .WithMany()
                .HasForeignKey(e => e.NF)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Purchase_Items>()
                .HasOne(e => e.productsFK)
                .WithMany()
                .HasForeignKey(e => e.Products)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Purchase_Items>()
                .HasOne(e => e.purchaseFK)
                .WithMany()
                .HasForeignKey(e => e.Purchase)
                .OnDelete(DeleteBehavior.Restrict);



            base.OnModelCreating(modelBuilder);
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VinClean.Repo.Models;

public partial class ServiceAppContext : DbContext
{
    public ServiceAppContext()
    {
    }

    public ServiceAppContext(DbContextOptions<ServiceAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Building> Buildings { get; set; }

    public virtual DbSet<BuildingType> BuildingTypes { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderImage> OrderImages { get; set; }

    public virtual DbSet<OrderRequest> OrderRequests { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceWorkIn> ServiceWorkIns { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=trongps-swp.database.windows.net;Initial Catalog=ServiceApp;Persist Security Info=True;User ID=swp;Password=Colen123;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__46A222CD07A7F4C9");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Email, "UQ__Account__AB6E61641D829675").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.Dob).HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Img)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("img");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("((0))")
                .HasColumnName("isDeleted");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PasswordResetToken).IsUnicode(false);
            entity.Property(e => e.ResetTokenExpires).HasColumnType("datetime");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.VerificationToken).IsUnicode(false);
            entity.Property(e => e.VerifiedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Account__role_id__656C112C");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.BlogId).HasName("PK__Blog__2975AA283CF2E426");

            entity.ToTable("Blog");

            entity.Property(e => e.BlogId).HasColumnName("blog_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Content)
                .HasColumnType("ntext")
                .HasColumnName("content");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.Img)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("img");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("((0))")
                .HasColumnName("isDeleted");
            entity.Property(e => e.ModifiedDate).HasColumnType("date");
            entity.Property(e => e.Sumarry)
                .HasMaxLength(255)
                .HasColumnName("sumarry");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");

            entity.HasOne(d => d.Category).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Blog__category_i__66603565");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BlogCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Blog__CreatedBy__6754599E");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.BlogModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__Blog__ModifiedBy__68487DD7");
        });

        modelBuilder.Entity<Building>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Building__3214EC07ED5178BF");

            entity.ToTable("Building");

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.TypeId).HasColumnName("Type_id");

            entity.HasOne(d => d.Type).WithMany(p => p.Buildings)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK__Building__Type_i__693CA210");
        });

        modelBuilder.Entity<BuildingType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Building__3214EC07B2BFE382");

            entity.ToTable("Building_type");

            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__D54EE9B48BC8C2E9");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Category1)
                .HasMaxLength(100)
                .HasColumnName("category");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comment__E7957687D64E14B7");

            entity.ToTable("Comment");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.BlogId).HasColumnName("blog_id");
            entity.Property(e => e.Content)
                .HasColumnType("ntext")
                .HasColumnName("content");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("((0))")
                .HasColumnName("isDeleted");
            entity.Property(e => e.ModifiedDate).HasColumnType("date");

            entity.HasOne(d => d.Blog).WithMany(p => p.Comments)
                .HasForeignKey(d => d.BlogId)
                .HasConstraintName("FK__Comment__blog_id__6A30C649");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CommentCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Comment__Created__6B24EA82");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.CommentModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__Comment__Modifie__6C190EBB");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__CD65CB85FACCACBA");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.Phone, "UQ__Customer__B43B145F149379A7").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("dob");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TotalMoney)
                .HasColumnType("money")
                .HasColumnName("total_money");
            entity.Property(e => e.TotalPoint).HasColumnName("total_point");

            entity.HasOne(d => d.Account).WithMany(p => p.Customers)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Customer__accoun__6D0D32F4");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__C52E0BA885740B83");

            entity.ToTable("Employee");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.Account).WithMany(p => p.Employees)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Employee__accoun__6E01572D");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC070AC548BC");

            entity.ToTable("Location");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longtitude).HasColumnName("longtitude");
            entity.Property(e => e.OrderId).HasColumnName("Order_id");

            entity.HasOne(d => d.Employee).WithMany(p => p.Locations)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Location__employ__6EF57B66");

            entity.HasOne(d => d.Order).WithMany(p => p.Locations)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Location__Order___6FE99F9F");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__F1FF8453C79AEF0E");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("Order_id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.BuildingId).HasColumnName("Building_id");
            entity.Property(e => e.CancelDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.EmployeeId).HasColumnName("Employee_id");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("((0))")
                .HasColumnName("isDeleted");
            entity.Property(e => e.Note)
                .HasColumnType("ntext")
                .HasColumnName("note");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.PointUsed).HasColumnName("Point_used");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.RatingId).HasColumnName("Rating_id");
            entity.Property(e => e.ReasonCancel).HasMaxLength(255);
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.SubPrice).HasColumnType("money");

            entity.HasOne(d => d.Building).WithMany(p => p.Orders)
                .HasForeignKey(d => d.BuildingId)
                .HasConstraintName("FK_orders_building");

            entity.HasOne(d => d.CancelByNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CancelBy)
                .HasConstraintName("FK__Order__ModifiedB__71D1E811");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Order__customer___70DDC3D8");

            entity.HasOne(d => d.Employee).WithMany(p => p.Orders)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_orders_employee");

            entity.HasOne(d => d.Rating).WithMany(p => p.Orders)
                .HasForeignKey(d => d.RatingId)
                .HasConstraintName("DF_Order_rateId_5CE63DAB");

            entity.HasOne(d => d.Service).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK_orders_service");
        });

        modelBuilder.Entity<OrderImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderIma__3213E83FC9DC582D");

            entity.ToTable("OrderImage");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.OrderId).HasColumnName("Order_id");
            entity.Property(e => e.Type)
                .HasMaxLength(30)
                .HasColumnName("type");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderImages)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderImag__Order__797309D9");
        });

        modelBuilder.Entity<OrderRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order_re__3214EC07B5369919");

            entity.ToTable("Order_request");

            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.CreateBy).HasColumnName("create_by");
            entity.Property(e => e.NewEmployeeId).HasColumnName("newEmployee_id");
            entity.Property(e => e.Note)
                .IsUnicode(false)
                .HasColumnName("note");
            entity.Property(e => e.OldEmployeeId).HasColumnName("oldEmployee_id");
            entity.Property(e => e.OrderId).HasColumnName("Order_id");
            entity.Property(e => e.Satus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("satus");

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.OrderRequests)
                .HasForeignKey(d => d.CreateBy)
                .HasConstraintName("fkcreateBy_Order_request_Account");

            entity.HasOne(d => d.NewEmployee).WithMany(p => p.OrderRequestNewEmployees)
                .HasForeignKey(d => d.NewEmployeeId)
                .HasConstraintName("fknewEmp_Order_request_Employee");

            entity.HasOne(d => d.OldEmployee).WithMany(p => p.OrderRequestOldEmployees)
                .HasForeignKey(d => d.OldEmployeeId)
                .HasConstraintName("fk_Order_request_Employee");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderRequests)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Order_req__Order__75A278F5");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RateId).HasName("PK__Rating__75920B4282918FFE");

            entity.ToTable("Rating");

            entity.Property(e => e.RateId).HasColumnName("rate_id");
            entity.Property(e => e.Comment)
                .HasColumnType("ntext")
                .HasColumnName("comment");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("((0))")
                .HasColumnName("isDeleted");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Rate).HasColumnName("rate");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");

            entity.HasOne(d => d.Customer).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Rating__customer__7A672E12");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__Rating__Modified__7B5B524B");

            entity.HasOne(d => d.Service).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Rating__service___7C4F7684");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__760965CCF8A356CD");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Service__3E0DB8AF77976D52");

            entity.ToTable("Service");

            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.Avaiable)
                .HasDefaultValueSql("((1))")
                .HasColumnName("avaiable");
            entity.Property(e => e.Cost)
                .HasColumnType("money")
                .HasColumnName("cost");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.Description)
                .HasColumnType("ntext")
                .HasColumnName("description");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("((0))")
                .HasColumnName("isDeleted");
            entity.Property(e => e.MinimalSlot).HasColumnName("minimal_slot");
            entity.Property(e => e.ModifiedDate).HasColumnType("date");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .HasColumnName("status");
            entity.Property(e => e.TypeId).HasColumnName("type_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ServiceCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Service__Created__7D439ABD");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ServiceModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__Service__Modifie__7E37BEF6");

            entity.HasOne(d => d.Type).WithMany(p => p.Services)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK__Service__type_id__7F2BE32F");
        });

        modelBuilder.Entity<ServiceWorkIn>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Service_WorkIn");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");

            entity.HasOne(d => d.Employee).WithMany()
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Service_W__emplo__00200768");

            entity.HasOne(d => d.Service).WithMany()
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Service_W__servi__01142BA1");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__Type__2C0005985559C86F");

            entity.ToTable("Type");

            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.Avaiable)
                .HasDefaultValueSql("((1))")
                .HasColumnName("avaiable");
            entity.Property(e => e.Img)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("img");
            entity.Property(e => e.Type1)
                .HasMaxLength(100)
                .HasColumnName("type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

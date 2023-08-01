using System;
using System.Collections.Generic;
using CYRetailIMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Infrastructure.Database;

public partial class CYDBContext : DbContext
{
    public CYDBContext()
    {
    }

    public CYDBContext(DbContextOptions<CYDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TMBranch> TMBranches { get; set; }

    public virtual DbSet<TMBranchDetail> TMBranchDetails { get; set; }

    public virtual DbSet<TMCurrency> TMCurrencies { get; set; }

    public virtual DbSet<TMDepartment> TMDepartments { get; set; }

    public virtual DbSet<TMEmployee> TMEmployees { get; set; }

    public virtual DbSet<TMItem> TMItems { get; set; }

    public virtual DbSet<TMItemInBranch> TMItemInBranches { get; set; }

    public virtual DbSet<TMItemPromotion> TMItemPromotions { get; set; }

    public virtual DbSet<TMItemPromotionDetail> TMItemPromotionDetails { get; set; }

    public virtual DbSet<TMItemType> TMItemTypes { get; set; }

    public virtual DbSet<TMMenu> TMMenus { get; set; }

    public virtual DbSet<TMPaymentType> TMPaymentTypes { get; set; }

    public virtual DbSet<TMPurchaseType> TMPurchaseTypes { get; set; }

    public virtual DbSet<TMRole> TMRoles { get; set; }

    public virtual DbSet<TMRoleInMenu> TMRoleInMenus { get; set; }

    public virtual DbSet<TMShipment> TMShipments { get; set; }

    public virtual DbSet<TMShipmentType> TMShipmentTypes { get; set; }

    public virtual DbSet<TMSubMenu> TMSubMenus { get; set; }

    public virtual DbSet<TMSupplier> TMSuppliers { get; set; }

    public virtual DbSet<TMSupplierContact> TMSupplierContacts { get; set; }

    public virtual DbSet<TMSupplierContactType> TMSupplierContactTypes { get; set; }

    public virtual DbSet<TMSupplierDetail> TMSupplierDetails { get; set; }

    public virtual DbSet<TMSupplierType> TMSupplierTypes { get; set; }

    public virtual DbSet<TMUnitOfMeasure> TMUnitOfMeasures { get; set; }

    public virtual DbSet<TMUser> TMUsers { get; set; }

    public virtual DbSet<TMUserInBranch> TMUserInBranchs { get; set; }

    public virtual DbSet<TMWarehouse> TMWarehouses { get; set; }

    public virtual DbSet<TTPurchaseOrder> TTPurchaseOrders { get; set; }

    public virtual DbSet<TTPurchaseOrderDetail> TTPurchaseOrderDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlServer("Server=.\\;Data Source=localhost;Initial Catalog=CYDB;Persist Security Info=True;User ID=cyuser;Password=#pakdum?0104;TrustServerCertificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TMBranch>(entity =>
        {
            entity.Property(e => e.BranchID).ValueGeneratedNever();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMBranchDetail>(entity =>
        {
            entity.Property(e => e.BranchID).ValueGeneratedNever();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Branch).WithOne(p => p.TMBranchDetail).HasConstraintName("FK_TMBranchDetail_TMBranch");
        });

        modelBuilder.Entity<TMCurrency>(entity =>
        {
            entity.HasKey(e => e.CurrencyID).HasName("PK_TMCurrencyType");

            entity.Property(e => e.CurrencyID).ValueGeneratedNever();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMDepartment>(entity =>
        {
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMEmployee>(entity =>
        {
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Department).WithMany(p => p.TMEmployees).HasConstraintName("FK_TMEmployee_TMDepartments");

            entity.HasOne(d => d.User).WithMany(p => p.TMEmployees).HasConstraintName("FK_TMEmployee_TMUsers");
        });

        modelBuilder.Entity<TMItem>(entity =>
        {
            entity.Property(e => e.ItemID).ValueGeneratedNever();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.ItemType).WithMany(p => p.TMItems).HasConstraintName("FK_TMItem_TMItemType");

            entity.HasOne(d => d.UnitOfMeasure).WithMany(p => p.TMItems).HasConstraintName("FK_TMItem_TMUnitOfMeasure");
        });

        modelBuilder.Entity<TMItemInBranch>(entity =>
        {
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Branch).WithMany(p => p.TMItemInBranches).HasConstraintName("FK_TMItemInBranch_TMBranch");

            entity.HasOne(d => d.Item).WithMany(p => p.TMItemInBranches).HasConstraintName("FK_TMItemInBranch_TMItem");
        });

        modelBuilder.Entity<TMItemPromotion>(entity =>
        {
            entity.Property(e => e.PromotionID).ValueGeneratedNever();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMItemPromotionDetail>(entity =>
        {
            entity.HasKey(e => new { e.PromotionID, e.ItemID }).HasName("PK_TMItemPromotionDetail_1");

            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Item).WithMany(p => p.TMItemPromotionDetails).HasConstraintName("FK_TMItemPromotionDetail_TMItem");

            entity.HasOne(d => d.Promotion).WithMany(p => p.TMItemPromotionDetails).HasConstraintName("FK_TMItemPromotionDetail_TMItemPromotion");
        });

        modelBuilder.Entity<TMItemType>(entity =>
        {
            entity.Property(e => e.ItemTypeID).ValueGeneratedNever();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMMenu>(entity =>
        {
            entity.Property(e => e.MenuID).ValueGeneratedNever();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMPaymentType>(entity =>
        {
            entity.Property(e => e.PaymenTypeID).ValueGeneratedNever();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMPurchaseType>(entity =>
        {
            entity.Property(e => e.PurchaseTypeID).ValueGeneratedNever();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMRole>(entity =>
        {
            entity.HasKey(e => e.RoleID).HasName("PK_Table1");

            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMRoleInMenu>(entity =>
        {
            entity.HasOne(d => d.Menu).WithMany(p => p.TMRoleInMenus).HasConstraintName("FK_TMRoleInMenus_TMMenus");

            entity.HasOne(d => d.Role).WithMany(p => p.TMRoleInMenus).HasConstraintName("FK_TMRoleInMenus_TMRoles");

            entity.HasOne(d => d.SubMenu).WithMany(p => p.TMRoleInMenus).HasConstraintName("FK_TMRoleInMenus_TMSubMenu");
        });

        modelBuilder.Entity<TMShipment>(entity =>
        {
            entity.Property(e => e.ShipmentID).ValueGeneratedNever();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.ShipmentType).WithMany(p => p.TMShipments)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TMShipment_TMShipmentType");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.TMShipments)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TMShipment_TMWarehouse");
        });

        modelBuilder.Entity<TMShipmentType>(entity =>
        {
            entity.Property(e => e.ShipmentTypeID).ValueGeneratedNever();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMSubMenu>(entity =>
        {
            entity.Property(e => e.SubMenuID).ValueGeneratedNever();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMSupplier>(entity =>
        {
            entity.HasKey(e => e.SupplierID).HasName("PK_TMVendor");

            entity.Property(e => e.SupplierID).ValueGeneratedNever();
            entity.Property(e => e.Description).IsFixedLength();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.SupplierType).WithMany(p => p.TMSuppliers).HasConstraintName("FK_TMSupplier_TMSupplierType");
        });

        modelBuilder.Entity<TMSupplierContact>(entity =>
        {
            entity.Property(e => e.SupplierContactID).ValueGeneratedNever();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.SupplierContactType).WithMany(p => p.TMSupplierContacts).HasConstraintName("FK_TMSupplierContact_TMSupplierContactType");

            entity.HasOne(d => d.Supplier).WithMany(p => p.TMSupplierContacts).HasConstraintName("FK_TMSupplierContact_TMSupplier");
        });

        modelBuilder.Entity<TMSupplierContactType>(entity =>
        {
            entity.HasKey(e => e.SupplierContactTypeID).HasName("PK_TMVendorContactType");

            entity.Property(e => e.SupplierContactTypeID).ValueGeneratedNever();
            entity.Property(e => e.Description).IsFixedLength();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMSupplierDetail>(entity =>
        {
            entity.Property(e => e.SupplierDetailID).ValueGeneratedNever();
            entity.Property(e => e.Description).IsFixedLength();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Supplier).WithMany(p => p.TMSupplierDetails).HasConstraintName("FK_TMSupplierDetail_TMSupplier");
        });

        modelBuilder.Entity<TMSupplierType>(entity =>
        {
            entity.HasKey(e => e.SupplierTypeID).HasName("PK_TMVendorType");

            entity.Property(e => e.SupplierTypeID).ValueGeneratedNever();
            entity.Property(e => e.Description).IsFixedLength();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMUnitOfMeasure>(entity =>
        {
            entity.Property(e => e.UnitOfMeasureID).ValueGeneratedNever();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMUser>(entity =>
        {
            entity.Property(e => e.ApproveStatus).HasDefaultValueSql("((0))");
            entity.Property(e => e.Password).IsFixedLength();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Role).WithMany(p => p.TMUsers).HasConstraintName("FK_TMUsers_TMRoles");
        });

        modelBuilder.Entity<TMUserInBranch>(entity =>
        {
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Branch).WithMany(p => p.TMUserInBranches).HasConstraintName("FK_TMUserInBranchs_TMBranch");

            entity.HasOne(d => d.User).WithMany(p => p.TMUserInBranches).HasConstraintName("FK_TMUserInBranchs_TMUsers");
        });

        modelBuilder.Entity<TMWarehouse>(entity =>
        {
            entity.Property(e => e.WarehouseID).ValueGeneratedNever();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Branch).WithMany(p => p.TMWarehouses).HasConstraintName("FK_TMWarehouse_TMBranch");
        });

        modelBuilder.Entity<TTPurchaseOrder>(entity =>
        {
            entity.Property(e => e.PurchaseOrderID).ValueGeneratedNever();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Currency).WithMany(p => p.TTPurchaseOrders).HasConstraintName("FK_TTPurchaseOrder_TMCurrency");

            entity.HasOne(d => d.PaymenType).WithMany(p => p.TTPurchaseOrders).HasConstraintName("FK_TTPurchaseOrder_TMPaymentType");

            entity.HasOne(d => d.PurchaseType).WithMany(p => p.TTPurchaseOrders).HasConstraintName("FK_TTPurchaseOrder_TMPurchaseType");

            entity.HasOne(d => d.Supplier).WithMany(p => p.TTPurchaseOrders).HasConstraintName("FK_TTPurchaseOrder_TMSupplier");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.TTPurchaseOrders).HasConstraintName("FK_TTPurchaseOrder_TMWarehouse");
        });

        modelBuilder.Entity<TTPurchaseOrderDetail>(entity =>
        {
            entity.Property(e => e.PurchaseOrderDetailID).ValueGeneratedNever();
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.TTPurchaseOrderDetails).HasConstraintName("FK_TTPurchaseOrderDetail_TTPurchaseOrder");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
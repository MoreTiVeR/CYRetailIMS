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

    public virtual DbSet<TMApproveStatus> TMApproveStatuses { get; set; }

    public virtual DbSet<TMBranch> TMBranches { get; set; }

    public virtual DbSet<TMBranchDetail> TMBranchDetails { get; set; }

    public virtual DbSet<TMCurrency> TMCurrencies { get; set; }

    public virtual DbSet<TMDepartment> TMDepartments { get; set; }

    public virtual DbSet<TMEmployee> TMEmployees { get; set; }

    public virtual DbSet<TMItem> TMItems { get; set; }

    public virtual DbSet<TMItemBrand> TMItemBrands { get; set; }

    public virtual DbSet<TMItemInBranch> TMItemInBranches { get; set; }

    public virtual DbSet<TMItemPromotion> TMItemPromotions { get; set; }

    public virtual DbSet<TMItemPromotionDetail> TMItemPromotionDetails { get; set; }

    public virtual DbSet<TMItemType> TMItemTypes { get; set; }

    public virtual DbSet<TMMenus> TMMenus { get; set; }

    public virtual DbSet<TMPaymentType> TMPaymentTypes { get; set; }

    public virtual DbSet<TMPurchaseType> TMPurchaseTypes { get; set; }

    public virtual DbSet<TMRole> TMRoles { get; set; }

    public virtual DbSet<TMRoleInMenu> TMRoleInMenus { get; set; }

    public virtual DbSet<TMShipmentType> TMShipmentTypes { get; set; }

    public virtual DbSet<TMStock> TMStocks { get; set; }

    public virtual DbSet<TMStockType> TMStockTypes { get; set; }

    public virtual DbSet<TMSubMenus> TMSubMenus { get; set; }

    public virtual DbSet<TMSupplier> TMSuppliers { get; set; }

    public virtual DbSet<TMSupplierContact> TMSupplierContacts { get; set; }

    public virtual DbSet<TMSupplierContactType> TMSupplierContactTypes { get; set; }

    public virtual DbSet<TMSupplierDetail> TMSupplierDetails { get; set; }

    public virtual DbSet<TMSupplierType> TMSupplierTypes { get; set; }

    public virtual DbSet<TMTransactionType> TMTransactionTypes { get; set; }

    public virtual DbSet<TMTransferType> TMTransferTypes { get; set; }

    public virtual DbSet<TMUnitOfMeasure> TMUnitOfMeasures { get; set; }

    public virtual DbSet<TMUsers> TMUsers { get; set; }

    public virtual DbSet<TMUserInBranch> TMUserInBranchs { get; set; }

    public virtual DbSet<TMWarehouse> TMWarehouses { get; set; }

    public virtual DbSet<TTItemTransfer> TTItemTransfers { get; set; }

    public virtual DbSet<TTPurchaseOrder> TTPurchaseOrders { get; set; }

    public virtual DbSet<TTPurchaseOrderDetail> TTPurchaseOrderDetails { get; set; }

    public virtual DbSet<TTShipment> TTShipments { get; set; }

    public virtual DbSet<TTStockTransaction> TTStockTransactions { get; set; }

    public virtual DbSet<TTTransaction> TTTransactions { get; set; }

	public virtual DbSet<TTTransactionAudit> TTTransactionAudits { get; set; }

	public virtual DbSet<TTTransactonDetail> TTTransactonDetails { get; set; }

    public virtual DbSet<TMGeography> TMGeographies { get; set; }

    public virtual DbSet<TMProvince> TMProvinces { get; set; }

    public virtual DbSet<TMDistrict> TMDistricts { get; set; }

    public virtual DbSet<TMSubDistrict> TMSubDistricts { get; set; }

    public virtual DbSet<TMAdjustItemType> TMAdjustItemTypes { get; set; }

    public virtual DbSet<TTAdjustItemTransaction> TTAdjustItemTransactions { get; set; }

	public virtual DbSet<TMTransportCompany> TMTransportCompanies { get; set; }

	public virtual DbSet<TMTransportPrefixDetail> TMTransportPrefixDetails { get; set; }

    public virtual DbSet<TTDraftItemTransfer> TTDraftItemTransfers { get; set; }

    public virtual DbSet<TTItemTransactionLog> TTItemTransactionLogs { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TMAdjustItemType>(entity =>
        {
            entity.HasKey(e => e.AdjustTypeID).HasName("PK_TMAdjustType");

            entity.Property(e => e.AdjustTypeID).ValueGeneratedNever();
        });

        modelBuilder.Entity<TMApproveStatus>(entity =>
        {
            entity.Property(e => e.ApproveStatusID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMBranch>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMBranchDetail>(entity =>
        {
            entity.Property(e => e.BranchID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Branch).WithOne(p => p.TMBranchDetail).HasConstraintName("FK_TMBranchDetail_TMBranch");
        });

        modelBuilder.Entity<TMCurrency>(entity =>
        {
            entity.HasKey(e => e.CurrencyID).HasName("PK_TMCurrencyType");

            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMDepartment>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMEmployee>(entity =>
        {
			entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

			entity.HasOne(d => d.Department).WithMany(p => p.TMEmployees).HasConstraintName("FK_TMEmployee_TMDepartments");

			entity.HasOne(d => d.User).WithMany(p => p.TMEmployees)
				.OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("FK_TMEmployee_TMUsers");
		});

        modelBuilder.Entity<TMItem>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.NotifyMinQty).HasDefaultValueSql("((0))");
            entity.HasOne(d => d.Brand).WithMany(p => p.TMItems).HasConstraintName("FK_TMItem_TMItemBrand");

            entity.HasOne(d => d.ItemType).WithMany(p => p.TMItems).HasConstraintName("FK_TMItem_TMItemType");

            entity.HasOne(d => d.UnitOfMeasure).WithMany(p => p.TMItems).HasConstraintName("FK_TMItem_TMUnitOfMeasure");
        });

        modelBuilder.Entity<TMItemBrand>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMItemInBranch>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Branch).WithMany(p => p.TMItemInBranches).HasConstraintName("FK_TMItemInBranch_TMBranch");

            entity.HasOne(d => d.Item).WithMany(p => p.TMItemInBranches).HasConstraintName("FK_TMItemInBranch_TMItem");
        });

        modelBuilder.Entity<TMItemPromotion>(entity =>
        {
            entity.Property(e => e.PromotionID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMItemPromotionDetail>(entity =>
        {
            entity.HasKey(e => new { e.PromotionID, e.ItemID }).HasName("PK_TMItemPromotionDetail_1");

            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Item).WithMany(p => p.TMItemPromotionDetails).HasConstraintName("FK_TMItemPromotionDetail_TMItem");

            entity.HasOne(d => d.Promotion).WithMany(p => p.TMItemPromotionDetails).HasConstraintName("FK_TMItemPromotionDetail_TMItemPromotion");
        });

		modelBuilder.Entity<TMItemTransferStatus>(entity =>
		{
			entity.Property(e => e.TransferStatusID).ValueGeneratedNever();
			entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
		});

		modelBuilder.Entity<TMItemType>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMMenus>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMPaymentType>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMPurchaseType>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMRole>(entity =>
        {
            entity.HasKey(e => e.RoleID).HasName("PK_Table1");

            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMRoleInMenu>(entity =>
        {
            entity.HasOne(d => d.Menu).WithMany(p => p.TMRoleInMenus).HasConstraintName("FK_TMRoleInMenus_TMMenus");

            entity.HasOne(d => d.Role).WithMany(p => p.TMRoleInMenus).HasConstraintName("FK_TMRoleInMenus_TMRoles");

            entity.HasOne(d => d.SubMenu).WithMany(p => p.TMRoleInMenus).HasConstraintName("FK_TMRoleInMenus_TMSubMenu");

            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMShipmentType>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.ShipmentTypeName).HasComment("ประเภทการขนส่ง ขนส่งทางบก ขนส่งทางน้ำ ขนส่งทางอากาศ ขนส่งระบบคอนเทนเนอร์ ขนส่งพัสดุแบบด่วน(Delivery Express)");
        });

        modelBuilder.Entity<TMStock>(entity =>
        {
            entity.Property(e => e.StockID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.TMStocks).HasConstraintName("FK_TMStock_TMWarehouse");
        });

        modelBuilder.Entity<TMStockType>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMSubMenus>(entity =>
        {
            entity.HasKey(e => e.SubMenuID).HasName("PK_TMSubMenu");

            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMSupplier>(entity =>
        {
            entity.HasKey(e => e.SupplierID).HasName("PK_TMVendor");

            entity.Property(e => e.Description).IsFixedLength();
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.SupplierType).WithMany(p => p.TMSuppliers).HasConstraintName("FK_TMSupplier_TMSupplierType");
        });

        modelBuilder.Entity<TMSupplierContact>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.SupplierContactType).WithMany(p => p.TMSupplierContacts).HasConstraintName("FK_TMSupplierContact_TMSupplierContactType");

            entity.HasOne(d => d.Supplier).WithMany(p => p.TMSupplierContacts).HasConstraintName("FK_TMSupplierContact_TMSupplier");
        });

        modelBuilder.Entity<TMSupplierContactType>(entity =>
        {
            entity.HasKey(e => e.SupplierContactTypeID).HasName("PK_TMVendorContactType");

            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMSupplierDetail>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Supplier).WithMany(p => p.TMSupplierDetails).HasConstraintName("FK_TMSupplierDetail_TMSupplier");
        });

        modelBuilder.Entity<TMSupplierType>(entity =>
        {
            entity.HasKey(e => e.SupplierTypeID).HasName("PK_TMVendorType");

            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMTransactionType>(entity =>
        {
            entity.HasKey(e => e.TransactionTypeID).HasName("PK_TMTTTransactionType");

            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMTransferType>(entity =>
        {
            entity.Property(e => e.TransferTypeID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TMUnitOfMeasure>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.UnitOfMeasureName).HasComment("หน่วยวัด เช่น ชิ้น อัน กล่อง");
        });

        modelBuilder.Entity<TMUsers>(entity =>
        {
            entity.Property(e => e.ApproveStatus).HasDefaultValueSql("((0))");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.Password).IsFixedLength();

            entity.HasOne(d => d.Role).WithMany(p => p.TMUsers).HasConstraintName("FK_TMUsers_TMRoles");
        });

        modelBuilder.Entity<TMUserInBranch>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Branch).WithMany(p => p.TMUserInBranches).HasConstraintName("FK_TMUserInBranchs_TMBranch");

            entity.HasOne(d => d.User).WithMany(p => p.TMUserInBranches).HasConstraintName("FK_TMUserInBranchs_TMUsers");
        });

        modelBuilder.Entity<TMWarehouse>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TTItemTransfer>(entity =>
        {
            entity.Property(e => e.DestinationID).HasComment("WarehouseID, BranchID ปลายทาง");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.SourceID).HasComment("WarehouseID, BranchID ต้นทาง");
            entity.Property(e => e.TransferTypeID).HasComment("Ref TMTransferType");

            entity.HasOne(d => d.TransferType).WithMany(p => p.TTItemTransfers).HasConstraintName("FK_TTItemTransfer_TMTransferType");
        });

        modelBuilder.Entity<TTPurchaseOrder>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Currency).WithMany(p => p.TTPurchaseOrders).HasConstraintName("FK_TTPurchaseOrder_TMCurrency");

            entity.HasOne(d => d.PaymenType).WithMany(p => p.TTPurchaseOrders).HasConstraintName("FK_TTPurchaseOrder_TMPaymentType");

            entity.HasOne(d => d.PurchaseType).WithMany(p => p.TTPurchaseOrders).HasConstraintName("FK_TTPurchaseOrder_TMPurchaseType");

            entity.HasOne(d => d.Supplier).WithMany(p => p.TTPurchaseOrders).HasConstraintName("FK_TTPurchaseOrder_TMSupplier");
        });

        modelBuilder.Entity<TTPurchaseOrderDetail>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.TTPurchaseOrderDetails).HasConstraintName("FK_TTPurchaseOrderDetail_TTPurchaseOrder");
        });

        modelBuilder.Entity<TTShipment>(entity =>
        {
            entity.HasKey(e => e.ShipmentID).HasName("PK_TMShipment");

            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.TTShipments).HasConstraintName("FK_TMShipment_TTPurchaseOrder");

            entity.HasOne(d => d.ShipmentType).WithMany(p => p.TTShipments)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TMShipment_TMShipmentType");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.TTShipments)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TMShipment_TMWarehouse");
        });

        modelBuilder.Entity<TTStockTransaction>(entity =>
        {
            entity.HasKey(e => e.StockTransactionID).HasName("PK_TTStockHistory");

            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.StockTypeID).HasComment("Ref TMStockType In, Out");

            entity.HasOne(d => d.Item).WithMany(p => p.TTStockTransactions).HasConstraintName("FK_TTStockTransaction_TMItem");

            entity.HasOne(d => d.StockType).WithMany(p => p.TTStockTransactions).HasConstraintName("FK_TTStockTransaction_TMStockType");
        });

        modelBuilder.Entity<TTTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionID).HasName("PK_TTSaleTransactions");

            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.TransactionType).WithMany(p => p.TTTransactions).HasConstraintName("FK_TTTransactions_TMTransactionType");
        });

		modelBuilder.Entity<TTTransactionAudit>(entity =>
		{
			entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
		});

		modelBuilder.Entity<TTTransactonDetail>(entity =>
        {
            entity.HasKey(e => e.TransactionDetailID).HasName("PK_TTSaleTransactonDetail");

            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Transaction).WithMany(p => p.TTTransactonDetails).HasConstraintName("FK_TTTransactonDetail_TTTransactions");
        });

        modelBuilder.Entity<TMGeography>(entity =>
        {
            entity.HasKey(e => e.GeoID).HasName("PK_Geography");

            entity.Property(e => e.GeoID).ValueGeneratedNever();
        });

        modelBuilder.Entity<TMProvince>(entity =>
        {
            entity.HasKey(e => new { e.ProvinceID, e.ProvinceCode, e.GeoID }).HasName("PK_Province");
        });

        modelBuilder.Entity<TMDistrict>(entity =>
        {
            entity.HasKey(e => new { e.SubDistrictID, e.SubDistrictCode, e.GeoID, e.ProvinceID }).HasName("PK_Amphur_1");
        });

        modelBuilder.Entity<TMSubDistrict>(entity =>
        {
            entity.HasKey(e => new { e.DistrictID, e.DistrictCode, e.SubDistrictID, e.ProvinceID, e.GeoID }).HasName("PK_District");
        });

        modelBuilder.Entity<TTAdjustItemTransaction>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

		modelBuilder.Entity<TMTransportCompany>(entity =>
		{
			entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
		});

		modelBuilder.Entity<TMTransportPrefixDetail>(entity =>
		{
			entity.Property(e => e.TransportPrefixID).ValueGeneratedNever();
			entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

			entity.HasOne(d => d.Transport).WithMany(p => p.TMTransportPrefixDetails)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_TMTransportPrefixDetail_TMTransportCompany");
		});

        modelBuilder.Entity<TTDraftItemTransfer>(entity =>
        {
            entity.Property(e => e.DestinationID).HasComment("WarehouseID, BranchID ปลายทาง");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.SourceID).HasComment("WarehouseID, BranchID ต้นทาง");
            entity.Property(e => e.TransferTypeID).HasComment("Ref TMTransferType");
        });

        modelBuilder.Entity<TTItemTransactionLog>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
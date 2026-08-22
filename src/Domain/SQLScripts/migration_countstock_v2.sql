-- ============================================================
-- Migration: Add CountStockStatusID, CounterRole, ApprovedBy, ApprovedDate
--            to TTCountStocks table
--            Add ItemRemark to TTCountStockDetail table
-- Feature:   ระบบนับสต๊อกแบบใหม่ (Count Stock v2)
-- Date:      2026-08-17
-- ============================================================

-- -------------------------------------------------------
-- 1.) TTCountStocks — เพิ่มคอลัมน์สำหรับ workflow ใหม่
-- -------------------------------------------------------
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'TTCountStocks' AND COLUMN_NAME = 'CountStockStatusID'
)
BEGIN
    ALTER TABLE TTCountStocks
        ADD CountStockStatusID INT NOT NULL DEFAULT 0;
        -- 0 = Draft, 1 = Submitted (รออนุมัติ), 2 = Approved (อนุมัติแล้ว)
    PRINT 'Added CountStockStatusID to TTCountStocks';
END;
GO

IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'TTCountStocks' AND COLUMN_NAME = 'CounterRole'
)
BEGIN
    ALTER TABLE TTCountStocks
        ADD CounterRole VARCHAR(20) NULL;
        -- 'PC' = พนักงานขาย, 'HeadPC' = หัวหน้า PC
    PRINT 'Added CounterRole to TTCountStocks';
END;
GO

IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'TTCountStocks' AND COLUMN_NAME = 'ApprovedBy'
)
BEGIN
    ALTER TABLE TTCountStocks
        ADD ApprovedBy VARCHAR(10) NULL;
    PRINT 'Added ApprovedBy to TTCountStocks';
END;
GO

IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'TTCountStocks' AND COLUMN_NAME = 'ApprovedDate'
)
BEGIN
    ALTER TABLE TTCountStocks
        ADD ApprovedDate DATETIME NULL;
    PRINT 'Added ApprovedDate to TTCountStocks';
END;
GO

-- -------------------------------------------------------
-- 2.) TTCountStockDetail — เพิ่ม ItemRemark สำหรับกรณีนับได้ 0
-- -------------------------------------------------------
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'TTCountStockDetail' AND COLUMN_NAME = 'ItemRemark'
)
BEGIN
    ALTER TABLE TTCountStockDetail
        ADD ItemRemark VARCHAR(200) NULL;
        -- หมายเหตุรายการ บังคับกรอกเมื่อ CountedAmountQty = 0
    PRINT 'Added ItemRemark to TTCountStockDetail';
END;
GO

-- -------------------------------------------------------
-- 3.) Update existing records to default status (0 = Draft migrated as Submitted)
--     Existing records are treated as already submitted (status = 1)
-- -------------------------------------------------------
UPDATE TTCountStocks
SET CountStockStatusID = 1,
    CounterRole = 'PC'
WHERE CountStockStatusID = 0
  AND CreatedDate < '2026-08-17';
GO

PRINT 'Migration completed: CountStock v2 schema changes applied';

-- ============================================================
-- Menu Script — เพิ่มเมนูนับสต๊อกใหม่ใน TMMenus / TMSubMenus
-- ============================================================
-- NOTE: ปรับ MenuID, RoleID ให้ตรงกับ DB จริง ก่อน run script นี้
-- -------------------------------------------------------

-- ตัวอย่าง: เพิ่ม submenu ใน parent menu "สต๊อก" (ปรับ ParentMenuID ให้ถูกต้อง)
-- ตรวจสอบ ID ของ menu สต๊อกก่อน:
-- SELECT * FROM TMMenus WHERE MenuName_TH LIKE '%สต๊อก%'
-- SELECT * FROM TMSubMenus WHERE CMS_ControllerName = 'Stock'

-- เพิ่มเมนูหน้านับสต๊อก (ใหม่) สำหรับ PC / HeadPC
IF NOT EXISTS (
    SELECT 1 FROM TMSubMenus
    WHERE CMS_ControllerName = 'Stock' AND CMS_ActionName = 'NewCountStockEntry'
)
BEGIN
    INSERT INTO TMSubMenus (
        MenuID,
        Seq,
        MenuName_TH,
        MenuName_EN,
        CMS_ControllerName,
        CMS_ActionName,
        Description,
        IsActive,
        CreatedBy,
        CreatedDate
    )
    SELECT TOP 1
        m.MenuID,
        45,
        N'นับสต๊อก (ใหม่)',
        'New Count Stock',
        'Stock',
        'NewCountStockEntry',
        N'หน้ากรอกข้อมูลนับสต๊อกแบบใหม่ สำหรับ PC และหัวหน้า PC',
        1,
        'SYSTEM',
        GETDATE()
    FROM TMMenus m
    WHERE m.MenuName_TH LIKE N'%สต็อกสินค้า%'
    ORDER BY m.MenuID;

    PRINT 'Added submenu: NewCountStockEntry';
END;
GO

-- เพิ่มเมนูหน้าเทียบข้อมูล
IF NOT EXISTS (
    SELECT 1 FROM TMSubMenus
    WHERE CMS_ControllerName = 'Stock' AND CMS_ActionName = 'CountStockCompare'
)
BEGIN
    INSERT INTO TMSubMenus (
        MenuID,
        Seq,
        MenuName_TH,
        MenuName_EN,
        CMS_ControllerName,
        CMS_ActionName,
        Description,
        IsActive,
        CreatedBy,
        CreatedDate
    )
    SELECT TOP 1
        m.MenuID,
        46,
        N'เทียบข้อมูลสต๊อก',
        'Stock Comparison',
        'Stock',
        'CountStockCompare',
        N'หน้าเปรียบเทียบสต๊อกระบบกับยอดที่นับได้จริง',
        1,
        'SYSTEM',
        GETDATE()
    FROM TMMenus m
    WHERE m.MenuName_TH LIKE N'%สต็อกสินค้า%'
    ORDER BY m.MenuID;

    PRINT 'Added submenu: CountStockCompare';
END;
GO

-- เพิ่มเมนูหน้ารออนุมัติ
IF NOT EXISTS (
    SELECT 1 FROM TMSubMenus
    WHERE CMS_ControllerName = 'Stock' AND CMS_ActionName = 'CountStockPendingApproval'
)
BEGIN
    INSERT INTO TMSubMenus (
        MenuID,
        Seq,
        MenuName_TH,
        MenuName_EN,
        CMS_ControllerName,
        CMS_ActionName,
        Description,
        IsActive,
        CreatedBy,
        CreatedDate
    )
    SELECT TOP 1
        m.MenuID,
        47,
        N'รออนุมัตินับสต๊อก',
        'Stock Pending Approval',
        'Stock',
        'CountStockPendingApproval',
        N'หน้ารายการนับสต๊อกที่รออนุมัติ',
        1,
        'SYSTEM',
        GETDATE()
    FROM TMMenus m
    WHERE m.MenuName_TH LIKE N'%สต็อกสินค้า%'
    ORDER BY m.MenuID;

    PRINT 'Added submenu: CountStockPendingApproval';
END;
GO

PRINT 'Menu script completed.';

--- INSERT MENU IN TMRoleInMenus for RoleID = 2 (PC) and RoleID = 3 (HeadPC)
INSERT INTO TMRoleInMenus(RoleID, MenuID, SubMenuID, CanView, CanCreate, CanEdit, CanDelete, CreatedBy, CreatedDate, IsActive)
VALUES (2, 3, 48, 1, 1, 1, 1, 'SYSTEM', GETDATE(), 1)
INSERT INTO TMRoleInMenus(RoleID, MenuID, SubMenuID, CanView, CanCreate, CanEdit, CanDelete, CreatedBy, CreatedDate, IsActive)
VALUES (2, 3, 49, 1, 1, 1, 1, 'SYSTEM', GETDATE(), 1)
INSERT INTO TMRoleInMenus(RoleID, MenuID, SubMenuID, CanView, CanCreate, CanEdit, CanDelete, CreatedBy, CreatedDate, IsActive)
VALUES (2, 3, 50, 1, 1, 1, 1, 'SYSTEM', GETDATE(), 1)
-- ============================================================
-- Migration: Count Stock V2 — Complete Schema
-- Feature:   ระบบนับสต๊อกแบบใหม่ (workflow Draft → Submit → Approve)
-- Date:      2026-09-01
-- Idempotent: ทุก statement ใช้ IF NOT EXISTS — run ซ้ำได้ปลอดภัย
-- ============================================================

BEGIN TRANSACTION;

-- ============================================================
-- 1. TTCountStocks — Workflow columns
-- ============================================================

IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'TTCountStocks' AND COLUMN_NAME = 'CountStockStatusID'
)
BEGIN
    ALTER TABLE TTCountStocks
        ADD CountStockStatusID INT NOT NULL CONSTRAINT DF_TTCountStocks_StatusID DEFAULT 0;
    -- 0 = Draft, 1 = Submitted (รออนุมัติ), 2 = Approved (อนุมัติแล้ว)
    PRINT 'Added: TTCountStocks.CountStockStatusID';
END;

IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'TTCountStocks' AND COLUMN_NAME = 'CounterRole'
)
BEGIN
    ALTER TABLE TTCountStocks
        ADD CounterRole VARCHAR(20) NULL;
    -- 'PC' = พนักงานขาย, 'HeadPC' = หัวหน้า PC
    PRINT 'Added: TTCountStocks.CounterRole';
END;

IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'TTCountStocks' AND COLUMN_NAME = 'ApprovedBy'
)
BEGIN
    ALTER TABLE TTCountStocks
        ADD ApprovedBy VARCHAR(10) NULL;
    PRINT 'Added: TTCountStocks.ApprovedBy';
END;

IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'TTCountStocks' AND COLUMN_NAME = 'ApprovedDate'
)
BEGIN
    ALTER TABLE TTCountStocks
        ADD ApprovedDate DATETIME NULL;
    PRINT 'Added: TTCountStocks.ApprovedDate';
END;

-- ============================================================
-- 2. TTCountStockDetail — Per-item support columns
-- ============================================================

IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'TTCountStockDetail' AND COLUMN_NAME = 'ItemID'
)
BEGIN
    ALTER TABLE TTCountStockDetail
        ADD ItemID INT NULL;
    -- FK ไปยัง TMItem.ItemID — NULL สำหรับ V1 legacy rows ที่บันทึกด้วย SubItemTypeID เท่านั้น
    PRINT 'Added: TTCountStockDetail.ItemID';
END;

IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'TTCountStockDetail' AND COLUMN_NAME = 'ItemRemark'
)
BEGIN
    ALTER TABLE TTCountStockDetail
        ADD ItemRemark VARCHAR(200) NULL;
    -- บังคับกรอกเมื่อ CountedAmountQty = 0 (enforce ที่ Application layer)
    PRINT 'Added: TTCountStockDetail.ItemRemark';
END;

-- ============================================================
-- 3. Indexes — Query performance
-- ============================================================

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name    = 'IX_TTCountStockDetail_ItemID'
      AND object_id = OBJECT_ID('TTCountStockDetail')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_TTCountStockDetail_ItemID
        ON TTCountStockDetail (ItemID)
        INCLUDE (CountStockID, SubItemTypeID, TotalCountQty);
    PRINT 'Added: IX_TTCountStockDetail_ItemID';
END;

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name    = 'IX_TTCountStocks_BranchID_Status_Role'
      AND object_id = OBJECT_ID('TTCountStocks')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_TTCountStocks_BranchID_Status_Role
        ON TTCountStocks (BranchID, CountStockStatusID, CounterRole)
        INCLUDE (CountDate, CreatedBy, ApprovedBy);
    PRINT 'Added: IX_TTCountStocks_BranchID_Status_Role';
END;

-- ============================================================
-- 4. Data Migration — Backfill existing rows
--    บันทึกเก่า (ก่อนวันที่ deploy V2) ให้ถือว่า Submitted และเป็น PC
--    ใช้ dynamic SQL เพราะคอลัมน์อาจถูกเพิ่งสร้างใน batch เดียวกัน
-- ============================================================

COMMIT TRANSACTION;   -- commit schema changes first, then backfill in a new batch

PRINT '=== Schema changes committed. Running data backfill... ===';
GO

BEGIN TRANSACTION;

DECLARE @rows INT = 0;

-- Dynamic SQL ป้องกัน parse-time error ก่อนที่คอลัมน์จะถูกสร้าง
EXEC sp_executesql N'
    UPDATE TTCountStocks
    SET    CountStockStatusID = 1,
           CounterRole        = ''PC''
    WHERE  CountStockStatusID = 0
      AND  CounterRole        IS NULL
      AND  CreatedDate        < ''2026-08-17''';

SET @rows = @@ROWCOUNT;
PRINT 'Backfilled legacy TTCountStocks rows → status=1, role=PC';
PRINT CAST(@rows AS VARCHAR(10)) + ' rows updated';

COMMIT TRANSACTION;

PRINT '=== Migration complete: Count Stock V2 ===';
GO

-- ============================================================
-- 5. Verification — ตรวจสอบผลลัพธ์หลัง migration
-- ============================================================

SELECT
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME IN ('TTCountStocks', 'TTCountStockDetail')
  AND COLUMN_NAME IN (
        'CountStockStatusID', 'CounterRole', 'ApprovedBy', 'ApprovedDate',
        'ItemID', 'ItemRemark'
  )
ORDER BY TABLE_NAME, COLUMN_NAME;
GO

-- Status distribution ของข้อมูลปัจจุบัน
SELECT
    CountStockStatusID,
    CounterRole,
    COUNT(*)        AS RecordCount,
    MIN(CountDate)  AS EarliestDate,
    MAX(CountDate)  AS LatestDate
FROM TTCountStocks
GROUP BY CountStockStatusID, CounterRole
ORDER BY CountStockStatusID, CounterRole;
GO

-- ============================================================
-- ROLLBACK SCRIPT (ใช้เฉพาะกรณีต้องการ revert)
-- ============================================================
/*
BEGIN TRANSACTION;

ALTER TABLE TTCountStocks    DROP CONSTRAINT IF EXISTS DF_TTCountStocks_StatusID;
ALTER TABLE TTCountStocks    DROP COLUMN IF EXISTS CountStockStatusID;
ALTER TABLE TTCountStocks    DROP COLUMN IF EXISTS CounterRole;
ALTER TABLE TTCountStocks    DROP COLUMN IF EXISTS ApprovedBy;
ALTER TABLE TTCountStocks    DROP COLUMN IF EXISTS ApprovedDate;

ALTER TABLE TTCountStockDetail DROP COLUMN IF EXISTS ItemID;
ALTER TABLE TTCountStockDetail DROP COLUMN IF EXISTS ItemRemark;

DROP INDEX IF EXISTS IX_TTCountStockDetail_ItemID              ON TTCountStockDetail;
DROP INDEX IF EXISTS IX_TTCountStocks_BranchID_Status_Role     ON TTCountStocks;

COMMIT TRANSACTION;

PRINT 'ROLLBACK complete';
*/

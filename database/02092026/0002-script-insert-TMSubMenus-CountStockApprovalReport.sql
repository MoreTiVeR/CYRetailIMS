USE CYDB;
GO

DECLARE @StockMenuID INT;
-- SELECT TOP 1 @StockMenuID = MenuID
-- FROM dbo.TMSubMenus
-- WHERE CMS_ControllerName = 'Stock'
--   AND CMS_ActionName IN ('CountStockPendingApproval', 'NewCountStockEntry', 'Index')
-- ORDER BY MenuID;

IF @StockMenuID IS NULL
BEGIN
    SELECT TOP 1 @StockMenuID = MenuID
    FROM dbo.TMMenus
    WHERE MenuName_EN LIKE N'%Report%'
    ORDER BY MenuID;
END;

IF @StockMenuID IS NOT NULL
AND NOT EXISTS (
    SELECT 1
    FROM dbo.TMSubMenus
    WHERE CMS_ControllerName = 'Report'
      AND CMS_ActionName = 'CountStockApprovalReport'
)
BEGIN
    INSERT INTO dbo.TMSubMenus
    (
        MenuID,
        Seq,
        MenuName_EN,
        MenuName_TH,
        Description,
        CMS_ControllerName,
        CMS_ActionName,
        CreatedBy,
        CreatedDate,
        IsActive
    )
    VALUES
    (
        @StockMenuID,
        48,
        'CountStock Approval Report',
        N'รายงานอนุมัตินับสต๊อก',
        N'รายงานประวัติการอนุมัตินับสต๊อกที่ผ่านเมนูรออนุมัติ',
        'Report',
        'CountStockApprovalReport',
        'admin',
        GETDATE(),
        1
    );
END;

-- Migrate existing submenu that was previously pointed to Stock controller
UPDATE dbo.TMSubMenus
SET CMS_ControllerName = 'Report',
    UpdatedBy = 'admin',
    UpdatedDate = GETDATE()
WHERE CMS_ActionName = 'CountStockApprovalReport'
  AND CMS_ControllerName = 'Stock';
GO

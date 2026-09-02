USE CYDB;
GO

IF NOT EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_NAME = 'TTCountStockApprovalHistory'
)
BEGIN
    CREATE TABLE dbo.TTCountStockApprovalHistory
    (
        CountStockApprovalHistoryID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        CountStockID INT NOT NULL,
        CountStockDetailID INT NOT NULL,
        BranchID INT NOT NULL,
        ItemID INT NOT NULL,
        SubItemTypeID INT NOT NULL,
        QtyInBranchOfCountStockDay INT NOT NULL,
        QtyInBranchBeforeApprove INT NOT NULL,
        QtyInBranchAfterApprove INT NOT NULL,
        CountedAmountQty INT NOT NULL,
        PendingReStockQty INT NOT NULL,
        DamagedQty INT NOT NULL,
        SaleBeforeCountQty INT NOT NULL,
        TotalCountQty INT NOT NULL,
        ShortageSurplusQty INT NOT NULL,
        ItemRemark VARCHAR(200) NULL,
        CounterRole VARCHAR(20) NOT NULL,
        ApprovedBy VARCHAR(10) NOT NULL,
        CountStockDate DATETIME NOT NULL,
        ApprovedDate DATETIME NOT NULL,
        CreatedBy VARCHAR(10) NOT NULL,
        CreatedDate DATETIME NOT NULL,
        UpdatedBy VARCHAR(10) NULL,
        UpdatedDate DATETIME NULL,
        IsActive BIT NOT NULL CONSTRAINT DF_TTCountStockApprovalHistory_IsActive DEFAULT(1)
    );
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_TTCountStockApprovalHistory_TTCountStocks'
)
BEGIN
    ALTER TABLE dbo.TTCountStockApprovalHistory
        ADD CONSTRAINT FK_TTCountStockApprovalHistory_TTCountStocks
        FOREIGN KEY (CountStockID) REFERENCES dbo.TTCountStocks(CountStockID);
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_TTCountStockApprovalHistory_Branch_ApprovedDate'
      AND object_id = OBJECT_ID('dbo.TTCountStockApprovalHistory')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_TTCountStockApprovalHistory_Branch_ApprovedDate
        ON dbo.TTCountStockApprovalHistory (BranchID, ApprovedDate)
        INCLUDE (CountStockID, ItemID, QtyInBranchBeforeApprove, QtyInBranchAfterApprove, IsActive);
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_TTCountStockApprovalHistory_CountStockID'
      AND object_id = OBJECT_ID('dbo.TTCountStockApprovalHistory')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_TTCountStockApprovalHistory_CountStockID
        ON dbo.TTCountStockApprovalHistory (CountStockID)
        INCLUDE (CountStockDetailID, ItemID, SubItemTypeID, ApprovedDate, IsActive);
END;
GO

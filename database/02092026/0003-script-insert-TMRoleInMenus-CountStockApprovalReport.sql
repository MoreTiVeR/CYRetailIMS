USE CYDB;
GO

DECLARE @SubMenuID INT;
DECLARE @MenuID INT;

SELECT TOP 1
    @SubMenuID = SubMenuID,
    @MenuID = MenuID
FROM dbo.TMSubMenus
WHERE CMS_ControllerName = 'Report'
  AND CMS_ActionName = 'CountStockApprovalReport';

IF @SubMenuID IS NOT NULL
AND NOT EXISTS (
    SELECT 1
    FROM dbo.TMRoleInMenus
    WHERE RoleID = 1
      AND MenuID = @MenuID
      AND SubMenuID = @SubMenuID
)
BEGIN
    INSERT INTO dbo.TMRoleInMenus
    (
        RoleID,
        MenuID,
        SubMenuID,
        CanView,
        CanCreate,
        CanEdit,
        CanDelete,
        CreatedBy,
        CreatedDate,
        IsActive
    )
    VALUES
    (
        1,
        @MenuID,
        @SubMenuID,
        1,
        1,
        1,
        1,
        'admin',
        GETDATE(),
        1
    );
END;
GO
